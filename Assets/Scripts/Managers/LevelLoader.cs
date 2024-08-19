using System;
using CodeMonkey;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    
    public static LevelLoader Instance { get; private set; }
    
    [InlineEditor]
    [SerializeField] private LevelDataSO levelDataToLoad;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        LoadLevelGrid(levelDataToLoad, OnLevelGridLoaded);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadLevelGrid(levelDataToLoad, OnLevelGridLoaded);
            CMDebug.TextPopupMouse($"Loading {levelDataToLoad.levelName}");
        }
    }

    public void LoadLevelGrid(LevelDataSO levelDataSO, Action callback) {
        // Test if the level actually has a level file
        if (!levelDataSO || !levelDataSO.levelFile || string.IsNullOrEmpty(levelDataSO.levelFile.name)) {
            Debug.LogError("No level data to load!");
        } else {
            // Trigger GridManager initialization
            GridManager.Instance.InitializeGrid(levelDataSO.levelSize.x, levelDataSO.levelSize.y);
            callback?.Invoke();
        }
    }

    private void OnLevelGridLoaded() {
        Debug.Log($"Grid map initialized. Loading {levelDataToLoad.name}");
        GridManager.Instance.InitializeLevel(levelDataToLoad);
        GameManager.Instance.LoadLevelData(levelDataToLoad);
    }
}