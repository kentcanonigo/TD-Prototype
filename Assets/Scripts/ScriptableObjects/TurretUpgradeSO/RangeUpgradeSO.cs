using UnityEngine;

[CreateAssetMenu(fileName = "RangeUpgradeSO", menuName = "Turret Upgrades/New Range Upgrade")]
public class RangeUpgradeSO : BaseTurretUpgradeSO {
    public float rangeMultiplier;

    public override void ApplyUpgrade(Turret turret) {
        turret.Range *= rangeMultiplier;
    }

    public override void RevertUpgrade(Turret turret) {
        turret.Range /= rangeMultiplier;
    }
}