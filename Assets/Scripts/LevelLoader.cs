using System;
using CodeMonkey;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    
    public static LevelLoader Instance { get; private set; }
    
    [SerializeField] private LevelDataSO levelDataToLoad;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GridManager.Instance.OnGridMapInitialized += GridManager_OnGridMapInitialized;
        LoadLevel(levelDataToLoad);
    }

    private void GridManager_OnGridMapInitialized(object sender, EventArgs e) {
        Debug.Log($"Grid map initialized. Loading {levelDataToLoad.name}");
        GridManager.Instance.InitializeLevel(levelDataToLoad);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadLevel(levelDataToLoad);
            CMDebug.TextPopupMouse($"Loading {levelDataToLoad.levelName}");
        }
    }

    public void LoadLevel(LevelDataSO levelDataSO) {
        // Test if the level actually has a level file
        if (levelDataSO == null || levelDataSO.levelFile == null || string.IsNullOrEmpty(levelDataSO.levelFile.name)) {
            Debug.LogError("No level data to load!");
        } else {
            // Trigger GridManager initialization
            GridManager.Instance.InitializeGrid(levelDataSO.levelSize.x, levelDataSO.levelSize.y);
        }
    }
}

