using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor]
[CreateAssetMenu(fileName = "TurretUpgrade", menuName = "New Turret Upgrade")]
public class TurretUpgradeSO : ScriptableObject {
    public int upgradeCost;
    [Header("Bonus stats of the upgrade")]
    public int bonusDamage;
    public float bonusRange;
    public float bonusFireRate;
    public float bonusRotationSpeed;
    public float bonusProjectileSpeed;
}
