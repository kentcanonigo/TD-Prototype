using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[SelectionBase]
public class Turret : MonoBehaviour {
    [Header("Turret Stats SO")]
    [SerializeField] protected TurretSO turretSO;
    
    [Header("Turret Physical Components")]
    [SerializeField] protected Transform firePoint; // The point from where the projectile is fired
    [SerializeField] protected Transform pivotPoint; // Where to pivot the gun

    [field: Header("Turret Stats")] public string TurretName { get; private set; }
    public string TurretDescription { get; private set; }
    public int BaseDamage { get; private set; }
    public float BaseRotationSpeed { get; private set; }
    public float BaseRange { get; private set; }
    public int BaseCost { get; private set; }
    public float BaseFireRate { get; private set; }
    
    [field: Header("Projectile Info")]
    [SerializeField] private ProjectileSO projectileSO; // The projectile that will be fired

    private void Awake() {
        // Assign turret stats from the ScriptableObject
        TurretName = turretSO.turretName;
        TurretDescription = turretSO.turretDescription;
        BaseDamage = turretSO.damage;
        BaseRange = turretSO.range;
        BaseCost = turretSO.cost;
        BaseFireRate = turretSO.fireRate;
        BaseRotationSpeed = turretSO.rotationSpeed;
    }
}
