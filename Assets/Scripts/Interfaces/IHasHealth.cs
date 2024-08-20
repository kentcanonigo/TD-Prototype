using System;
using UnityEngine;

public interface IHasHealth {
    int HealthPoints { get; }
    int Armor { get; }
    void TakeDamage(float damage);
    void Kill();
    bool IsDead { get; }
    event DeathHandler<Transform> OnDeath;
    public delegate void DeathHandler<T>(Transform transform, EventArgs args);
    event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs {
        public float healthNormalized;
    }
}

