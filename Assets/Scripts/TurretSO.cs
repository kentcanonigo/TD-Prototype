using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretStats", menuName = "New Turret")]
public class TurretSO : ScriptableObject {
    [Header("Turret Stats")] public string turretName;
    [TextArea(10, 5)] public string turretDescription;
    public GameObject turretPrefab;
    public int damage = 10;
    public float range = 4f;
    public bool isSplashDamage = false;
    public float splashDamageRadius = 2f;
    public int cost = 10;
    public int[] upgradeCosts = {20,40,60,80};
}
