using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IHasHealth {
    [Header("Enemy")]
    [SerializeField] private EnemySO enemySO; // Reference to the enemy stats ScriptableObject

    public int HealthPoints { get; private set; } // Health points of the enemy
    public int TotalHealthPoints { get; private set; } // Total HP of the enemy
    public int Armor { get; private set; } // Armor value that reduces incoming damage
    public float Speed { get; private set; } // Speed at which the enemy moves
    public float SizeMultiplier { get; private set; } // SizeMultiplier; // Size multiplier of the enemy
    public int DamageToCore { get; private set; } // Damage that the enemy deals to the core
    public bool IsDead => HealthPoints <= 0; // Implementing IsDead property
    public event DeathHandler<Transform> OnDeath;
    

    private Wiggle enemyWiggle;

    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs {
        public float healthNormalized;
    }

    private void Awake() {
        TotalHealthPoints = enemySO.healthPoints; // Set stats from the ScriptableObject
        Armor = enemySO.armor;
        Speed = enemySO.speed;
        SizeMultiplier = enemySO.sizeMultiplier;
        DamageToCore = enemySO.damageToCore;
        HealthPoints = TotalHealthPoints;
        enemyWiggle = gameObject.GetComponentInChildren<Wiggle>();
    }

    private void Start() {
        enemyWiggle.animationLoopTime = enemySO.wiggleSpeed;
        enemyWiggle.posRange = enemySO.wigglePosRange;
        enemyWiggle.rotRange = enemySO.wiggleRotRange;
        transform.localScale *= SizeMultiplier;
    }

    public virtual void TakeDamage(int damage) {
        // Calculate actual damage after considering armor
        int actualDamage = Mathf.Max(damage - Armor, 0);
        HealthPoints -= actualDamage;

        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
            healthNormalized = (float)HealthPoints / TotalHealthPoints
        });
        
        // Check if the enemy is dead
        if (IsDead) {
            Kill();
        }
    }

    public virtual void Kill() {
        // Handle enemy death (e.g., play death animation, destroy the object)
        //Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke(transform, EventArgs.Empty);
        Destroy(gameObject); // Destroy the enemy GameObject
    }
}
