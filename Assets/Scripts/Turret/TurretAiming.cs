using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAiming : MonoBehaviour {
    [SerializeField] private Transform pivotPoint; // Where to pivot the gun

    [SerializeField] private AimSmoothing aimSmoothing = AimSmoothing.Lerp; // The method of aim smoothing
    [SerializeField] private float rotationSpeedMultiplier = 2f;

    private enum AimSmoothing {
        RotateTowards,
        Slerp,
        Lerp,
    }
    
    
    [field: SerializeField] public bool IsAutoAim { get; private set; }
    private Turret turret;
    private TurretTargetSelection turretTargetSelection;

    private void Awake() {
        turret = GetComponent<Turret>();
        turretTargetSelection = GetComponent<TurretTargetSelection>();
    }

    private void Update() {
        if (IsAutoAim) {
            // Aim at the selected target
            AimAtEnemy(turretTargetSelection.SelectedTarget, turret.BaseRotationSpeed);
        } else {
            // Autoaim is disabled, manually aim at the target
        }
    }

    private Quaternion targetRotation;

    public void AimAtEnemy(Transform target, float rotationSpeed) {
        if (target == null) return;

        // Calculate the direction and angle to the target
        Vector3 direction = target.position - pivotPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create the target rotation
        targetRotation = Quaternion.Euler(0f, 0f, angle);

        if (!IsCloseToTarget(20f)) {
            // If the target is not close to the crosshairs, use RotateTowards
            pivotPoint.rotation = Quaternion.RotateTowards(pivotPoint.rotation, targetRotation, rotationSpeed * 50f * Time.deltaTime);
        } else {
            // If the target is close to the crosshairs, use whatever aim smoothing is set
            switch (aimSmoothing) {
                case AimSmoothing.Lerp:
                    pivotPoint.rotation = Quaternion.Lerp(pivotPoint.rotation, targetRotation, rotationSpeed * rotationSpeedMultiplier * Time.deltaTime);
                    break;
                case AimSmoothing.Slerp:
                    pivotPoint.rotation = Quaternion.Slerp(pivotPoint.rotation, targetRotation, rotationSpeed * rotationSpeedMultiplier * Time.deltaTime);
                    break;
                case AimSmoothing.RotateTowards:
                    pivotPoint.rotation = Quaternion.RotateTowards(pivotPoint.rotation, targetRotation, rotationSpeed * rotationSpeedMultiplier * Time.deltaTime);
                    break;
            }
        }
    }

    public bool IsCloseToTarget(float angleDifference) {
        angleDifference = Quaternion.Angle(pivotPoint.rotation, targetRotation);
        if (angleDifference < 20f) { // Adjust the angle threshold as needed
            return true;
        }
        return false;
    }
}