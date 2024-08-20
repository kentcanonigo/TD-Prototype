using UnityEngine;

public abstract class BaseTurretUpgradeSO : ScriptableObject {
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeSprite;
    public int creditsCost;
    public abstract void ApplyUpgrade(Turret turret);
    public abstract void RevertUpgrade(Turret turret);
}