using UnityEngine;

public abstract class BaseTurretUpgradeSO : ScriptableObject {
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeSprite;
    public Color upgradeColor;
    public int creditsCost;

    public abstract void ApplyUpgrade(Turret turret, int applicationCount);
    public abstract void RevertUpgrade(Turret turret, int applicationCount);

    protected float CalculateDiminishingReturn(float baseValue, int applicationCount, float diminishingFactor) {
        return baseValue * Mathf.Pow(diminishingFactor, applicationCount - 1);
    }
}
