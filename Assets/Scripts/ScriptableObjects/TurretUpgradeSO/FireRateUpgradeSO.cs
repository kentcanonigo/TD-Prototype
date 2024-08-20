using UnityEngine;

[CreateAssetMenu(fileName = "FireRateUpgradeSO", menuName = "Turret Upgrades/New Fire Rate Upgrade")]
public class FireRateUpgradeSO : BaseTurretUpgradeSO {
    public float fireRateMultiplier;

    public override void ApplyUpgrade(Turret turret) {
        turret.BaseFireRate *= fireRateMultiplier;
    }

    public override void RevertUpgrade(Turret turret) {
        turret.BaseFireRate /= fireRateMultiplier;
    }
}