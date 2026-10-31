using UnityEngine;
using UnityEngine.UI;

public class healthManager : MonoBehaviour
{
    public Image HealthBar;
    public float HealthAmount = 100f;
    private float MaxHealth;
    public float yOffset;
    void Start()
    {
        MaxHealth = HealthAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null) return;

        death();
    }

    void LateUpdate()
    {
        transform.position = transform.parent.position + new Vector3(0f, yOffset, 0f);
        transform.rotation = Quaternion.identity;
    }

    public void TakeDamage(float damage)
    { 
        HealthAmount -= damage;
        HealthBar.fillAmount = HealthAmount / MaxHealth;
    }
    
    public void Heal(float HealingAmount)
    {
        HealthAmount += HealingAmount;
        HealthAmount = Mathf.Clamp(HealthAmount, 0, MaxHealth);
        HealthBar.fillAmount = HealthAmount / MaxHealth;
    }

    void death()
    {

        if (HealthAmount <= 0)
        {
            if (transform.parent.CompareTag("Player"))
            { 
                transform.parent.gameObject.SetActive(false);
                return;
            }

            Destroy(transform.parent.gameObject);
        }
    }
}
