using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretStats", menuName = "New Turret")]
[InlineEditor]
public class TurretSO : ScriptableObject {
    [Header("Turret Stats")] public string turretName;
    [TextArea(10, 5)] public string turretDescription;
    [Header("IMPORTANT! The Prefab for the turret (used for instantiation)")]
    public GameObject turretPrefab;
    public int baseDamage = 10;
    public float baseRange = 4f;
    public int baseCost = 10;
    public float baseFireRate = 1f;
    public float baseRotationSpeed = 1f;
    public float baseProjectileSpeed = 10f;
}
