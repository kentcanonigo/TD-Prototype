using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[SelectionBase]
public class Turret : MonoBehaviour {
    [field: Header("Turret Stats SO")]
    [field: SerializeField]
    public TurretSO TurretSO { get; private set; }

    [field: Header("Turret Stats")] public string TurretName { get; private set; }
    public string TurretDescription { get; private set; }
    public int BaseDamage { get; private set; }
    public float BaseRotationSpeed { get; private set; }
    public float BaseRange { get; private set; }
    public int BaseCost { get; private set; }
    public float BaseFireRate { get; private set; }
    
    private void Awake() {
        // Assign turret stats from the ScriptableObject
        TurretName = TurretSO.turretName;
        TurretDescription = TurretSO.turretDescription;
        BaseDamage = TurretSO.baseDamage;
        BaseRange = TurretSO.baseRange;
        BaseCost = TurretSO.baseCost;
        BaseFireRate = TurretSO.baseFireRate;
        BaseRotationSpeed = TurretSO.baseRotationSpeed;
    }
}
