using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Turret))]
public class TurretEnemyDetection : MonoBehaviour {

    private float detectionRange; // The detection range of the turret;
    private TurretSO turretSO;
    private Turret turret;

    #region EDITOR
    
    private void OnEnable() {
        UpdateTurretSO(); // Initial update
    }

    public void OnValidate() {
        UpdateTurretSO(); // Call to update the turretSO when OnValidate is triggered
    }
    
    private void UpdateTurretSO() {
        // Find the Turret component in the parent or sibling objects
        turret = GetComponent<Turret>();
        if (turret != null) {
            turretSO = turret.TurretSO; // Get the turret scriptable object
            detectionRange = turretSO != null ? turretSO.baseRange : 0f; // Set detection range
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = turretSO != null ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    #endregion
    
    private void Awake() {
        turret = GetComponent<Turret>();
        if (turret != null) {
            turretSO = turret.TurretSO; // Get the TurretSO from the Turret component
        }
    }

    private void Start() {
        CircleCollider2D rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = turret.BaseRange;
    }
}
