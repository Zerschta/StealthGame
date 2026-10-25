using System.Text;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
public class Enemy_Movement : MonoBehaviour
{
    public float speed = 2f;
    public float runningSpeed = 3;
    public float minTime = 1f;   
    public float maxTime = 3f;

    public float rotationspeed = 5f;
    public float minCooldown = 0.3f;
    public float maxCooldown = 2f;
    public float cooldown;
    private Quaternion targetRotation;

    private float timer = 0f;
    private float closedcooldown;

    public bool FoundPlayer = false;
    public bool EnemyGotDamage  = false;
    public float maxDistance = 5f;

    float range = 5f;

    [SerializeField] LayerMask layerMask;
    [SerializeField] FieldOfViewEnemy FieldOfView;
    [SerializeField] GameObject player;
    
    void Start()
    {
        player = GameObject.Find("Player");
        if (player == null) Debug.LogError("Player nicht gefunden!");
        NewDirection();
    }

    void Update()
    {
        FoundPlayer = FieldOfView.CheckRays();

        if (EnemyGotDamage == true)
        {
            FoundPlayer = EnemyGotDamage;

            if (player.transform.gameObject.activeSelf == false)
            {
                EnemyGotDamage = false;
            }
        }

        timer -= Time.deltaTime;

        RayCast();
        if (FoundPlayer)
        {
            FollowPlayer();
            speed = runningSpeed;
            return;
        }
        else speed = speed;

        if (timer <= 0f)
        {
            closedcooldown -= Time.deltaTime;
            if (closedcooldown <= 0f)
            {
                NewDirection();
                closedcooldown = cooldown;
            }
        }
        else if (timer > 0f)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation , Time.deltaTime * rotationspeed);

        
    }



    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        Vector2 richtung = player.transform.position - transform.position;
        transform.up = richtung;

        float distance = Vector2.Distance(transform.position, player.transform.position);
    }

    void NewDirection()
    {
        float angle = Random.Range(0f, 360f);
        targetRotation = Quaternion.Euler(0f, 0f, angle);
        timer = Random.Range(minTime, maxTime);
        cooldown = Random.Range(minCooldown, maxCooldown);
    }

    void RayCast()
    {
        Vector2 start = transform.position;
        Vector2 direction = transform.up;

        RaycastHit2D hit = Physics2D.Raycast(
            start,
            direction,
            range,
            layerMask
        );

        Debug.DrawRay(start, direction * range, Color.red);

        if (hit.collider != null &&
            hit.collider.CompareTag("Wall"))
        {
            
            Vector2 awayDirection = hit.normal;

            float angle = Mathf.Atan2(
                awayDirection.y,
                awayDirection.x
            ) * Mathf.Rad2Deg;

            
            targetRotation = Quaternion.Euler(
                0f, 0f, angle - 90f
            );
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up * range);
    }
}
