using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "New Enemy Stats")]
[InlineEditor]
public class EnemySO : ScriptableObject {
    [BoxGroup("Basic Info")]
    public string enemyName; // Name of the enemy
    [BoxGroup("Basic Info")] [TextArea]
    public string enemyDescription; // Description of the enemy
    [Required] [AssetsOnly]
    public GameObject enemyPrefab;
    [Range(1, 100000)]
    public int healthPoints = 100; // Health points of the enemy
    [Range(0, 100)]
    public int armor;          // Armor value that reduces incoming damage
    [Range(0.1f, 5)]
    public float speed = 2f;       // Speed at which the enemy moves
    [Range(1, 10)]
    public int damageToCore = 1; // The damage of this enemy to the core if it were to reach it
    [Range(1, 2)]
    public float sizeMultiplier = 1f; // Size multiplier of the enemy
    public int creditValue = 1; // Credits dropped when the enemy dies
}