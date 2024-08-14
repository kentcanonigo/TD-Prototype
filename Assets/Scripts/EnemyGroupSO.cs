using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewWave", menuName = "New Enemy Group")]
public class EnemyGroupSO : ScriptableObject  {
    [Header("Group Information")]
    public EnemySO enemySO; // Reference to the enemy ScriptableObject
    public int amountToSpawn;        // Number of enemies to spawn in this group
}
