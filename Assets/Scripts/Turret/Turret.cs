using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class Turret : MonoBehaviour {
    [field: Header("Turret Stats SO")]
    [field: SerializeField]
    [field: Required]
    public TurretSO TurretSO { get; private set; }

    public Dictionary<BaseTurretUpgradeSO, int> ActiveUpgrades { get; private set; }
    public int CurrentTotalUpgrades { get; private set; }
    public int MaxActiveUpgrades { get; private set; } = 4;

    [field: Header("Turret Stats")]
    [ReadOnly, ShowInInspector] public float Damage { get; set; }
    [ReadOnly, ShowInInspector] public float RotationSpeed { get; set; }
    [ReadOnly, ShowInInspector] public float Range { get; set; }
    [ReadOnly, ShowInInspector] public int Cost { get; set; }
    [ReadOnly, ShowInInspector] public float FireRate { get; set; }
    [ReadOnly, ShowInInspector] public float ProjectileSpeed { get; set; }

    private void Awake() {
        ActiveUpgrades = new Dictionary<BaseTurretUpgradeSO, int>();
    }

    private void Start() {
        ApplyBaseStats();
    }

    private void ApplyBaseStats() {
        if (Damage == 0) Damage = TurretSO.baseDamage;
        if (Range == 0) Range = TurretSO.baseRange;
        if (Cost == 0) Cost = TurretSO.baseCost;
        if (FireRate == 0) FireRate = TurretSO.baseFireRate;
        if (RotationSpeed == 0) RotationSpeed = TurretSO.baseRotationSpeed;
        if (ProjectileSpeed == 0) ProjectileSpeed = TurretSO.baseProjectileSpeed;
    }
    
    public void Initialize(TurretSO turretSO) {
        this.TurretSO = turretSO;
        ApplyBaseStats();
    }

    public bool TryAddUpgrade(BaseTurretUpgradeSO upgrade) {
        // Calculate the current total upgrades
        CurrentTotalUpgrades = 0;
        foreach (var entry in ActiveUpgrades) {
            CurrentTotalUpgrades += entry.Value;
        }

        if (CurrentTotalUpgrades >= MaxActiveUpgrades) {
            Debug.LogWarning("Max total upgrades reached!");
            return false;
        }

        int applicationCount = ActiveUpgrades.ContainsKey(upgrade) ? ActiveUpgrades[upgrade] : 0;
        applicationCount++;

        // Apply the upgrade
        upgrade.ApplyUpgrade(this, applicationCount);
        ActiveUpgrades[upgrade] = applicationCount;

        // Increment the current total upgrades after successfully adding an upgrade
        CurrentTotalUpgrades++;

        Debug.Log($"Applied {upgrade.upgradeName} ({applicationCount} times). Active Upgrades: {CurrentTotalUpgrades}");
        return true;
    }

    public void RemoveUpgrade(BaseTurretUpgradeSO upgrade) {
        if (ActiveUpgrades.TryGetValue(upgrade, out int applicationCount) && applicationCount > 0) {
            upgrade.RevertUpgrade(this, applicationCount);
            applicationCount--;

            if (applicationCount > 0) {
                ActiveUpgrades[upgrade] = applicationCount;
            } else {
                ActiveUpgrades.Remove(upgrade);
            }

            // Decrement the current total upgrades after successfully removing an upgrade
            CurrentTotalUpgrades--;
        }
    }


    public void ClearUpgrades() {
        foreach (var upgrade in ActiveUpgrades) {
            upgrade.Key.RevertUpgrade(this, upgrade.Value);
        }
        ActiveUpgrades.Clear();
    }

    public override string ToString() {
        return TurretSO.turretName;
    }

    public TurretSO GetTurretSO() {
        return TurretSO;
    }

    public void DisableAllModules() {
        ToggleAllModules(false);
    }

    public void EnableAllModules() {
        ToggleAllModules(true);
    }

    private void ToggleAllModules(bool enable) {
        TryGetComponent(out TurretEnemyDetection enemyDetection);
        if (enemyDetection != null) enemyDetection.enabled = enable;

        TryGetComponent(out TurretTargetSelection targetingSelection);
        if (targetingSelection != null) targetingSelection.enabled = enable;

        TryGetComponent(out TurretFiring turretFiring);
        if (turretFiring != null) turretFiring.enabled = enable;

        TryGetComponent(out TurretAiming turretAiming);
        if (turretAiming != null) turretAiming.enabled = enable;
    }
}