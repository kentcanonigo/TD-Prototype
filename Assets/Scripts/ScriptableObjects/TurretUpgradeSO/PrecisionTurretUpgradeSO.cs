using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PrecisionUpgradeSO", menuName = "Turret Upgrades/New Precision Upgrade")]
public class PrecisionTurretUpgradeSO : BaseTurretUpgradeSO {
    public float precisionValue;
    
    public override string UpgradeType => UpgradeTypes.PRECISION;
    
    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Multiplier Scaling")]
    public string ProjectileSpeedFor1stApplication => FormatUpgradeValue(CalculateUpgradeValue(1) / 2);

    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Multiplier Scaling")]
    public string ProjectileSpeedFor2ndApplication => FormatUpgradeValue(CalculateUpgradeValue(2) / 2);

    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Multiplier Scaling")]
    public string ProjectileSpeedFor3rdApplication => FormatUpgradeValue(CalculateUpgradeValue(3) / 2);

    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Multiplier Scaling")]
    public string ProjectileSpeedFor4thApplication => FormatUpgradeValue(CalculateUpgradeValue(4) / 2);
    
    // Add this property for the total multiplier value
    [ShowInInspector, ReadOnly, LabelWidth(250), BoxGroup("Multiplier Scaling")]
    public string TotalProjectileSpeedMultiplier => FormatUpgradeValue(CalculateTotalMultiplier());
    
    private float CalculateTotalMultiplier() {
        float total = 0f;
        total += CalculateUpgradeValue(1) / 2;
        total += CalculateUpgradeValue(2) / 2;
        total += CalculateUpgradeValue(3) / 2;
        total += CalculateUpgradeValue(4) / 2;
        return total;
    }

    public override void ApplyUpgrade(Turret turret, int applicationCount) {
        float adjustedValue = CalculateUpgradeValue(applicationCount);
        if (isMultiplier) {
            //Debug.Log($"RangeUpgradeSO ApplyUpgrade - baseMultiplier: {precisionValue}, applicationCount: {applicationCount}, adjustedMultiplier: {adjustedValue}");
            turret.RotationSpeed *= adjustedValue;
            turret.ProjectileSpeed *= adjustedValue / 2;
        } else {
            //Debug.Log($"RangeUpgradeSO ApplyUpgrade - baseBonus: {precisionValue}, applicationCount: {applicationCount}, adjustedBonus: {adjustedValue}");
            turret.RotationSpeed += adjustedValue;
            turret.ProjectileSpeed += adjustedValue / 2;
        }
    }

    public override void RevertUpgrade(Turret turret, int applicationCount) {
        float adjustedValue = CalculateUpgradeValue(applicationCount);
        if (isMultiplier) {
            turret.RotationSpeed /= adjustedValue;
            turret.ProjectileSpeed /= adjustedValue / 2;
        } else {
            turret.RotationSpeed -= adjustedValue;
            turret.ProjectileSpeed -= adjustedValue / 2;
        }
    }

    public override float GetBaseValue() {
        return precisionValue;
    }
}