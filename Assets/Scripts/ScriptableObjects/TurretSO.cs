using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretStats", menuName = "New Turret")]
[InlineEditor]
public class TurretSO : ScriptableObject {
    [BoxGroup("Basic Info")]
    public string turretName;
    [BoxGroup("Basic Info")] [TextArea]
    public string turretDescription;
    [Required] [AssetsOnly] [AssetSelector(Paths = "Assets/Prefabs/Turrets")]
    public GameObject turretPrefab;
    [InfoBox("The ScriptableObject that contains the Turret's possible upgrades")]
    [AssetSelector(Paths = "Assets/ScriptableObjects/Turrets/TurretUpgradeLists")]
    [field: SerializeField] public TurretUpgradeListSO turretUpgradeListSO;
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
