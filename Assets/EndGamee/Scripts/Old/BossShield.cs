using UnityEngine;

public class BossShield : MonoBehaviour
{
    public GameObject shieldVisual; // The octagon shield visual
    public int totalPillars = 4;
    private int remainingPillars;

    public Material shieldNormalMat;
    public Material shieldCrackedMat;
    public Renderer shieldRenderer;

    void Start()
    {
        remainingPillars = totalPillars;
        shieldRenderer.material = shieldNormalMat;
        shieldVisual.SetActive(true);
    }

    public void NotifyPillarDestroyed()
    {
        remainingPillars--;

        if (remainingPillars == 2 && shieldRenderer != null)
        {
            shieldRenderer.material = shieldCrackedMat; // Mid damage visual
        }

        if (remainingPillars <= 0)
        {
            DisableShield();
        }
    }

    void DisableShield()
    {
        // You can animate this or fade it
        shieldVisual.SetActive(false);
        Debug.Log("Shield down! Boss can be damaged.");
    }
}
