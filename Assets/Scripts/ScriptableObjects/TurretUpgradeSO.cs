using UnityEngine;

[CreateAssetMenu(fileName = "TurretUpgrade", menuName = "New Turret Upgrade")]
public class TurretUpgradeSO : ScriptableObject {
    public int bonusDamage;
    public float bonusRange;
    public float bonusRateOfFire;
    public int upgradeCost;
}
