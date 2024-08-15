using System;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private int damage;

    public float speed;
    public float timeToDestroySelf = 20f;
    public GameObject impactEffect;

    // Set up the projectile's damage
    public void SetBaseData(int damage, float speed) {
        this.damage = damage;
        this.speed = speed;
    }

    private void Update() {
        timeToDestroySelf -= Time.deltaTime;
        if (timeToDestroySelf < 0) {
            Destroy(gameObject);
        }
    }

    // FixedUpdate for consistent physics calculations
    void FixedUpdate() {
        // Move the projectile forward based on its speed in the direction it is facing
        transform.Translate(Vector3.right * (speed * Time.fixedDeltaTime));
    }

    // Trigger collision detection
    void OnTriggerEnter2D(Collider2D collision) {
        IHasHealth targetHealth = collision.GetComponent<IHasHealth>();
        if (targetHealth != null) {
            HitTarget(targetHealth);
        } else {
            // Handle cases where the projectile hits something else, like terrain or obstacles
            Destroy(gameObject);
        }
    }

    // Handle hitting the target
    void HitTarget(IHasHealth targetHealth) {
        targetHealth.TakeDamage(damage);

        if (impactEffect != null) {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject); // Destroy the projectile after it hits the target
    }
}