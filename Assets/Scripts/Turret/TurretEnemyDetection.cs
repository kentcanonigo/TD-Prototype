using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Turret))] [RequireComponent(typeof(CircleCollider2D))]
public class TurretEnemyDetection : MonoBehaviour {
    [field: Header("Enemy Detection List")]
    public List<Transform> EnemiesInRange { get; private set; }

    private CircleCollider2D rangeCollider;

    private Turret turret;

    #region EDITOR

    private float detectionRangeGizmo; // The detection range of the turret;
    private Color rangeColor;
    
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
            detectionRangeGizmo = turret.TurretSO != null ? turret.TurretSO.baseRange : 0f; // Set detection range
            rangeColor = turret.TurretSO != null ? Color.green : Color.red;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = rangeColor;
        Gizmos.DrawWireSphere(transform.position, detectionRangeGizmo);
    }

    #endregion
    
    private void Awake() {
        turret = GetComponent<Turret>();
        rangeCollider = GetComponent<CircleCollider2D>();
        EnemiesInRange = new List<Transform>();
    }

    private void Start() {
        rangeCollider.isTrigger = true;
        rangeCollider.radius = turret.BaseRange;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Get the top-level parent object that has the IHasHealth component
        Transform parent = collision.transform.root;

        if (parent.TryGetComponent(out IHasHealth enemy)) {
            // Add the enemy to the list when it enters the trigger
            EnemiesInRange.Add(parent);
            // Optionally, subscribe to the enemy's death event here
            //enemy.OnEnemyDeath += HandleEnemyDeath;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // Get the top-level parent object that has the IHasHealth component
        Transform parent = collision.transform.root;

        if (parent.TryGetComponent(out IHasHealth enemy)) {
            // Remove the enemy from the list when it exits the trigger
            EnemiesInRange.Remove(parent);
            // Optionally, unsubscribe from the enemy's death event here
            //enemy.OnEnemyDeath -= HandleEnemyDeath;
        }
    }

}
