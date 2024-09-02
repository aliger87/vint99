using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;         // اللاعب الذي ستتبع الكاميرا
    public Vector3 offset = new Vector3(0, 5, -10); // الإزاحة بين الكاميرا واللاعب
    public float smoothSpeed = 0.125f; // سرعة التنعيم للحركة

    public float rotationSpeed = 250.0f; // سرعة التدوير حول اللاعب

    private void LateUpdate()
    {
        // تتبع اللاعب
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // تدوير الكاميرا حول اللاعب
        if (Input.GetMouseButton(1)) // تدوير عند الضغط على زر الماوس الأيمن
        {
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.RotateAround(player.position, Vector3.up, horizontalInput);
        }

        // الاتجاه نحو اللاعب
        transform.LookAt(player);
    }
}
