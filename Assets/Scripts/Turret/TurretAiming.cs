using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAiming : MonoBehaviour {
    [SerializeField] protected Transform pivotPoint; // Where to pivot the gun
    
    public void Aim(Transform target, float rotationSpeed) {
        if (target == null) return;

        // Calculate the direction and angle to the target
        Vector3 direction = target.position - pivotPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create the target rotation
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        // Smoothly rotate towards the target
        pivotPoint.rotation = Quaternion.Lerp(pivotPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    
        Debug.Log($"Aiming at target: {target.name}");
    
        // Shoot if the rotation is sufficiently close to the target rotation
        
        
        /*
         
          NOTE: this should be in the TurretFiring script
          
         float angleDifference = Quaternion.Angle(pivotPoint.rotation, targetRotation);
        if (angleDifference < 20f && fireCooldown <= 0f) { // Adjust the angle threshold as needed
            throw new NotImplementedException("TurretFiring.Fire()");
        }
        
        */
    }
}
