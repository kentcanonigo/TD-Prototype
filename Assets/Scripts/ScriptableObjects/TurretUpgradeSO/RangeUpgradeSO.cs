using UnityEngine;

[CreateAssetMenu(fileName = "RangeUpgradeSO", menuName = "Turret Upgrades/New Range Upgrade")]
public class RangeUpgradeSO : BaseTurretUpgradeSO {
    public float rangeValue;

    public override void ApplyUpgrade(Turret turret, int applicationCount) {
        float adjustedValue = CalculateUpgradeValue(applicationCount);
        if (isMultiplier) {
            Debug.Log($"RangeUpgradeSO ApplyUpgrade - baseMultiplier: {rangeValue}, applicationCount: {applicationCount}, adjustedMultiplier: {adjustedValue}");
            turret.Range *= adjustedValue;
        } else {
            Debug.Log($"RangeUpgradeSO ApplyUpgrade - baseBonus: {rangeValue}, applicationCount: {applicationCount}, adjustedBonus: {adjustedValue}");
            turret.Range += adjustedValue;
        }
    }

    public override void RevertUpgrade(Turret turret, int applicationCount) {
        float adjustedValue = CalculateUpgradeValue(applicationCount);
        if (isMultiplier) {
            turret.Range /= adjustedValue;
        } else {
            turret.Range -= adjustedValue;
        }
    }

    public override float GetBaseValue() {
        return rangeValue;
    }
}