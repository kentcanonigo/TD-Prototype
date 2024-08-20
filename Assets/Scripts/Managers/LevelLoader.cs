using System;
using CodeMonkey;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    
    public static LevelLoader Instance { get; private set; }
    
    [InlineEditor]
    [SerializeField] public LevelDataSO levelDataToLoad;
    [SerializeField] private bool alwaysShowGridGizmos;
    [SerializeField] private bool showGridNumberGizmos;
    
    private void OnDrawGizmos() {
        if (alwaysShowGridGizmos) {
            int gridHeight = levelDataToLoad.levelSize.y;
            int gridWidth = levelDataToLoad.levelSize.x;
            Gizmos.color = Color.white; // Set the color for the grid lines
            
            for (int x = 0; x < gridWidth; x++) {
                for (int y = 0; y < gridHeight; y++) {
#if UNITY_EDITOR
                    if (showGridNumberGizmos) {
                        Handles.Label(GetWorldPosition(x, y) + new Vector3(1f / 2 - .4f, 1f / 2, 0), $"({x}, {y})");
                    }
                    
                    Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1));
                    Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y));
#endif
                }
            }

            Gizmos.DrawLine(GetWorldPosition(0, gridHeight), GetWorldPosition(gridWidth, gridHeight));
            Gizmos.DrawLine(GetWorldPosition(gridWidth, 0), GetWorldPosition(gridWidth, gridHeight));
        }
    }

    private void OnDrawGizmosSelected() {
        if (!alwaysShowGridGizmos) {
            int gridHeight = levelDataToLoad.levelSize.y;
            int gridWidth = levelDataToLoad.levelSize.x;
            Gizmos.color = Color.green; // Set the color for the grid lines
            
            for (int x = 0; x < gridWidth; x++) {
                for (int y = 0; y < gridHeight; y++) {
#if UNITY_EDITOR
                    if (showGridNumberGizmos) {
                        Handles.Label(GetWorldPosition(x, y) + new Vector3(1f / 2 - .4f, 1f / 2, 0), $"({x}, {y})");
                    }
                    
                    Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1));
                    Gizmos.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y));
#endif
                }
            }

            Gizmos.DrawLine(GetWorldPosition(0, gridHeight), GetWorldPosition(gridWidth, gridHeight));
            Gizmos.DrawLine(GetWorldPosition(gridWidth, 0), GetWorldPosition(gridWidth, gridHeight));
        }
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * 1f + Vector3.zero;
    }
    
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
            Debug.LogWarning("No level data to load!");
            GridManager.Instance.InitializeGrid(levelDataSO.levelSize.x, levelDataSO.levelSize.y);
            callback?.Invoke();
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