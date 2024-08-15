using System;
using UnityEngine;

public interface IHasHealth {
    int HealthPoints { get; }
    int Armor { get; }
    void TakeDamage(int damage);
    void Kill();
    bool IsDead { get; }
    event EnemyDeathHandler<Transform> OnEnemyDeath;
}

public delegate void EnemyDeathHandler<T>(Transform transform, EventArgs args);
