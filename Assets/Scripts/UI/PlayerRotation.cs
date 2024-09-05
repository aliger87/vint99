using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // سرعة الدوران

    void Update()
    {
        // الحصول على إدخال الدوران من الفأرة أو لوحة المفاتيح
        float rotationInput = Input.GetAxis("Horizontal");

        // حساب الدوران بناءً على الإدخال وسرعة الدوران
        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

        // تدوير اللاعب حول المحور العمودي (Y-axis)
        transform.Rotate(0f, rotationAmount, 0f);
    }
}
