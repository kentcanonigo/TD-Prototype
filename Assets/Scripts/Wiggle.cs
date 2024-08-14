using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    [Header("Animation Size")]
    public float animationLoopTime = 1.0f; // Time to complete one wiggle cycle
    public float posRange = 2.0f; // Range for position wiggle
    public float rotRange = 1.0f; // Range for rotation wiggle

    [Header("Settings")]
    public bool noZMove = true; // Prevent movement in the Z-axis
    public bool shouldReturnToStart = false; // Return to starting position
    public bool lockRotToZ = false; // Lock rotation on the Z-axis

    private Vector3 initPos, initRot;
    private float timePassed;
    private float noiseOffset; // Random offset for Perlin noise

    void Start()
    {
        initPos = transform.localPosition;
        initRot = transform.localRotation.eulerAngles;

        // Initialize a random offset for Perlin noise
        noiseOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        timePassed += Time.deltaTime / animationLoopTime;

        // Use Perlin noise with an offset for smooth and unpredictable motion
        float noiseX = Mathf.PerlinNoise(timePassed + noiseOffset, 0) * posRange - (posRange / 2);
        float noiseY = Mathf.PerlinNoise(timePassed + noiseOffset, 1) * posRange - (posRange / 2);
        float noiseZ = noZMove ? 0 : Mathf.PerlinNoise(timePassed + noiseOffset, 2) * posRange - (posRange / 2);

        // Set the new position based on the noise
        Vector3 newPosition = initPos + new Vector3(noiseX, noiseY, noiseZ);
        transform.localPosition = newPosition;

        // Use Perlin noise for rotation with an offset
        float noiseRotX = lockRotToZ ? 0 : Mathf.PerlinNoise(timePassed + noiseOffset, 3) * rotRange - (rotRange / 2);
        float noiseRotY = lockRotToZ ? 0 : Mathf.PerlinNoise(timePassed + noiseOffset, 4) * rotRange - (rotRange / 2);
        float noiseRotZ = Mathf.PerlinNoise(timePassed + noiseOffset, 5) * rotRange - (rotRange / 2);

        // Set the new rotation based on the noise
        Quaternion newRotation = Quaternion.Euler(initRot.x + noiseRotX, initRot.y + noiseRotY, initRot.z + noiseRotZ);
        transform.localRotation = newRotation;
    }
}
