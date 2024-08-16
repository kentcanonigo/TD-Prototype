using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Turret))]
[RequireComponent(typeof(CircleCollider2D))]
public class TurretEnemyDetection : MonoBehaviour {
    [field: Header("Enemy Detection List")]
    public List<Transform> EnemiesInRange { get; private set; }

    private CircleCollider2D rangeCollider;
    private float lastBaseRange;

    private Turret turret;

    #region EDITOR

    private float detectionRangeGizmo;
    private Color rangeColor;

    private void OnEnable() {
        UpdateGizmos();
    }

    private void OnValidate() {
        UpdateGizmos();
    }

    public void UpdateGizmos() {
        turret = GetComponent<Turret>();
        if (turret) {
            detectionRangeGizmo = turret.BaseRange; // Use the current range
            rangeColor = turret.TurretSO ? Color.green : Color.red;

            // Make sure to update the collider radius even in edit mode
            if (rangeCollider == null) {
                rangeCollider = GetComponent<CircleCollider2D>();
            }
            rangeCollider.radius = detectionRangeGizmo;
        }

        // Repaint the scene view to reflect the changes immediately
        if (!Application.isPlaying) {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
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

    private void Update() {
        if (Mathf.Abs(turret.BaseRange - lastBaseRange) > Mathf.Epsilon) {
            rangeCollider.radius = turret.BaseRange;
            lastBaseRange = turret.BaseRange;
            detectionRangeGizmo = lastBaseRange;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Transform parent = collision.transform.root;

        if (parent.TryGetComponent(out IHasHealth enemy)) {
            EnemiesInRange.Add(parent);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Transform parent = collision.transform.root;

        if (parent.TryGetComponent(out IHasHealth enemy)) {
            EnemiesInRange.Remove(parent);
        }
    }
}
