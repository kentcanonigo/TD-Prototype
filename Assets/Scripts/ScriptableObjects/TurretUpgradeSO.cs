using UnityEngine;

[CreateAssetMenu(fileName = "TurretUpgrade", menuName = "New Turret Upgrade")]
public class TurretUpgradeSO : ScriptableObject {
    [Header("Cost of the upgrade")]
    public int upgradeCost;
    [Header("Bonus stats of the upgrade")]
    public int bonusDamage;
    public float bonusRange;
    public float bonusFireRate;
    public float bonusRotationSpeed;
    public float bonusProjectileSpeed;
}
