using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerMovementController : MonoBehaviour
{
    private Animator animator;
    private CharacterController cc;
    private Camera playerCamera;
    private Transform cameraRoot;
    private CharacterItem groundItem;
    private GameObject groundViewItem;
    public ItemBag bag { get; private set; }

    [Header("Basic")]
    public float walkSpeed = 1.0f;
    public float runSpeed = 3.0f;
    public float mouseSensitivity = 100f;
    public float animatorLerpPower = 8;
    public float gravityScale = 10;
    private float xRotation = 0f;

    [Header("Interaction")]
    public GameObject interactionPanel;
    public Image interactionImage;
    public TextMeshProUGUI interactionText;

    public float pickupRange = 2.0f;

    private CanvasGroup canvasGroup;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;

    public GameObject deathBoxPrefab;

    [Header("Points")]
    public Transform Head;
    public Transform Root;

    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        cameraRoot = playerCamera.transform.parent;
        bag = GetComponent<ItemBag>();

        currentHealth = maxHealth;
        healthSlider.value = CalculateHealth();

        canvasGroup = interactionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = interactionPanel.AddComponent<CanvasGroup>();
        }

        Cursor.lockState = CursorLockMode.Locked;

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

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        animator.SetFloat("InputX", Mathf.Lerp(animator.GetFloat("InputX"), moveHorizontal, Time.deltaTime * animatorLerpPower));
        animator.SetFloat("InputY", Mathf.Lerp(animator.GetFloat("InputY"), moveVertical * (isRunning ? 1 : .5f), Time.deltaTime * animatorLerpPower));

        healthSlider.value = Mathf.Lerp(healthSlider.value, CalculateHealth(), Time.deltaTime * animatorLerpPower);

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        cc.Move(((movement.normalized * currentSpeed) + (Vector3.down * gravityScale)) * Time.deltaTime);

        LookAround();

        if (Input.GetKeyDown(KeyCode.F) && groundItem != null)
            if (bag.AddItem(groundItem))
            {
                Destroy(groundViewItem);
                groundItem.Take(this);
                groundViewItem = null;
                groundItem = null;
            }
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

        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(20f);
        }
        if (other.CompareTag("Item"))
        {
            groundItem = other.GetComponent<ViewItem>().Item.GetComponent<CharacterItem>();
            groundViewItem = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        groundItem = null;
    }

    private void FixedUpdate()
    {
        if (groundItem != null) UpdateInteractionPanel();
        else HideInteractionPanel();
    }

    void UpdateInteractionPanel()
    {
        if (groundItem != null)
        {
            interactionImage.sprite = groundItem.Image;
            interactionText.text = groundItem.name;
            ShowInteractionPanel();
        }
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
        cameraRoot.SetParent(null);

        // إنشاء الصندوق في مكان موت اللاعب
        Instantiate(deathBoxPrefab, deathBoxPrefab.transform.position + transform.position, Quaternion.Euler(deathBoxPrefab.transform.eulerAngles + transform.eulerAngles));
    }
}
