using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerMovementController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private Camera playerCamera;

    public float walkSpeed = 1.0f;
    public float runSpeed = 3.0f;
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    public GameObject backpackOnGround;
    public GameObject backpackOnPlayer;
    public GameObject helmetOnGround;
    public GameObject helmetOnPlayer;
    public GameObject armorOnGround;
    public GameObject armorOnPlayer;

    public GameObject interactionPanel;
    public Image interactionImage;
    public TextMeshProUGUI interactionText;

    public Sprite backpackImage;
    public Sprite helmetImage;
    public Sprite armorImage;

    public float pickupRange = 2.0f;

    private List<GameObject> objectsOnGround = new List<GameObject>();
    private List<GameObject> objectsOnPlayer = new List<GameObject>();
    private List<Sprite> objectImages = new List<Sprite>();
    private List<string> objectTexts = new List<string>();

    private CanvasGroup canvasGroup;

    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;

    public GameObject deathBoxPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        currentHealth = maxHealth;
        healthSlider.value = CalculateHealth();

        canvasGroup = interactionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = interactionPanel.AddComponent<CanvasGroup>();
        }

        Cursor.lockState = CursorLockMode.Locked;

        if (backpackOnPlayer != null) backpackOnPlayer.SetActive(false);
        if (helmetOnPlayer != null) helmetOnPlayer.SetActive(false);
        if (armorOnPlayer != null) armorOnPlayer.SetActive(false);
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
            canvasGroup.alpha = 0f;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) RotateTowardsMouse();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = transform.TransformDirection(movement);
        float speed = movement.magnitude;

        animator.SetFloat("Motion", speed);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        animator.SetBool("Run", isRunning);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.MovePosition(transform.position + movement.normalized * currentSpeed * Time.deltaTime);

        LookAround();

        if (Input.GetKeyDown(KeyCode.F) && objectsOnGround.Count > 0) PickupItems();
    }

    void RotateTowardsMouse()
    {
        if (playerCamera != null)
        {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (playerPlane.Raycast(ray, out float hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime);
            }
        }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void PickupItems()
    {
        for (int i = 0; i < objectsOnGround.Count; i++)
        {
            objectsOnGround[i].SetActive(false);
            objectsOnPlayer[i].SetActive(true);
        }

        objectsOnGround.Clear();
        objectsOnPlayer.Clear();
        objectImages.Clear();
        objectTexts.Clear();

        HideInteractionPanel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(20f);
        }

        if (other.gameObject == backpackOnGround)
        {
            AddItemToPanel(backpackOnGround, backpackOnPlayer, backpackImage, "ﺔﺒﻴﻘﺤﻟﺍ ﺬﺧﻷ F ﻂﻐﺿﺍ");
        }
        else if (other.gameObject == helmetOnGround)
        {
            AddItemToPanel(helmetOnGround, helmetOnPlayer, helmetImage, "ﺓﺫﻮﺨﻟﺍ ﺬﺧﻷ F ﻂﻐﺿﺍ");
        }
        else if (other.gameObject == armorOnGround)
        {
            AddItemToPanel(armorOnGround, armorOnPlayer, armorImage, "ﻉﺭﺪﻟﺍ ﺬﺧﻷ F ﻂﻐﺿﺍ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsOnGround.Contains(other.gameObject))
        {
            RemoveItemFromPanel(other.gameObject);
        }
    }

    void AddItemToPanel(GameObject groundItem, GameObject playerItem, Sprite itemImage, string itemText)
    {
        objectsOnGround.Add(groundItem);
        objectsOnPlayer.Add(playerItem);
        objectImages.Add(itemImage);
        objectTexts.Add(itemText);
        UpdateInteractionPanel();
    }

    void RemoveItemFromPanel(GameObject groundItem)
    {
        int index = objectsOnGround.IndexOf(groundItem);
        if (index >= 0)
        {
            objectsOnGround.RemoveAt(index);
            objectsOnPlayer.RemoveAt(index);
            objectImages.RemoveAt(index);
            objectTexts.RemoveAt(index);
        }
        if (objectsOnGround.Count == 0)
        {
            HideInteractionPanel();
        }
        else
        {
            UpdateInteractionPanel();
        }
    }

    void UpdateInteractionPanel()
    {
        if (objectImages.Count > 0)
        {
            interactionImage.sprite = objectImages[objectImages.Count - 1];
            interactionText.text = objectTexts[objectTexts.Count - 1];
        }
        ShowInteractionPanel();
    }

    void ShowInteractionPanel()
    {
        interactionPanel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadePanel(canvasGroup, canvasGroup.alpha, 1f, 0.3f));
    }

    void HideInteractionPanel()
    {
        StopAllCoroutines();
        StartCoroutine(FadePanel(canvasGroup, canvasGroup.alpha, 0f, 0.3f, () => interactionPanel.SetActive(false)));
    }

    System.Collections.IEnumerator FadePanel(CanvasGroup cg, float start, float end, float duration, System.Action onComplete = null)
    {
        float counter = 0f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, counter / duration);
            yield return null;
        }
        onComplete?.Invoke();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthSlider.value = CalculateHealth();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    float CalculateHealth()
    {
        return currentHealth / maxHealth;
    }

    void Die()
    {
        // إخفاء اللاعب
        gameObject.SetActive(false);

        // فصل الكاميرا عن اللاعب للحفاظ عليها نشطة
        playerCamera.transform.SetParent(null);

        // إنشاء الصندوق في مكان موت اللاعب
        Instantiate(deathBoxPrefab, transform.position, Quaternion.identity);
    }
}
