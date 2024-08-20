using UnityEngine;

[CreateAssetMenu(fileName = "FireRateUpgradeSO", menuName = "Turret Upgrades/New Fire Rate Upgrade")]
public class FireRateUpgradeSO : BaseTurretUpgradeSO {
    public float fireRateMultiplier;
    public float diminishingFactor = 0.9f; // Adjust this factor as needed

    public override void ApplyUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(fireRateMultiplier, applicationCount, diminishingFactor);
        turret.FireRate *= adjustedMultiplier;
    }

    public override void RevertUpgrade(Turret turret, int applicationCount) {
        float adjustedMultiplier = CalculateDiminishingReturn(fireRateMultiplier, applicationCount, diminishingFactor);
        turret.FireRate /= adjustedMultiplier;
    }
}
