using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
public class Turret : MonoBehaviour {
    [field: Header("Turret Stats SO")] [SerializeField] [Required]
    private TurretSO turretSO; // Backing field for the property

    private List<BaseTurretUpgradeSO> activeUpgrades;

    public TurretSO TurretSO {
        get => turretSO;
        set => turretSO = value; // Allow setting in the class
    }

    [field: Header("Turret Stats")]
    public float BaseDamage { get; set; }
    public float BaseRotationSpeed { get; set; }
    public float BaseRange { get; set; }
    public int BaseCost { get; set; }
    public float BaseFireRate { get; set; }
    public float BaseProjectileSpeed { get; set; }

    private void Awake() {
        activeUpgrades = new List<BaseTurretUpgradeSO>();
    }

    private void Start() {
        ApplyBaseStats();
    }

    private void ApplyBaseStats() {
        if (BaseDamage == 0) BaseDamage = TurretSO.baseDamage;
        if (BaseRange == 0) BaseRange = TurretSO.baseRange;
        if (BaseCost == 0) BaseCost = TurretSO.baseCost;
        if (BaseFireRate == 0) BaseFireRate = TurretSO.baseFireRate;
        if (BaseRotationSpeed == 0) BaseRotationSpeed = TurretSO.baseRotationSpeed;
        if (BaseProjectileSpeed == 0) BaseProjectileSpeed = TurretSO.baseProjectileSpeed;
    }
    
    public void Initialize(TurretSO turretSO) {
        this.turretSO = turretSO;
        ApplyBaseStats();
    }
    
    public void AddUpgrade(BaseTurretUpgradeSO upgrade) {
        upgrade.ApplyUpgrade(this);
        activeUpgrades.Add(upgrade);
    }

    public void RemoveUpgrade(BaseTurretUpgradeSO upgrade) {
        upgrade.RevertUpgrade(this);
        activeUpgrades.Remove(upgrade);
    }

    public void ClearUpgrades() {
        foreach (var upgrade in activeUpgrades) {
            upgrade.RevertUpgrade(this);
        }
        activeUpgrades.Clear();
    }

    public override string ToString() {
        return turretSO.turretName;
    }

    public TurretSO GetTurretSO() {
        return turretSO;
    }
    
    public bool TryGetTargetingSelection(out TurretTargetSelection targetingSelection) {
        targetingSelection = GetComponent<TurretTargetSelection>();
        return targetingSelection;
    }
    
    public bool TryGetEnemyDetection(out TurretEnemyDetection enemyDetection) {
        enemyDetection = GetComponent<TurretEnemyDetection>();
        return enemyDetection;
    }
    
    public bool TryGetFiring(out TurretFiring turretFiring) {
        turretFiring = GetComponent<TurretFiring>();
        return turretFiring;
    }
    
    public bool TryGetAiming(out TurretAiming turretAiming) {
        turretAiming = GetComponent<TurretAiming>();
        return turretAiming;
    }
    
    public bool TryGetRangeVisual(out TurretRangeVisual rangeVisual) {
        rangeVisual = GetComponent<TurretRangeVisual>();
        return rangeVisual;
    }

    public void DisableAllModules() {
        if (TryGetEnemyDetection(out TurretEnemyDetection enemyDetection)) {
            enemyDetection.enabled = false;
        }
        if (TryGetTargetingSelection(out TurretTargetSelection targetingSelection)) {
            targetingSelection.enabled = false;
        }
        if (TryGetFiring(out TurretFiring turretFiring)) {
            turretFiring.enabled = false;
        }
        if (TryGetAiming(out TurretAiming turretAiming)) {
            turretAiming.enabled = false;
        }
    }

    public void EnableAllModules() {
        if (TryGetEnemyDetection(out TurretEnemyDetection enemyDetection)) {
            enemyDetection.enabled = true;
        }
        if (TryGetTargetingSelection(out TurretTargetSelection targetingSelection)) {
            targetingSelection.enabled = true;
        }
        if (TryGetFiring(out TurretFiring turretFiring)) {
            turretFiring.enabled = true;
        }
        if (TryGetAiming(out TurretAiming turretAiming)) {
            turretAiming.enabled = true;
        }
    }
}