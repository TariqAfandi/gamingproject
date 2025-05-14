using UnityEngine;
using Unity.FPS.Game;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("General Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject player;
    public float attackInterval = 3f;

    [Header("Shockwave Settings")]
    public GameObject shockwavePrefab;
    public Transform shockwaveSpawnPoint;

    [Header("Railgun Settings")]
    public GameObject railgunLaserPrefab;
    public Transform[] railgunSpawnPoints;

    private bool shieldActive = true;
    private bool isVulnerable = false;

    void Start()
    {
        Debug.Log("BossAI STARTED");
        currentHealth = maxHealth;

        shieldActive = false;
        isVulnerable = true;

        StartCoroutine(AttackCycle());
    }

    IEnumerator AttackCycle()
    {
        while (true)
        {
            if (shieldActive)
            {
                PerformShockwaveAttack();
            }
            else if (isVulnerable)
            {
                PerformRailgunAttack();
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }

    void PerformShockwaveAttack()
    {
        Debug.Log("Performing Shockwave Attack");
        GameObject shockwave = Instantiate(shockwavePrefab, shockwaveSpawnPoint.position, Quaternion.identity);

        ShockwaveController controller = shockwave.GetComponent<ShockwaveController>();
        if (controller != null)
        {
            controller.ActivateShockwave();
        }
        else
        {
            Debug.LogWarning("ShockwaveController script not found on shockwave prefab!");
        }
    }

    void PerformRailgunAttack()
    {
        if (player == null) return;

        foreach (Transform spawn in railgunSpawnPoints)
        {
            CharacterController col = player.GetComponent<CharacterController>();
            Vector3 targetCenter = player.transform.position + Vector3.up * col.height * 0.5f;

            // Raycast from spawn to player center
            Vector3 direction = (targetCenter - spawn.position).normalized;
            float maxDistance = Vector3.Distance(spawn.position, targetCenter);

            RaycastHit hit;
            Vector3 finalTarget = targetCenter;

            if (Physics.Raycast(spawn.position, direction, out hit, maxDistance))
            {
                // Hit something before reaching player
                finalTarget = hit.point;
            }

            GameObject laser = Instantiate(railgunLaserPrefab, spawn.position, Quaternion.identity);
            RailgunLaser laserScript = laser.GetComponent<RailgunLaser>();
            if (laserScript != null)
            {
                laserScript.Initialize(spawn.position, finalTarget);
            }
        }
    }




    public void DisableShield()
    {
        shieldActive = false;
        isVulnerable = true;

        // Optionally enable effects, sounds, etc
    }

    public void TakeDamage(float amount)
    {
        if (!isVulnerable) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Play explosion, disable enemy, trigger win screen
        Destroy(gameObject);
    }
}
