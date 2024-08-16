using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[SelectionBase]
public class UpgradeableTurret : Turret, IUpgradable {
    [SerializeField] private TurretUpgradeListSO turretUpgradeListSO; // The list of available upgrades
    [field: Header("Applied Turret Upgrades")]
    [SerializeField] public List<TurretUpgradeSO> appliedTurretUpgradeSOList = new List<TurretUpgradeSO>(); // The list of applied upgrades

    private int currentUpgradeIndex = 0; // The index of the current upgrade

    private void OnValidate() {
        // This is called whenever a value is changed in the inspector
        ApplyAllUpgrades();
    }

    private void OnEnable() {
        // This is called when the script is enabled, including after exiting play mode
        ApplyAllUpgrades();
    }

    private void ApplyAllUpgrades() {
        // Reset to base stats
        BaseDamage = TurretSO.baseDamage;
        BaseRange = TurretSO.baseRange;
        BaseFireRate = TurretSO.baseFireRate;
        BaseRotationSpeed = TurretSO.baseRotationSpeed;
        BaseProjectileSpeed = TurretSO.baseProjectileSpeed;

        // Reapply each upgrade
        foreach (TurretUpgradeSO upgrade in appliedTurretUpgradeSOList) {
            BaseDamage += upgrade.bonusDamage;
            BaseRange += upgrade.bonusRange;
            BaseFireRate += upgrade.bonusFireRate;
            BaseRotationSpeed += upgrade.bonusRotationSpeed;
            BaseProjectileSpeed += upgrade.bonusProjectileSpeed;
        }

        // Update the currentUpgradeIndex to reflect the actual number of upgrades
        currentUpgradeIndex = appliedTurretUpgradeSOList.Count;
    }

    public bool Upgrade() {
        if (currentUpgradeIndex >= turretUpgradeListSO.turretUpgradeSOList.Count) return false;

        // Register Undo before making changes
        Undo.RecordObject(this, "Upgrade Turret");

        TurretUpgradeSO upgrade = turretUpgradeListSO.turretUpgradeSOList[currentUpgradeIndex];
        appliedTurretUpgradeSOList.Add(upgrade);
        currentUpgradeIndex++;

        // Apply the upgrade
        ApplyAllUpgrades();

        // Mark the object as dirty to ensure the changes are saved
        EditorUtility.SetDirty(this);

        return true;
    }

    public bool Downgrade() {
        if (currentUpgradeIndex <= 0) return false;

        // Register Undo before making changes
        Undo.RecordObject(this, "Downgrade Turret");

        TurretUpgradeSO upgrade = appliedTurretUpgradeSOList[currentUpgradeIndex - 1];
        appliedTurretUpgradeSOList.RemoveAt(currentUpgradeIndex - 1);
        currentUpgradeIndex--;

        // Apply the downgrade
        ApplyAllUpgrades();

        // Mark the object as dirty to ensure the changes are saved
        EditorUtility.SetDirty(this);

        return true;
    }
}
