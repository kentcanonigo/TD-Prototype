using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgradeSO", menuName = "Turret Upgrades/New Damage Upgrade")]
public class DamageUpgradeSO : BaseTurretUpgradeSO {
    public float damageMultiplier;

    public override void ApplyUpgrade(Turret turret) {
        turret.Damage *= damageMultiplier;
    }

    public override void RevertUpgrade(Turret turret) {
        turret.Damage /= damageMultiplier;
    }
}