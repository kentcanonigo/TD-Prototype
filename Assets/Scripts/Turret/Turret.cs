using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Turret : MonoBehaviour, IUpgradable {
    [field: Header("Turret Stats SO")] [SerializeField]
    private TurretSO turretSO; // Backing field for the property

    [SerializeField] private List<TurretUpgradeSO> appliedUpgrades;

    public TurretSO TurretSO {
        get => turretSO;
        set => turretSO = value; // Allow setting in the class
    }

    [field: Header("Turret Stats")] public string TurretName { get; set; }
    public string TurretDescription { get; set; }
    public int BaseDamage { get; set; }
    public float BaseRotationSpeed { get; set; }
    public float BaseRange { get; set; }
    public int BaseCost { get; set; }
    public float BaseFireRate { get; set; }
    public float BaseProjectileSpeed { get; set; }

    private void Awake() {
        appliedUpgrades = new List<TurretUpgradeSO>();
    }
    
    void Start() {
        ApplyBaseStats();
        ApplyUpgrades();
    }

    void ApplyBaseStats() {
        if (string.IsNullOrEmpty(TurretName)) TurretName = TurretSO.turretName;
        if (string.IsNullOrEmpty(TurretDescription)) TurretDescription = TurretSO.turretDescription;
        if (BaseDamage == 0) BaseDamage = TurretSO.baseDamage;
        if (BaseRange == 0) BaseRange = TurretSO.baseRange;
        if (BaseCost == 0) BaseCost = TurretSO.baseCost;
        if (BaseFireRate == 0) BaseFireRate = TurretSO.baseFireRate;
        if (BaseRotationSpeed == 0) BaseRotationSpeed = TurretSO.baseRotationSpeed;
        if (BaseProjectileSpeed == 0) BaseProjectileSpeed = TurretSO.baseProjectileSpeed;
    }

    void ApplyUpgrades() {
        foreach (TurretUpgradeSO upgrade in appliedUpgrades) {
            BaseDamage += upgrade.bonusDamage;
            BaseRange += upgrade.bonusRange;
            BaseFireRate += upgrade.bonusFireRate;
            BaseRotationSpeed += upgrade.bonusRotationSpeed;
            BaseProjectileSpeed += upgrade.bonusProjectileSpeed;
            // Apply other upgrade effects
        }
    }

    public void AddUpgrade(TurretUpgradeSO upgrade) {
        appliedUpgrades.Add(upgrade);
        ApplyUpgrades();
    }

    public void RemoveUpgrade(TurretUpgradeSO upgrade) {
        appliedUpgrades.Remove(upgrade);
        ApplyBaseStats();
        ApplyUpgrades();
    }
}