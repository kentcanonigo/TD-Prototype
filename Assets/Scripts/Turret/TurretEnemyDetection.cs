using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Turret))]
public class TurretEnemyDetection : MonoBehaviour {
    [field: Header("Enemy Detection List")]
    public List<Transform> EnemiesInRange { get; private set; }

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
        EnemiesInRange = new List<Transform>();
        overlapResults = new List<Collider2D>();
        contactFilter = new ContactFilter2D {
            useTriggers = true,
            layerMask = LayerMask.GetMask("Enemies"),
            useLayerMask = true
        };
    }

    private void Start() {
        CircleCollider2D rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = turret.BaseRange;
    }
    
    private float checkInterval = 0.5f; // seconds
    private float nextCheckTime = 0f;

    private void Update() {
        if (Time.time >= nextCheckTime) {
            CheckEnemiesInRange();
            nextCheckTime = Time.time + checkInterval; // Set next check time
        }
    }
    
    // Needed for overlap circle
    
    private List<Collider2D> overlapResults;
    private ContactFilter2D contactFilter;
    
    private void CheckEnemiesInRange() {
        // Clear the current list of enemies
        EnemiesInRange.Clear();

        // Use OverlapCircle with the contact filter
        int colliderCount = Physics2D.OverlapCircle(transform.position, turret.BaseRange, contactFilter, overlapResults);

        for (int i = 0; i < colliderCount; i++) {
            // Check if the collider has the IEnemy component
            if (overlapResults[i].TryGetComponent(out IHasHealth enemy)) {
                EnemiesInRange.Add(overlapResults[i].transform);
                enemy.OnEnemyDeath += HandleEnemyDeath; // Subscribe to the death event
                //Debug.Log($"Enemy in range: {overlapResults[i].name}");
            }
        }
    }

    private void HandleEnemyDeath(Transform enemyTransform, EventArgs args) {
        EnemiesInRange.Remove(enemyTransform);
        
        // Ensure we unsubscribe from the event to avoid memory leaks
        if (enemyTransform.TryGetComponent(out IHasHealth enemy)) {
            enemy.OnEnemyDeath -= HandleEnemyDeath; // Unsubscribe from the event
        }
    }
}
