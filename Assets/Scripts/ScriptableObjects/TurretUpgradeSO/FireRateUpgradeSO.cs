using UnityEngine;

[CreateAssetMenu(fileName = "FireRateUpgradeSO", menuName = "Turret Upgrades/New Fire Rate Upgrade")]
public class FireRateUpgradeSO : BaseTurretUpgradeSO {
    public float fireRateMultiplier;

    public override void ApplyUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(fireRateMultiplier, applicationCount, diminishingFactor);
        Debug.Log($"RangeUpgradeSO ApplyUpgrade - baseMultiplier: {fireRateMultiplier}, applicationCount: {applicationCount}, adjustedMultiplier: {adjustedMultiplier}");
        turret.FireRate *= adjustedMultiplier;
    }

    public override void RevertUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(fireRateMultiplier, applicationCount, diminishingFactor);
        turret.FireRate /= adjustedMultiplier;
    }

    public override float GetBaseMultiplier() {
        return fireRateMultiplier;
    }
}
