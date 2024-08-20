using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgradeSO", menuName = "Turret Upgrades/New Damage Upgrade")]
public class DamageUpgradeSO : BaseTurretUpgradeSO {
    public float damageMultiplier;

    public override void ApplyUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(damageMultiplier, applicationCount, diminishingFactor);
        Debug.Log($"RangeUpgradeSO ApplyUpgrade - baseMultiplier: {damageMultiplier}, applicationCount: {applicationCount}, adjustedMultiplier: {adjustedMultiplier}");
        turret.Damage *= adjustedMultiplier;
    }

    public override void RevertUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(damageMultiplier, applicationCount, diminishingFactor);
        turret.Damage /= adjustedMultiplier;
    }

    public override float GetBaseMultiplier() {
        return damageMultiplier;
    }
}