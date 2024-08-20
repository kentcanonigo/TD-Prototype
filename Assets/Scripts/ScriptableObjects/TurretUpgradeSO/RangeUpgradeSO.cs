using UnityEngine;

[CreateAssetMenu(fileName = "RangeUpgradeSO", menuName = "Turret Upgrades/New Range Upgrade")]
public class RangeUpgradeSO : BaseTurretUpgradeSO {
    public float rangeMultiplier;
    public float diminishingFactor = 0.9f; // Adjust this factor as needed

    public override void ApplyUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(rangeMultiplier, applicationCount, diminishingFactor);
        turret.Range *= adjustedMultiplier;
    }

    public override void RevertUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(rangeMultiplier, applicationCount, diminishingFactor);
        turret.Range /= adjustedMultiplier;
    }
}