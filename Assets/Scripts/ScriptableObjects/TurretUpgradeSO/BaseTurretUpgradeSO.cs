using UnityEngine;

public abstract class BaseTurretUpgradeSO : ScriptableObject {
    public Sprite upgradeSprite;
    public int creditsCost;
    public abstract void ApplyUpgrade(Turret turret);
    public abstract void RevertUpgrade(Turret turret);
}