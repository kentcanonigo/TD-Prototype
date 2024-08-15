using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurretEnemyDetection))]
public class TurretTargetSelection : MonoBehaviour {

    private TurretEnemyDetection turretEnemyDetection;
    public Transform SelectedTarget { get; private set; }

    // Enum for targeting preferences
    public enum TargetingPreference { Closest, Furthest, LowestHealth, HighestHealth }
    public TargetingPreference targetingPreference;
    
    private void Awake() {
        turretEnemyDetection = GetComponent<TurretEnemyDetection>();
    }

    private void Update() {
        SelectTarget();
    }

    public void SelectTarget() {
        if (turretEnemyDetection.EnemiesInRange.Count <= 0) {
            SelectedTarget = null;
            //Debug.Log("No enemies in range to select.");
            return;
        }

        switch (targetingPreference) {
            case TargetingPreference.Closest:
                SelectedTarget = GetClosestTarget();
                break;
            case TargetingPreference.Furthest:
                SelectedTarget = GetFurthestTarget();
                break;
            case TargetingPreference.LowestHealth:
                SelectedTarget = GetLowestHealthTarget();
                break;
            case TargetingPreference.HighestHealth:
                SelectedTarget = GetHighestHealthTarget();
                break;
        }
    }

    private Transform GetClosestTarget() {
        Transform closestTarget = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform enemy in turretEnemyDetection.EnemiesInRange) {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestTarget = enemy;
            }
        }
        
        return closestTarget;
    }

    private Transform GetFurthestTarget() {
        Transform furthestTarget = null;
        float maxDistance = 0f;

        foreach (Transform enemy in turretEnemyDetection.EnemiesInRange) {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance > maxDistance) {
                maxDistance = distance;
                furthestTarget = enemy;
            }
        }
        
        return furthestTarget;
    }

    private Transform GetLowestHealthTarget() {
        Transform lowestHealthTarget = null;
        float minHealth = Mathf.Infinity;

        foreach (Transform enemy in turretEnemyDetection.EnemiesInRange) {
            IHasHealth healthComponent = enemy.GetComponent<IHasHealth>();
            if (healthComponent != null && healthComponent.HealthPoints < minHealth) {
                minHealth = healthComponent.HealthPoints;
                lowestHealthTarget = enemy;
            }
        }
        return lowestHealthTarget;
    }

    private Transform GetHighestHealthTarget() {
        Transform highestHealthTarget = SelectedTarget; // Start with the current target
        int maxHealth = highestHealthTarget ? highestHealthTarget.GetComponent<IHasHealth>().HealthPoints : 0;
        int priorityThreshold = 10; // Larger margin to switch targets

        foreach (Transform enemy in turretEnemyDetection.EnemiesInRange) {
            IHasHealth healthComponent = enemy.GetComponent<IHasHealth>();
            if (healthComponent != null) {
                int enemyHealth = healthComponent.HealthPoints;

                // If current target is valid and within tolerance, keep it
                if (highestHealthTarget != null && enemy == highestHealthTarget) {
                    continue; // Skip checking the current target, it's already selected
                }

                // If this enemy's health is higher than the current maxHealth + priorityThreshold, select it
                if (enemyHealth > maxHealth + priorityThreshold) {
                    maxHealth = enemyHealth;
                    highestHealthTarget = enemy;
                }
                // If this enemy's health is within the priority threshold and there's no other target with significantly higher health, prefer the current target
                else if (enemyHealth > maxHealth && enemyHealth <= maxHealth + priorityThreshold && highestHealthTarget == null) {
                    highestHealthTarget = enemy;
                }
            }
        }

        return highestHealthTarget;
    }


}
