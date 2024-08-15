using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "New Enemy Stats")]
public class EnemySO : ScriptableObject {
    [Header("Enemy Stats")]
    public string enemyName; // Name of the enemy
    [TextArea(10, 5)] public string enemyDescription; // Description of the enemy
    public GameObject enemyPrefab;
    public int healthPoints = 100; // Health points of the enemy
    public int armor;          // Armor value that reduces incoming damage
    public float speed = 2f;       // Speed at which the enemy moves
    public int damageToCore = 1; // The damage of this enemy to the core if it were to reach it
    public float sizeMultiplier = 1f; // Size multiplier of the enemy
    [Header("Wiggle for Enemy Movement")]
    public float wiggleSpeed = 1f;
    public float wigglePosRange = 0.1f;
    public float wiggleRotRange = 0.2f;
}