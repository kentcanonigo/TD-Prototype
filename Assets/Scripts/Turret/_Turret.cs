using System.Collections.Generic;
using UnityEngine;

public class _Turret : MonoBehaviour {
    [Header("Turret Stats")]
    [SerializeField] protected TurretSO turretSO;

    [SerializeField] private Transform firePoint; // The point from where the projectile is fired
    [SerializeField] private Transform pivotPoint; // Where to pivot the gun
    [SerializeField] private GameObject projectilePrefab; // The projectile that will be fired

    [SerializeField] public float rotationSpeed;

    [SerializeField] protected int damage;
    protected float range;
    protected bool isSplashDamage;
    protected float splashDamageRadius;
    protected int cost;
    protected int[] upgradeCosts;

    [Header("Targeting")] protected List<Transform> enemiesInRange;
    protected Transform currentTarget;

    // Enum for targeting preferences
    public enum TargetingPreference { Closest, Furthest, LowestHealth, HighestHealth }
    public TargetingPreference targetingPreference;

    protected virtual void Start() {
        // Assign turret stats from the ScriptableObject
        damage = turretSO.baseDamage;
        range = turretSO.baseRange;
        //isSplashDamage = turretSO.isSplashDamage;
        //splashDamageRadius = turretSO.splashDamageRadius;
        //cost = turretSO.cost;
        //upgradeCosts = turretSO.upgradeCosts;
        enemiesInRange = new List<Transform>();

        // Add a sphere collider to detect enemies in range
        CircleCollider2D rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = range;

        Debug.Log($"{turretSO.turretName} turret initialized with range: {range}");
    }

    protected virtual void Update() {
        fireCooldown -= Time.deltaTime;
        SelectTarget();
        if (currentTarget) { // If there is a current target
            Aim();
        } else {
            IdleRotation();
        }
    }

    protected virtual void Aim() {
        if (currentTarget == null) return;

        // Calculate the direction and angle to the target
        Vector3 direction = currentTarget.position - pivotPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create the target rotation
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        // Smoothly rotate towards the target
        pivotPoint.rotation = Quaternion.Lerp(pivotPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    
        Debug.Log($"Aiming at target: {currentTarget.name}");
    
        // Shoot if the rotation is sufficiently close to the target rotation
        float angleDifference = Quaternion.Angle(pivotPoint.rotation, targetRotation);
        if (angleDifference < 20f && fireCooldown <= 0f) { // Adjust the angle threshold as needed
            Shoot();
        }
    }

    
    private float idleRotationTimer = 5f;
    private float idleRotationInterval = 8f; // Change direction every 3 seconds
    private Quaternion randomRotation;
    
    protected virtual void IdleRotation() {
        idleRotationTimer -= Time.deltaTime;
        if (idleRotationTimer <= 0f) {
            // Generate a new random rotation on the Z axis
            float randomAngle = Random.Range(0f, 360f);
            randomRotation = Quaternion.Euler(0f, 0f, randomAngle);

            idleRotationTimer = idleRotationInterval; // Reset the timer
            Debug.Log($"Idle rotation: New random angle = {randomAngle}");
        }

        // Smoothly rotate towards the random direction
        pivotPoint.rotation = Quaternion.RotateTowards(pivotPoint.rotation, randomRotation, Time.deltaTime * 10f);
    }
    
    [SerializeField] private float rateOfFire = 1f; // Time between shots in seconds
    private float fireCooldown = 0f;

    protected virtual void Shoot() {
        if (fireCooldown <= 0f) {
            // Instantiate the projectile and shoot it towards the target
            if (firePoint != null && projectilePrefab != null) {
                GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Projectile projectile = projectileGO.GetComponent<Projectile>();
                if (projectile != null) {
                    projectile.SetBaseData(damage, 1);
                    Debug.Log($"Shot fired towards {currentTarget.name} with damage: {damage}");
                } else {
                    Debug.LogError("Projectile prefab does not have a Projectile component.");
                }
            } else {
                Debug.LogError("FirePoint or ProjectilePrefab is not set.");
            }

            fireCooldown = 1f / rateOfFire; // Reset the cooldown timer based on the rate of fire
        }
    }



    protected virtual void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("TriggerEnter");
        if (other.CompareTag("Enemy")) {
            enemiesInRange.Add(other.transform);
            Debug.Log($"Enemy entered range: {other.name}");
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other) {
        Debug.Log("TriggerExit");
        if (other.CompareTag("Enemy")) {
            enemiesInRange.Remove(other.transform);
            Debug.Log($"Enemy exited range: {other.name}");
        }
    }

    protected virtual void SelectTarget() {
        if (enemiesInRange.Count <= 0) {
            currentTarget = null;
            Debug.Log("No enemies in range to select.");
            return;
        }

        switch (targetingPreference) {
            case TargetingPreference.Closest:
                currentTarget = GetClosestTarget();
                break;
            case TargetingPreference.Furthest:
                currentTarget = GetFurthestTarget();
                break;
            case TargetingPreference.LowestHealth:
                currentTarget = GetLowestHealthTarget();
                break;
            case TargetingPreference.HighestHealth:
                currentTarget = GetHighestHealthTarget();
                break;
        }

        if (currentTarget != null) {
            Debug.Log($"Target selected: {currentTarget.name}");
        }
    }

    protected Transform GetClosestTarget() {
        Transform closestTarget = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange) {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestTarget = enemy;
            }
        }

        if (closestTarget) {
            Debug.Log($"Closest target is: {closestTarget.name}");
        }
        return closestTarget;
    }

    protected Transform GetFurthestTarget() {
        Transform furthestTarget = null;
        float maxDistance = 0f;

        foreach (Transform enemy in enemiesInRange) {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance > maxDistance) {
                maxDistance = distance;
                furthestTarget = enemy;
            }
        }

        if (furthestTarget) {
            Debug.Log($"Furthest target is: {furthestTarget.name}");
        }
        return furthestTarget;
    }

    protected Transform GetLowestHealthTarget() {
        Transform lowestHealthTarget = null;
        float minHealth = Mathf.Infinity;

        foreach (Transform enemy in enemiesInRange) {
            IHasHealth healthComponent = enemy.GetComponent<IHasHealth>();
            if (healthComponent != null && healthComponent.HealthPoints < minHealth) {
                minHealth = healthComponent.HealthPoints;
                lowestHealthTarget = enemy;
            }
        }

        if (lowestHealthTarget) {
            Debug.Log($"Lowest health target is: {lowestHealthTarget.name}");
        }
        return lowestHealthTarget;
    }

    protected Transform GetHighestHealthTarget() {
        Transform highestHealthTarget = null;
        float maxHealth = 0f;

        foreach (Transform enemy in enemiesInRange) {
            IHasHealth healthComponent = enemy.GetComponent<IHasHealth>();
            if (healthComponent != null && healthComponent.HealthPoints > maxHealth) {
                maxHealth = healthComponent.HealthPoints;
                highestHealthTarget = enemy;
            }
        }

        if (highestHealthTarget) {
            Debug.Log($"Highest health target is: {highestHealthTarget.name}");
        }
        return highestHealthTarget;
    }
}
