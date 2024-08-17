using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class Enemy : MonoBehaviour, IHasHealth {
    [Header("Enemy")]
    [Required]
    [SerializeField] private EnemySO enemySO; // Reference to the enemy stats ScriptableObject

    public int HealthPoints { get; private set; } // Health points of the enemy
    public int TotalHealthPoints { get; private set; } // Total HP of the enemy
    public int Armor { get; private set; } // Armor value that reduces incoming damage
    public float Speed { get; private set; } // Speed at which the enemy moves
    public float SizeMultiplier { get; private set; } // SizeMultiplier; // Size multiplier of the enemy
    public int DamageToCore { get; private set; } // Damage that the enemy deals to the core
    public bool IsDead => HealthPoints <= 0; // Implementing IsDead property
    public event IHasHealth.DeathHandler<Transform> OnDeath;
    public event EventHandler<IHasHealth.OnHealthChangedEventArgs> OnHealthChanged;

    private void Awake() {
        TotalHealthPoints = enemySO.healthPoints; // Set stats from the ScriptableObject
        Armor = enemySO.armor;
        Speed = enemySO.speed;
        SizeMultiplier = enemySO.sizeMultiplier;
        DamageToCore = enemySO.damageToCore;
        HealthPoints = TotalHealthPoints;
    }

    private void Start() {
        transform.localScale *= SizeMultiplier;
    }

    public void TakeDamage(int damage) {
        // Calculate actual damage after considering armor
        int actualDamage = Mathf.Max(damage - Armor, 0);
        HealthPoints -= actualDamage;

        OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs {
            healthNormalized = (float)HealthPoints / TotalHealthPoints
        });
        
        // Check if the enemy is dead
        if (IsDead) {
            Kill();
        }
    }

    public void Kill() {
        // Handle enemy death (e.g., play death animation, destroy the object)
        //Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke(transform, EventArgs.Empty);
        Destroy(gameObject); // Destroy the enemy GameObject
    }
    
    public void SetHealth(int health) {
        TotalHealthPoints = health;
        HealthPoints = health;
        OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs {
            healthNormalized = (float)HealthPoints / TotalHealthPoints
        });
    }
    
    public void SetArmor(int armor) {
        Armor = armor;
    }
    
    public void SetSpeed(float speed) {
        Speed = speed;
    }
    
    public void SetSizeMultiplier(float sizeMultiplier) {
        SizeMultiplier = sizeMultiplier;
        transform.localScale *= SizeMultiplier;
    }
    
    public void SetDamageToCore(int damageToCore) {
        DamageToCore = damageToCore;
    }
}
