using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretStats", menuName = "New Turret")]
[InlineEditor]
public class TurretSO : ScriptableObject {
    [BoxGroup("Basic Info")]
    public string turretName;
    [BoxGroup("Basic Info")] [TextArea]
    public string turretDescription;
    [Required] [AssetsOnly]
    public GameObject turretPrefab;
    [Range(0, 1000)]
    public int baseDamage = 10;
    [Range(0, 10)]
    public float baseRange = 4f;
    [Range(0, 500)]
    public int baseCost = 10;
    [Range(0.1f, 50f)]
    public float baseFireRate = 1f;
    [Range(0.1f, 50f)]
    public float baseRotationSpeed = 1f;
    [Range(0.1f, 50f)]
    public float baseProjectileSpeed = 10f;
}
