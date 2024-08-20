using UnityEngine;

[CreateAssetMenu(fileName = "RangeUpgradeSO", menuName = "Turret Upgrades/New Range Upgrade")]
public class RangeUpgradeSO : BaseTurretUpgradeSO {
    public float rangeMultiplier;

    public override void ApplyUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(rangeMultiplier, applicationCount, diminishingFactor);
        Debug.Log($"RangeUpgradeSO ApplyUpgrade - baseMultiplier: {rangeMultiplier}, applicationCount: {applicationCount}, adjustedMultiplier: {adjustedMultiplier}");
        turret.Range *= adjustedMultiplier;
    }

    public override void RevertUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(rangeMultiplier, applicationCount, diminishingFactor);
        turret.Range /= adjustedMultiplier;
    }

    public override float GetBaseMultiplier() {
        return rangeMultiplier;
    }
}