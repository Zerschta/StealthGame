using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FieldOfViewEnemy : MonoBehaviour
{
    public LayerMask obstacleMask;
    public class RayData
    {
        public Vector2 origin;
        public Vector2 direction;
        public Vector2 endPoint;

        public RayData(Vector2 origin, Vector2 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }
    }
   
    public List<RayData> rays = new List<RayData>();

    public float spread = 90f;
    public int rayCount = 20;
    public float maxDistance = 10f;

    void CreateRays()
    {
        rays.Clear();

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -spread / 2f
                        + i * spread / (rayCount - 1f);

            Vector2 direction =
                Quaternion.Euler(0f, 0f, angle + 90)
                * transform.right;

            rays.Add(new RayData(
                transform.position,
                direction
            ));
        }
    }
    public bool CheckRays()
    {
        bool playerDetected = false;

        foreach (RayData ray in rays)
        {
            RaycastHit2D hit = Physics2D.Raycast(
                ray.origin,
                ray.direction,
                maxDistance,
                obstacleMask
            );

            
            if (hit.collider != null)
            {
                ray.endPoint = hit.point;

                if (hit.collider.CompareTag("Player"))
                {
                    playerDetected = true;
                }
            }
            else
            {
                ray.endPoint =
                    ray.origin + ray.direction * maxDistance;
            }
        }
        return playerDetected;
    }

    void DrawRays()
    {
        foreach (RayData ray in rays)
        {
            Debug.DrawLine(
                ray.origin,
                ray.endPoint,
                Color.green
            );
        }
    }
    void Update()
    {
        CreateRays();
        CheckRays();
        DrawRays();
    }

}
