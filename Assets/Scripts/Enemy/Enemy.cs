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
    public int CreditValue { get; private set; } // Credits dropped when the enemy dies
    public bool IsDead => HealthPoints <= 0; // Implementing IsDead property
    public event IHasHealth.DeathHandler<Transform> OnDeath;
    public event EventHandler<IHasHealth.OnHealthChangedEventArgs> OnHealthChanged;

    private void Awake() {
        TotalHealthPoints = enemySO.healthPoints; // Set stats from the ScriptableObject
        Armor = enemySO.armor;
        Speed = enemySO.speed;
        SizeMultiplier = enemySO.sizeMultiplier;
        DamageToCore = enemySO.damageToCore;
        CreditValue = enemySO.creditValue;
        HealthPoints = TotalHealthPoints;
    }

    private void Start() {
        transform.localScale *= SizeMultiplier;
    }

    public void TakeDamage(float damage) {
        // Calculate actual damage after considering armor
        float actualDamage = Mathf.Max(damage - Armor, 0);
        HealthPoints -= (int)actualDamage;

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
        GameManager.Instance.AddCredits(CreditValue);
        Destroy(gameObject); // Destroy the enemy GameObject
    }
    
    public void SetHealth(int health) {
        TotalHealthPoints = health;
        HealthPoints = health;
        OnHealthChanged?.Invoke(this, new IHasHealth.OnHealthChangedEventArgs {
            healthNormalized = (float)HealthPoints / TotalHealthPoints
        });
    }
}
