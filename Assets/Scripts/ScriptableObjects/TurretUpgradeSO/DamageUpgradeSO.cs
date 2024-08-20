using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgradeSO", menuName = "Turret Upgrades/New Damage Upgrade")]
public class DamageUpgradeSO : BaseTurretUpgradeSO {
    public float damageMultiplier;
    public float diminishingFactor = 0.9f; // Adjust this factor as needed

    public override void ApplyUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(damageMultiplier, applicationCount, diminishingFactor);
        turret.Damage *= adjustedMultiplier;
    }

    public override void RevertUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(damageMultiplier, applicationCount, diminishingFactor);
        turret.Damage /= adjustedMultiplier;
    }
}