using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[SelectionBase]
public class UpgradableTurret : Turret, IUpgradable {
    [field: Header("Turret Upgrades")]
    [SerializeField] private TurretUpgradeSO[] turretUpgradeSOList; // The list of upgrades that can be applied to the turret

    public void Upgrade() {
        throw new NotImplementedException();
    }

    public void Downgrade() {
        throw new NotImplementedException();
    }
}