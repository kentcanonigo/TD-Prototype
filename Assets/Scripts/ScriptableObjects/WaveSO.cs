using System.Collections.Generic;
using UnityEngine;
using VHierarchy.Libs;

[CreateAssetMenu(fileName = "NewWaveSO", menuName = "New Wave")]
public class WaveSO : ScriptableObject {
    [Header("Groups to spawn in wave")]
    [SerializeField] public List<EnemySO> groupsToSpawnInWave;
    public float secondsBeforeNextGroup = 10f;
}