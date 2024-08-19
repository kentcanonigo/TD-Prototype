using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI waveUIText;
    [SerializeField] private TextMeshProUGUI creditsText;
    [FormerlySerializedAs("spiralHPUIText")] [SerializeField] private TextMeshProUGUI coreHPUIText;
    [SerializeField] private TextMeshProUGUI modulesUIText;
    [SerializeField] private Animator debugMenuAnimator;
    [SerializeField] private Button toggleDebugMenuButton;
    [SerializeField] private Toggle toggleMapEditModeButton;
    
    [SerializeField] private CanvasGroup debugCanvasGroup;
    [SerializeField] private Button decrementHPButton;
    [SerializeField] private Button incrementHPButton;
    [SerializeField] private Button startWaveButton;
    [SerializeField] private Button endWaveButton;
    [SerializeField] private Button spawnEnemyButton;
    [SerializeField] private TextMeshProUGUI gameStateText;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

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
        
        toggleDebugMenuButton.onClick.AddListener((() => {
            debugMenuAnimator.SetBool(IsOpen, !debugMenuAnimator.GetBool(IsOpen));
        }));

        toggleMapEditModeButton.onValueChanged.AddListener((value => {
            GameManager.Instance.mapEditMode = value;
        }));
    }

    private void Start() {
        GameManager.Instance.OnValueChanged += GameManager_OnValueChanged;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        debugCanvasGroup.gameObject.SetActive(GameManager.Instance.showDebugMenu);
        UpdateVisual();
    }

    private void GameManager_OnGameStateChanged(object sender, EventArgs e) {
        //Debug.Log("Gamestate changed has changed");
        gameStateText.text = $"Gamestate: {GameManager.Instance.CurrentGameState}";
        UpdateVisual();
    }

    private void GameManager_OnValueChanged(object sender, EventArgs e) {
        //Debug.Log("Values in game has changed");
        UpdateVisual();
    }

    private void UpdateVisual() {
        //Debug.Log($"Updating Visuals");
        waveUIText.text = $"Wave: {GameManager.Instance.CurrentWave}/{GameManager.Instance.GetTotalWaves()}";
        coreHPUIText.text = $"Core HP: {GameManager.Instance.CurrentCoreHP}/{GameManager.Instance.MaxCoreHP}";
        modulesUIText.text = $"Modules: {GameManager.Instance.CurrentModules}";
        creditsText.text =  $"Credits: {GameManager.Instance.CurrentCredits}";
    }

}
