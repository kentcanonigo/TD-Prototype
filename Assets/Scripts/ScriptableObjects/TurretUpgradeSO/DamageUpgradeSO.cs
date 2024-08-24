using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgradeSO", menuName = "Turret Upgrades/New Damage Upgrade")]
public class DamageUpgradeSO : BaseTurretUpgradeSO {
    public float damageValue; // General term for either multiplier or bonus

    public override string UpgradeType => UpgradeTypes.DAMAGE;

    public override void ApplyUpgrade(Turret turret, int applicationCount) {
        float adjustedValue = CalculateUpgradeValue(applicationCount);
        if (isMultiplier) {
            //Debug.Log($"DamageUpgradeSO ApplyUpgrade - baseMultiplier: {damageValue}, applicationCount: {applicationCount}, adjustedMultiplier: {adjustedValue}");
            turret.Damage *= adjustedValue; // Apply as a multiplier
        } else {
            //Debug.Log($"DamageUpgradeSO ApplyUpgrade - baseBonus: {damageValue}, applicationCount: {applicationCount}, adjustedBonus: {adjustedValue}");
            turret.Damage += adjustedValue; // Apply as a flat bonus
        }
    }

    public override void RevertUpgrade(Turret turret, int applicationCount) {
        float adjustedValue = CalculateUpgradeValue(applicationCount);
        if (isMultiplier) {
            turret.Damage /= adjustedValue; // Revert multiplier
        } else {
            turret.Damage -= adjustedValue; // Revert flat bonus
        }
    }

    public override float GetBaseValue() {
        return damageValue;
    }
}