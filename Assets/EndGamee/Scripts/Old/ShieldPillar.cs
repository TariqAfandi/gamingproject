using UnityEngine;

public class ShieldPillar : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public Renderer[] glowIndicators;          // Glowing strip objects on pillar
    public Material activeMaterial;            // Emissive material when active
    public Material destroyedMaterial;         // Non-emissive material when destroyed

    public BossShield bossShield;              // Reference to shield script

    private bool isDestroyed = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Ensure it starts with the active material
        foreach (var rend in glowIndicators)
        {
            rend.material = activeMaterial;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDestroyed) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDestroyed();
        }
    }

    void OnDestroyed()
    {
        isDestroyed = true;

        foreach (var rend in glowIndicators)
        {
            rend.material = destroyedMaterial;
        }

        if (bossShield != null)
        {
            bossShield.NotifyPillarDestroyed();
        }

        // Optional: disable interaction
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    // Optional: to test if damage works without bullets
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // For quick debug testing
        {
            TakeDamage(1);
        }
    }
}
