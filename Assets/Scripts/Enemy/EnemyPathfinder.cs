using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyPathfinder : MonoBehaviour {
    
    private Enemy enemy;
    private List<Vector3> path; // List of positions for the enemy to follow
    private int currentPathIndex; // Index of the current path position

    private void Awake() {
        enemy = GetComponent<Enemy>();
    }

    private void Update() {
        MoveAlongPath();
    }

    public void InitializePath(List<Vector3> path) {
        this.path = path;
        currentPathIndex = 0; // Start at the first position
    }
    
    private void MoveAlongPath() {
        if (path == null || path.Count == 0) return;

        // Move toward the current path position
        Vector3 targetPosition = path[currentPathIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemy.Speed * Time.deltaTime);

        // Check if the enemy has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
            currentPathIndex++; // Move to the next position in the path
            if (currentPathIndex >= path.Count) {
                // Reached the end of the path (you can add logic for what happens next)

                OnReachedSpiral();
            }
        }
    }

    private void OnReachedSpiral() {
        
        GameManager.Instance.DecreaseCoreHP(enemy.DamageToCore);
        enemy.Kill();
    }
}