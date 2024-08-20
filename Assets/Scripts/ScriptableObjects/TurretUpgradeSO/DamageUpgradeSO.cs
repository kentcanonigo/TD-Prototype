using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgradeSO", menuName = "Turret Upgrades/New Damage Upgrade")]
public class DamageUpgradeSO : BaseTurretUpgradeSO {
    public float damageMultiplier;

    public override void ApplyUpgrade(Turret turret) {
        turret.BaseDamage *= damageMultiplier;
    }

    public override void RevertUpgrade(Turret turret) {
        turret.BaseDamage /= damageMultiplier;
    }
}