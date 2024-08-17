using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] public bool showDebugMenu;
    [SerializeField] public bool mapEditMode;
    [SerializeField] public EnemySO testEnemySO;
    
    public static GameManager Instance { get; private set; }

    public int CurrentCredits { get; private set; }
    public int CurrentCoreHP { get; private set; }
    public int MaxCoreHP { get; private set; }
    public int CurrentWave { get; private set; }
    public int StartingModules { get; private set; }
    public int CurrentModules { get; private set; }
    public List<WaveSO> waveSOList { get; private set; }
    public List<int> ModuleRewardsList { get; private set; }

    public event EventHandler OnValueChanged;
    public event EventHandler OnGameStateChanged;
    
    public GameState CurrentGameState { get; private set; }
    
    public enum GameState {
        Loading,
        BuildPhase,
        WavePhase,
        EndWavePhase,
        Victory
    }

    private void Awake() {
        Instance = this;
        CurrentGameState = GameState.Loading;
        
        // Initialize default values in case level data does not have data
        CurrentWave = 0;
        waveSOList = null;
        StartingModules = 0;
        ModuleRewardsList = null;
        MaxCoreHP = 25;
        CurrentModules = StartingModules;
        CurrentCoreHP = MaxCoreHP;
        CurrentCredits = 0;
        OnValueChanged?.Invoke(this, EventArgs.Empty);
    }

    public void LoadLevelData(LevelDataSO levelDataSO) {
        waveSOList = levelDataSO.waveSOList;
        StartingModules = levelDataSO.startingModules;
        ModuleRewardsList = levelDataSO.moduleRewards;
        MaxCoreHP = levelDataSO.totalCoreHP;
        CurrentCredits = levelDataSO.startingCredits;
        CurrentModules = StartingModules;
        CurrentCoreHP = MaxCoreHP;
        OnValueChanged?.Invoke(this, EventArgs.Empty);
        
        CurrentGameState = GameState.BuildPhase;
    }

    // Helper Functions

    public void SpawnTestEnemy() {
        Vector3 spawnPosition = GridManager.Instance.GetWorldPosition(GridManager.Instance.GetMainVortexPosition()) + new Vector3(GridManager.Instance.GetCellSize() / 2, GridManager.Instance.GetCellSize() / 2, 0);
        EnemyPathfinder currentEnemyPathfinder = Instantiate(testEnemySO.enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<EnemyPathfinder>();

        // Get the path from the GridManager
        if (GridManager.Instance.GetMainVortexPosition() is GridMapObject vortexNode) {
            if (GridManager.Instance.GetVortexPathDictionary().TryGetValue(vortexNode, out List<GridMapObject> pathGridObjects)) {
                // Convert the GridMapObjects to world positions at the center of each cell
                List<Vector3> path = new List<Vector3>();
                foreach (var gridObject in pathGridObjects) {
                    // Calculate the center position of each cell
                    Vector3 centerPosition = GridManager.Instance.GetWorldPosition(gridObject.x, gridObject.y) + new Vector3(GridManager.Instance.GetCellSize() / 2, GridManager.Instance.GetCellSize() / 2, 0);
                    path.Add(centerPosition);
                }

                // Initialize the Glorplax (or any enemy) with the path
                currentEnemyPathfinder.InitializePath(path);
            }
        }
    }
    
    public void DecrementModuleCount() {
        if (CurrentModules > 0) {
            CurrentModules--;
            OnValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // TODO : Add Module Count based on Wave
    public void AddModuleCount(int amount) {
        CurrentModules += amount;
        OnValueChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public void DecreaseCoreHP(int amount) {
        if (CurrentCoreHP - amount > 0) {
            CurrentCoreHP--;
            OnValueChanged?.Invoke(this, EventArgs.Empty);
        } else {
            CurrentCoreHP--;
            OnValueChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("Core HP is zero!");
            // TODO: Game over logic
        }
    }

    public void AddCoreHP(int amount) {
        CurrentCoreHP++;
        OnValueChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTotalWaves() {
        return waveSOList.Count;
    }
    
    public void AddCredits(int amount) {
        CurrentCredits += amount;
        OnValueChanged?.Invoke(this, EventArgs.Empty);
    }
    
    // Game State Functions
    
    public void StartWave() {
        //Debug.Log("Start Wave called!");
        if (CurrentGameState == GameState.BuildPhase) {
            //Debug.Log("Now in wave phase!");
            CurrentGameState = GameState.WavePhase;
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);
            // TODO: Start wave logic (e.g., spawn enemies)
        } else {
            Debug.Log($"Cannot start wave. {CurrentGameState}");
        }
    }
    
    public void EndWave() {
        //Debug.Log("End Wave called!");
        if (CurrentGameState == GameState.WavePhase) {
            //Debug.Log("Now in End Wave phase!");
            CurrentGameState = GameState.EndWavePhase;
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);
            // TODO: Handle end of wave logic (e.g., rewards, reset, etc.)
            CompleteEndWave();
        } else {
            Debug.Log($"Cannot end wave. {CurrentGameState}");
        }
    }

    public void CompleteEndWave() {
        //Debug.Log("Complete End Wave called!");
        if (CurrentGameState == GameState.EndWavePhase && CurrentWave < waveSOList.Count) {
            // Return to the BuildPhase after completing end wave actions
            Debug.Log("Wave Complete! +1 to current wave");
            CurrentGameState = GameState.BuildPhase;
            CurrentWave++;
            OnValueChanged?.Invoke(this,EventArgs.Empty);
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);
            // TODO: Prepare for the next wave or any other logic
        } else {
            // Last wave completed
            // TODO: Logic for completing the level
            CurrentGameState = GameState.Victory;
            Debug.Log("Game Complete!");
        }
    }

    // Game Loop Functions
    
    private void Update() {
        switch (CurrentGameState) {
            case GameState.BuildPhase:
                // Handle logic for build phase
                HandleBuildPhase();
                break;

            case GameState.WavePhase:
                // Handle logic for wave phase
                HandleWavePhase();
                break;

            case GameState.EndWavePhase:
                // Handle logic for end wave phase
                HandleEndWavePhase();
                break;
        }
        //Debug.Log(CurrentGameState);
    }

    private void HandleBuildPhase() {
        // Logic for the build phase (e.g., waiting for player input to start a wave)
    }

    private void HandleWavePhase() {
        // Logic for the wave phase (e.g., spawning enemies)
        // Example: Check if the current wave has ended
    }

    private void HandleEndWavePhase() {
        // Logic for the end wave phase (e.g., processing rewards, resetting state)
    }
    
    
}