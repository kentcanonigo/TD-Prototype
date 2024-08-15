using UnityEngine;

[CreateAssetMenu(fileName = "NewWiggleSO", menuName = "New Enemy Wiggle Setting")]
public class WiggleSO : ScriptableObject {
    [Header("How much the enemy will move around in place")]
    public float wiggleSpeed = 1f;
    public float wigglePosRange = 0.2f;
    public float wiggleRotRange = 0.2f;
}