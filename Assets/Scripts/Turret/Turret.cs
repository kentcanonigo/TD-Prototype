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
    public int MaxActiveUpgrades { get; private set; } = 4;

    public TurretSO TurretSO {
        get => turretSO;
        set => turretSO = value; // Allow setting in the class
    }

    [field: Header("Turret Stats")]
    [ReadOnly, ShowInInspector] public float Damage { get; set; }
    [ReadOnly, ShowInInspector] public float RotationSpeed { get; set; }
    [ReadOnly, ShowInInspector] public float Range { get; set; }
    [ReadOnly, ShowInInspector] public int Cost { get; set; }
    [ReadOnly, ShowInInspector] public float FireRate { get; set; }
    [ReadOnly, ShowInInspector] public float ProjectileSpeed { get; set; }

    private void Awake() {
        activeUpgrades = new List<BaseTurretUpgradeSO>();
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
        this.turretSO = turretSO;
        ApplyBaseStats();
    }
    
    public bool TryAddUpgrade(BaseTurretUpgradeSO upgrade) {
        if (activeUpgrades.Count >= MaxActiveUpgrades) {
            Debug.LogWarning("Max upgrades reached!");
            return false;
        }
        upgrade.ApplyUpgrade(this);
        activeUpgrades.Add(upgrade);
        Debug.Log(activeUpgrades.Count);
        return true;
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