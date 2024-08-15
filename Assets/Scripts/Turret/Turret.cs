using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[SelectionBase]
public class Turret : MonoBehaviour {
    [field: Header("Turret Stats SO")]
    [SerializeField] private TurretSO turretSO; // Backing field for the property

    public TurretSO TurretSO {
        get { return turretSO; }
        private set { turretSO = value; } // Optionally allow setting in the class if needed
    }

    [field: Header("Turret Stats")] public string TurretName { get; private set; }
    public string TurretDescription { get; private set; }
    public int BaseDamage { get; private set; }
    public float BaseRotationSpeed { get; private set; }
    public float BaseRange { get; private set; }
    public int BaseCost { get; private set; }
    public float BaseFireRate { get; private set; }
    public float BaseFireCooldown { get; private set; }
    public float BaseProjectileSpeed { get; private set; }
    
    private void Awake() {
        // Assign turret stats from the ScriptableObject
        TurretName = TurretSO.turretName;
        TurretDescription = TurretSO.turretDescription;
        BaseDamage = TurretSO.baseDamage;
        BaseRange = TurretSO.baseRange;
        BaseCost = TurretSO.baseCost;
        BaseFireRate = TurretSO.baseFireRate;
        BaseFireCooldown = TurretSO.baseFireCooldown;
        BaseRotationSpeed = TurretSO.baseRotationSpeed;
        BaseProjectileSpeed = TurretSO.baseProjectileSpeed;
    }
}
