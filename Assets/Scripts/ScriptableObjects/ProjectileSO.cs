using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "Projectiles/New Projectile")]
public class ProjectileSO : ScriptableObject {
    public GameObject projectilePrefab;
    public DamageTypeSO damageTypeSO;
}