using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI waveUIText;
    [FormerlySerializedAs("spiralHPUIText")] [SerializeField] private TextMeshProUGUI coreHPUIText;
    [SerializeField] private TextMeshProUGUI modulesUIText;
    
    [SerializeField] private CanvasGroup debugCanvasGroup;
    [SerializeField] private Button decrementHPButton;
    [SerializeField] private Button incrementHPButton;
    [SerializeField] private Button startWaveButton;
    [SerializeField] private Button endWaveButton;
    [SerializeField] private Button spawnEnemyButton;
    [SerializeField] private TextMeshProUGUI gameStateText;

    private void Awake() {
        decrementHPButton.onClick.AddListener((() => {
            GameManager.Instance.DecreaseCoreHP(1);
        }));
        
        incrementHPButton.onClick.AddListener((() => {
            GameManager.Instance.AddCoreHP(1);
        }));
        
        startWaveButton.onClick.AddListener((() => {
            GameManager.Instance.StartWave();
        }));
        
        endWaveButton.onClick.AddListener((() => {
            GameManager.Instance.EndWave();
        }));        
        
        spawnEnemyButton.onClick.AddListener((() => {
            GameManager.Instance.SpawnTestEnemy();
        }));
    }

    private void Start() {
        GameManager.Instance.OnValueChanged += GameManager_OnValueChanged;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        debugCanvasGroup.gameObject.SetActive(GameManager.Instance.showDebugMenu);
    }

    private void GameManager_OnGameStateChanged(object sender, EventArgs e) {
        Debug.Log("Gamestate changed has changed");
        gameStateText.text = $"Gamestate: {GameManager.Instance.CurrentGameState}";
    }

    private void GameManager_OnValueChanged(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        //Debug.Log($"Updating UI: Wave: {GameManager.Instance.CurrentWave}/{GameManager.Instance.NumberOfWaves}, Core HP: {GameManager.Instance.CurrentCoreHP}/{GameManager.Instance.MaxCoreHP}, Modules: {GameManager.Instance.CurrentModules}");
        waveUIText.text = $"Wave: {GameManager.Instance.CurrentWave}/{GameManager.Instance.GetTotalWaves()}";
        coreHPUIText.text = $"Core HP: {GameManager.Instance.CurrentCoreHP}/{GameManager.Instance.MaxCoreHP}";
        modulesUIText.text = $"Modules: {GameManager.Instance.CurrentModules}";
    }

}
