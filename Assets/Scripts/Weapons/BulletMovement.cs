using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public WeaponData Speed;
    private GameObject Enemy;
    private Enemy_Movement Found;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        movement();
        checkRange();
    }

    void movement()
    {
        transform.position += transform.right * Time.deltaTime * Speed.bulletSpeed;

        
    }

    void checkRange()
    {
        if (Vector3.Distance(startPosition, transform.position) >= Speed.range)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Object") || collision.collider.CompareTag("Player"))
        {
            healthManager life = collision.gameObject.GetComponentInChildren<healthManager>();
            life.TakeDamage(Speed.damage);
            Destroy(gameObject);
        }
        else if(collision.collider.CompareTag("Enemy"))
        {
            healthManager life = collision.gameObject.GetComponentInChildren<healthManager>();
            life.TakeDamage(Speed.damage);
            Destroy(gameObject);

            Enemy = collision.collider.gameObject;
            Found = Enemy.GetComponent<Enemy_Movement>();
            Found.EnemyGotDamage = true;
        }
    }
}