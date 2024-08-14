using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveManager : MonoBehaviour {
    [Header("Wave Settings")]
    [SerializeField] private EnemySO enemySO;      // Reference to the test enemy prefab
    
    private void SpawnEnemyAtVortex(Vector2Int vortexPosition) {
        // Instantiate the enemy prefab at the vortex position
        GameObject enemy = Instantiate(enemySO.enemyPrefab, new Vector3(vortexPosition.x, vortexPosition.y, 0), Quaternion.identity);
        // You can set the path for the enemy to follow to the core here if you have a pathfinding system
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        // Set any necessary enemy parameters here, if needed
    }
}