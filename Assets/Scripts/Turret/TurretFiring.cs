using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TurretFiring : MonoBehaviour, IFireable {
    [Required]
    [Header("Turret Physical Components")]
    [SerializeField] private Transform firePoint; // The point from where the projectile is fired
    
    [Required]
    [field: Header("Projectile Info")]
    [SerializeField] private ProjectileSO projectileSO; // The projectile that will be fired

    [field: SerializeField] private bool shootAlways;
    [SerializeField] private float angleThreshold = 20f;

    private Turret turret;
    private TurretAiming turretAiming;
    private TurretTargetSelection turretTargetSelection;
    private float fireCooldown;
    private bool isAutoAim;
    
    private void Start() {
        turret = GetComponent<Turret>();
        turretAiming = GetComponent<TurretAiming>();
        turretTargetSelection = GetComponent<TurretTargetSelection>();
        fireCooldown = turret.BaseFireRate;

        if (turretAiming) {
            isAutoAim = turretAiming.IsAutoAim;
        } else {
            isAutoAim = false;
        }
    }

    private void Update() {
        // Shoot if the rotation is sufficiently close to the target rotation
        fireCooldown -= Time.deltaTime;
        if (ShouldShoot()) {
            if (isAutoAim) {
                // Turret is autoaiming
                if (turretTargetSelection.SelectedTarget != null) {
                    // Turret has a target
                    Fire();
                }
            }
        }
    }

    private bool ShouldShoot() {
        if (shootAlways) {
            //Debug.Log("Shooting always is enabled.");
            return true;
        }

        if (turretTargetSelection.SelectedTarget != null) {
            bool closeToTarget = turretAiming.IsCloseToTarget(angleThreshold);
            //Debug.Log($"Is close to target: {closeToTarget}");
            return closeToTarget;
        }

        //Debug.Log("No target selected.");
        return false;
    }

    public void Fire() {
        if (fireCooldown <= 0f) {
            // Instantiate the projectile and shoot it towards the target
            if (firePoint != null && projectileSO.projectilePrefab != null) {
                GameObject projectileGO = Instantiate(projectileSO.projectilePrefab, firePoint.position, firePoint.rotation);
                Projectile projectile = projectileGO.GetComponent<Projectile>();
                if (projectile != null) {
                    projectile.SetBaseData(turret.BaseDamage, turret.BaseProjectileSpeed);
                } else {
                    Debug.LogError("Projectile prefab does not have a Projectile component.");
                }
            } else {
                Debug.LogError("FirePoint or ProjectilePrefab is not set.");
            }

            fireCooldown = 1f / turret.BaseFireRate; // Reset the cooldown timer based on the rateOfFire
        }
    }
}
