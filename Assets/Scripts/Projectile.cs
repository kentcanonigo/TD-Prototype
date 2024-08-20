using System;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private float damage;
    private float speed;
    [SerializeField] public ProjectileSO projectileSO;
    private float timer;

    // Set up the projectile's damage
    public void SetBaseData(float damage, float speed) {
        this.damage = damage;
        this.speed = speed;
    }

    private void Start() {
        timer = projectileSO.expireTime;
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer < 0) {
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
        // Move the projectile forward based on its speed in the direction it is facing
        transform.Translate(Vector3.right * (speed * Time.fixedDeltaTime));
    }

    bool hasHit = false;
    
    // Trigger collision detection
    void OnTriggerEnter2D(Collider2D collision) {
        IHasHealth targetHealth = collision.GetComponentInParent<IHasHealth>();
        if (!hasHit) {
            if (targetHealth != null) {
                SpawnParticles(collision.GetComponent<Transform>().position);
                HitTarget(targetHealth);
            } else {
                // Handle cases where the projectile hits something else, like terrain or obstacles
                Destroy(gameObject);
            }
            hasHit = true;
        }
    }

    // Handle hitting the target
    void HitTarget(IHasHealth targetHealth) {
        targetHealth.TakeDamage(damage);
        
        Destroy(gameObject); // Destroy the projectile after it hits the target
    }

    private void SpawnParticles(Vector3 hitPosition) {
        if (projectileSO.damageTypeSO.damageTypeEffect != null) {
            // Calculate the direction from the hit position to the projectile's position
            Vector3 direction = (transform.position - hitPosition).normalized;
        
            // Calculate the rotation to face the opposite direction
            Quaternion spawnRotation = Quaternion.LookRotation(Vector3.forward, direction);
        
            // Instantiate the particle effect at the projectile's position with the calculated rotation
            Instantiate(projectileSO.damageTypeSO.damageTypeEffect, transform.position, spawnRotation);
        }
    }
}