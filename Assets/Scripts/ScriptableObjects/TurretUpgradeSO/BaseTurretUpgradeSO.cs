using UnityEngine;

public abstract class BaseTurretUpgradeSO : ScriptableObject {
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeSprite;
    public Color upgradeColor;
    public int baseCreditsCost; // Base cost of the upgrade
    public float diminishingFactor;
    public float costScalingFactor; // Factor to scale cost with each application

    public abstract void ApplyUpgrade(Turret turret, int applicationCount);
    public abstract void RevertUpgrade(Turret turret, int applicationCount);

    public abstract float GetBaseMultiplier();

    public float CalculateDiminishingReturn(float baseMultiplier, int applicationCount, float diminishingFactor) {
        return 1.0f + (baseMultiplier - 1.0f) * Mathf.Pow(diminishingFactor, applicationCount - 1);
    }
    
    // Method to calculate the current cost based on the application count
    public int GetCurrentCost(int applicationCount) {
        // Calculate the cost
        float cost = baseCreditsCost * Mathf.Pow(costScalingFactor, applicationCount);

        // Ceil to the nearest multiple of 10
        int costCeiledToNearest10 = Mathf.CeilToInt(cost / 10f) * 10;

        return costCeiledToNearest10;
    }
}
