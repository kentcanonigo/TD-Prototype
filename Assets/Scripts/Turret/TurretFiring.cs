using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFiring : MonoBehaviour, IFireable {
    [Header("Turret Physical Components")]
    [SerializeField] protected Transform firePoint; // The point from where the projectile is fired
    
    [field: Header("Projectile Info")]
    [SerializeField] private ProjectileSO projectileSO; // The projectile that will be fired

    public void Fire() {
        Debug.Log("Firing!");
    }
}
