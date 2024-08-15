using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    public float wiggleSpeed = 1.0f; // Time to complete one wiggle cycle
    public float posRange = 1.0f; // Range for position wiggle
    public float rotRange = 1.0f; // Range for rotation wiggle

    private Vector3 initPos, initRot;
    private float timePassed;
    private float noiseOffset; // Random offset for Perlin noise

    private void Start()
    {
        initPos = transform.localPosition;
        initRot = transform.localRotation.eulerAngles;

        // Initialize a random offset for Perlin noise
        noiseOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        timePassed += Time.deltaTime / wiggleSpeed;

        // Use Perlin noise with an offset for smooth and unpredictable motion
        float noiseX = Mathf.PerlinNoise(timePassed + noiseOffset, 0) * posRange - (posRange / 2);
        float noiseY = Mathf.PerlinNoise(timePassed + noiseOffset, 1) * posRange - (posRange / 2);

        // Set the new position based on the noise
        Vector3 newPosition = initPos + new Vector3(noiseX, noiseY, 0);
        transform.localPosition = newPosition;

        // Use Perlin noise for rotation with an offset
        float noiseRotZ = Mathf.PerlinNoise(timePassed + noiseOffset, 5) * rotRange - (rotRange / 2);

        // Set the new rotation based on the noise
        Quaternion newRotation = Quaternion.Euler(initRot.x, initRot.y, initRot.z + noiseRotZ);
        transform.localRotation = newRotation;
    }
}
