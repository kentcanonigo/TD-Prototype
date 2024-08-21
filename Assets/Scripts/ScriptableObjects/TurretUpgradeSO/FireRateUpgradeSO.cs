using UnityEngine;

[CreateAssetMenu(fileName = "FireRateUpgradeSO", menuName = "Turret Upgrades/New Fire Rate Upgrade")]
public class FireRateUpgradeSO : BaseTurretUpgradeSO {
    public float fireRateValue; // Renamed to a more general term

    public override void ApplyUpgrade(Turret turret, int applicationCount) {
        float adjustedValue = CalculateUpgradeValue(applicationCount);
        if (isMultiplier) {
            Debug.Log($"FireRateUpgradeSO ApplyUpgrade - baseMultiplier: {fireRateValue}, applicationCount: {applicationCount}, adjustedMultiplier: {adjustedValue}");
            turret.FireRate *= adjustedValue; // Apply as a multiplier
        } else {
            Debug.Log($"FireRateUpgradeSO ApplyUpgrade - baseBonus: {fireRateValue}, applicationCount: {applicationCount}, adjustedBonus: {adjustedValue}");
            turret.FireRate += adjustedValue; // Apply as a flat bonus
        }
    }

    public override void RevertUpgrade(Turret turret, int applicationCount) {
        float adjustedValue = CalculateUpgradeValue(applicationCount);
        if (isMultiplier) {
            turret.FireRate /= adjustedValue; // Revert multiplier
        } else {
            turret.FireRate -= adjustedValue; // Revert flat bonus
        }
    }

    public override float GetBaseValue() {
        return fireRateValue;
    }
}