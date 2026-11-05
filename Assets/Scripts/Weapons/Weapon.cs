using Unity.Mathematics;
using UnityEngine;


using UnityEngine;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythic
}

public class Weapon : MonoBehaviour
{
    public WeaponData Data;
    public GameObject GunRot;
    public GameObject GunHold;
    public Transform BulletSpawner;

    [Header("Weapon Rarity")]
    public Rarity rarity;
    public float rarityScore;

    private float cooldownTimer;

    private void Start()
    {
        GunRot = GameObject.Find("WeaponRotation");
        GunHold = GameObject.Find("WeaponHolder");
        BulletSpawner = transform.Find("BulletSpawner");

        CalculateRarity();
    }

    private void OnValidate()
    {
        if (Data == null) return;

        CalculateRarity();
    }

    // RARITY SYSTEM

    public void CalculateRarity()
    {
        float dps = (Data.damage * (float)Data.count) / Data.cooldown;

        float D = dps / (50f + dps);
        float P = 1f / (1f + Data.spread / 10f);
        float R = Data.range / (15f + Data.range);
        float V = Data.bulletSpeed / (20f + Data.bulletSpeed);
        float C = Data.count / (1f + Data.count);

        rarityScore = 100f *
            Mathf.Pow(D, 0.40f) *
            Mathf.Pow(P, 0.15f) *
            Mathf.Pow(R, 0.15f) *
            Mathf.Pow(V, 0.10f) *
            Mathf.Pow(C, 0.20f);

        rarity = GetRarity(rarityScore);

        Debug.Log($"Weapon: {Data.name} | Score: {rarityScore:F2} | Rarity: {rarity}");
    }

    private Rarity GetRarity(float score)
    {
        if (score >= 90f) return Rarity.Mythic;
        if (score >= 75f) return Rarity.Legendary;
        if (score >= 60f) return Rarity.Epic;
        if (score >= 40f) return Rarity.Rare;
        if (score >= 25f) return Rarity.Uncommon;

        return Rarity.Common;
    }

    // PLAYER WEAPON LOGIC

    private void Update()
    {
        float magazine = Data.Magazine;

        if (!transform.IsChildOf(GunHold.transform)) return;

        if (magazine > 0)
        {
            cooldownTimer -= Time.deltaTime;

            if ((Input.GetKey(KeyCode.Space) ||
                 Input.GetKey(KeyCode.Mouse0)) && cooldownTimer <= 0f)
            {
                fire();
                cooldownTimer = Data.cooldown;
                Debug.Log("Fire");
                Data.Magazine -= 1;
            }
        }
        else
        {
            Data.Magazine = 0;
            Debug.Log("Emptymag");
        }

        transform.position = GunHold.transform.position;
    }

    public void fire()
    {
        int count = Data.count;
        float spread = Data.spread;
        float startAngle = -spread * (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + spread * i;
            Quaternion rotation = GunRot.transform.rotation *
                                  Quaternion.Euler(0f, 0f, angle);

            GameObject bullet = Instantiate(
                Data.projectilePrefab,
                BulletSpawner.position,
                rotation
            );

            bullet.GetComponent<BulletMovement>().Speed = Data;
        }

        if (Data.shootSound != null)
        {
            AudioSource.PlayClipAtPoint(
                Data.shootSound,
                BulletSpawner.position
            );
        }
    }

    // ENEMY WEAPON LOGIC

    public void Enemyfire()
    {
        float magazine = Data.Magazine;

        int count = Data.count;
        float spread = Data.spread;
        float startAngle = -spread * (count - 1) / 2f;

        if (magazine > 0)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                cooldownTimer = Data.cooldown;

                for (int i = 0; i < count; i++)
                {
                    float angle = startAngle + spread * i;
                    Quaternion rotation = transform.rotation *
                                          Quaternion.Euler(0f, 0f, angle);

                    GameObject bullet = Instantiate(
                        Data.projectilePrefab,
                        BulletSpawner.position,
                        rotation
                    );

                    bullet.GetComponent<BulletMovement>().Speed = Data;
                }

                if (Data.shootSound != null)
                {
                    AudioSource.PlayClipAtPoint(
                        Data.shootSound,
                        BulletSpawner.position
                    );
                }
            }
        }
    }
}
