using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretStats", menuName = "New Turret")]
public class TurretSO : ScriptableObject {
    [Header("Turret Stats")] public string turretName;
    [TextArea(10, 5)] public string turretDescription;
    [Header("IMPORTANT! The Prefab for the turret (used for instantiation)")]
    public GameObject turretPrefab;
    public int damage = 10;
    public float range = 4f;
    public int cost = 10;
    public float fireRate = 1f;
    public float rotationSpeed = 1f;
}
