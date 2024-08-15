using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHasHealth {
    [Header("Enemy")]
    [SerializeField] protected EnemySO enemySO; // Reference to the enemy stats ScriptableObject

    [SerializeField] protected int healthPoints { get; set; } // Health points of the enemy
    protected int totalHealthPoints; // Total HP of the enemy
    protected int armor; // Armor value that reduces incoming damage
    protected float speed; // Speed at which the enemy moves
    protected float sizeMultiplier; // Size multiplier of the enemy
    protected int damageToCore;

    public int HealthPoints => healthPoints; // Implementing the HealthPoints property
    public int Armor => armor; // Implementing the Armor property
    public bool IsDead => healthPoints <= 0; // Implementing IsDead property
    public event EnemyDeathHandler<Transform> OnEnemyDeath;

    protected List<Vector3> path; // List of positions for the enemy to follow
    protected int currentPathIndex; // Index of the current path position

    protected Wiggle enemyWiggle;

    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs {
        public float healthNormalized;
    }

    private void Awake() {
        enemyWiggle = gameObject.GetComponentInChildren<Wiggle>();
    }

    protected virtual void Start() {
        // Set stats from the ScriptableObject
        totalHealthPoints = enemySO.healthPoints;
        armor = enemySO.armor;
        speed = enemySO.speed;
        sizeMultiplier = enemySO.sizeMultiplier;
        damageToCore = enemySO.damageToCore;
        enemyWiggle.animationLoopTime = enemySO.wiggleSpeed;
        enemyWiggle.posRange = enemySO.wigglePosRange;
        enemyWiggle.rotRange = enemySO.wiggleRotRange;
        healthPoints = totalHealthPoints;

        // Set the size of the enemy GameObject
        transform.localScale *= sizeMultiplier;
    }

    public void InitializePath(List<Vector3> path) {
        this.path = path;
        currentPathIndex = 0; // Start at the first position
    }

    protected virtual void Update() {
        MoveAlongPath();
    }

    protected virtual void MoveAlongPath() {
        if (path == null || path.Count == 0) return;

        // Move toward the current path position
        Vector3 targetPosition = path[currentPathIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the enemy has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
            currentPathIndex++; // Move to the next position in the path
            if (currentPathIndex >= path.Count) {
                // Reached the end of the path (you can add logic for what happens next)
                //Debug.Log($"{gameObject.name} reached the spiral!");
                
                GameManager.Instance.DecreaseCoreHP(damageToCore);
                Kill(); // For example, you can call Die here
            }
        }
    }

    public virtual void TakeDamage(int damage) {
        // Calculate actual damage after considering armor
        int actualDamage = Mathf.Max(damage - armor, 0);
        healthPoints -= actualDamage;

        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
            healthNormalized = (float)healthPoints / totalHealthPoints
        });
        
        // Check if the enemy is dead
        if (IsDead) {
            Kill();
        }
    }

    public virtual void Kill() {
        // Handle enemy death (e.g., play death animation, destroy the object)
        //Debug.Log($"{gameObject.name} died!");
        OnEnemyDeath?.Invoke(transform, EventArgs.Empty);
        Destroy(gameObject); // Destroy the enemy GameObject
    }

    // Optional: Method to reset enemy stats
    public virtual void ResetEnemy() {
        // Reset stats from the ScriptableObject if needed
        totalHealthPoints = enemySO.healthPoints;
        healthPoints = totalHealthPoints;
        armor = enemySO.armor;
        speed = enemySO.speed;
        sizeMultiplier = enemySO.sizeMultiplier;
        transform.localScale = Vector3.one * sizeMultiplier; // Apply the size to the GameObject
        
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
            healthNormalized = (float)healthPoints / totalHealthPoints
        });
    }
}
