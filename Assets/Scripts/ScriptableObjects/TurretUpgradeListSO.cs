using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretStats", menuName = "New Turret Upgrade List")]
public class TurretUpgradeListSO : ScriptableObject {
    public List<TurretUpgradeSO> turretUpgradeSOList; // The list of upgrades that can be applied to the turret
}
