using UnityEngine;

public abstract class BaseTurretUpgradeSO : ScriptableObject {
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeSprite;
    public Color upgradeColor;
    public int creditsCost;
    public float diminishingFactor;

    public abstract void ApplyUpgrade(Turret turret, int applicationCount);
    public abstract void RevertUpgrade(Turret turret, int applicationCount);

    public abstract float GetBaseMultiplier();

    public float CalculateDiminishingReturn(float baseMultiplier, int applicationCount, float diminishingFactor) {
        return 1.0f + (baseMultiplier - 1.0f) * Mathf.Pow(diminishingFactor, applicationCount - 1);
    }
}
