using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelDataSO", menuName = "New Level Data")]
public class LevelDataSO : ScriptableObject {
    public string levelName;
    public Vector2Int levelSize;
    public int totalCoreHP;
    public int startingModules;
    public List<int> moduleRewards;
    public TextAsset levelFile;
    public Vector2Int spiralPosition;
    public List<WaveSO> waveSOList;
    public List<Vector2Int> vortexPositions;
    
    public string GetFileName() {
        return levelFile != null ? levelFile.name : string.Empty;
    }

    // Custom method to initialize default data
    public void InitializeDefaults() {
        levelSize = new Vector2Int(18, 14);
        startingModules = 3;
        moduleRewards = new List<int> { 1, 2, 3 };
        totalCoreHP = 25;
    }
}


[CustomEditor(typeof(LevelDataSO))]
public class LevelDataSOEditor : Editor {
    private void OnEnable() {
        LevelDataSO levelDataSO = (LevelDataSO)target;

        // Initialize default data when the ScriptableObject is first created
        if (levelDataSO.moduleRewards == null || levelDataSO.moduleRewards.Count == 0) {
            levelDataSO.InitializeDefaults();
            EditorUtility.SetDirty(levelDataSO);
        }
    }
}