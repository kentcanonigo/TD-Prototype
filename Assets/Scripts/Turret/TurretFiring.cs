using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFiring : MonoBehaviour, IFireable {
    [Header("Turret Physical Components")]
    [SerializeField] private Transform firePoint; // The point from where the projectile is fired
    
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
        fireCooldown = turret.BaseFireCooldown;

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
            return true; // Always shoot if the boolean is true
        }
        // If not always shooting, check if the turret is close to the target
        return turretAiming.IsCloseToTarget(angleThreshold);
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
