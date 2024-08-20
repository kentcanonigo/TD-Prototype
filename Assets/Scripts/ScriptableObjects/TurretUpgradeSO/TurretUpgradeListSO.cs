using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretUpgradeListSO", menuName = "Turret Upgrades/New Turret Upgrade List")]
public class TurretUpgradeListSO : ScriptableObject {
    [AssetSelector(Paths = "Assets/ScriptableObjects/Turrets/TurretUpgradeLists")]
    [SerializeField] public List<BaseTurretUpgradeSO> possibleUpgradesListSO;
}