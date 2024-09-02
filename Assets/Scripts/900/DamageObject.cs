using UnityEngine;

public class DamageObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovementController player = other.GetComponent<PlayerMovementController>();
            if (player != null)
            {
                player.TakeDamage(20f);
            }
        }
    }
}
