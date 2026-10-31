using UnityEngine;

public class HealObject : MonoBehaviour
{
    [SerializeField] private float Heal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;       

        healthManager healthManager =
            collision.gameObject.GetComponentInChildren<healthManager>();

        if (healthManager == null)
        {
            Debug.LogWarning("HealthManager not found!");
            return;
        }

        healthManager.Heal(Heal);
        Destroy(gameObject);
    }
}
