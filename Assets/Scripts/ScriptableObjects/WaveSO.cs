using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveSO", menuName = "New Wave")]
public class WaveSO : ScriptableObject {
    [Header("Groups to spawn in wave")]
    public List<EnemyGroupSO> enemyGroupSO; // List of enemy groups to spawn this wave
    public float secondsBeforeNextGroup = 10f;
}