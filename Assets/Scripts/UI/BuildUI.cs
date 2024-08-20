using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour {
    [Required] [SceneObjectsOnly] [Header("Module Building")] [SerializeField]
    private GameObject buildModuleUI;

    [SerializeField] private Button buildModuleButton;

    [Required] [Header("Turret Building")] [SerializeField]
    private GameObject buildTurretUI;

    [SerializeField] private Button buildBlasterButton;
    [SerializeField] private Button sellModuleButton;

    [Required] [SceneObjectsOnly] [Header("Turret Info")] [SerializeField]
    private GameObject turretInfoUI;

    [SerializeField] private Button turretInfoButton;
    [SerializeField] private Button turretUpgradeButton;
    [SerializeField] private Button turretSellButton;
    [SerializeField] private Button turretTargetingButton;

    [Required] [SceneObjectsOnly] [Header("Turret Targeting UI")] [SerializeField]
    private GameObject turretTargetingUI;

    [SerializeField] private TextMeshProUGUI currentTargetingText;
    [SerializeField] private Button firstEnteredButton;
    [SerializeField] private Button lastEnteredButton;
    [SerializeField] private Button closestButton;
    [SerializeField] private Button furthestButton;
    [SerializeField] private Button highestHPButton;
    [SerializeField] private Button lowestHPButton;
    
    [Required] [SceneObjectsOnly] [Header("Confirm Build UI")] [SerializeField]
    private GameObject confirmBuildUI;
    [SerializeField] private Button confirmBuildButton;
    [SerializeField] private Button cancelBuildButton;

    private GridMapObject lastSelectedGridObject;
    private Turret lastSelectedTurret;

    private void Awake() {
        buildModuleButton.onClick.AddListener((() => { BuildManager.Instance.BuildModule(); }));

        turretInfoButton.onClick.AddListener((() => {
            ShowTurretInfo();
            InfoUI.Instance.Toggle();
        }));

        turretUpgradeButton.onClick.AddListener((() => { }));

        turretSellButton.onClick.AddListener((() => { BuildManager.Instance.SellTurret(); }));

        sellModuleButton.onClick.AddListener((() => { BuildManager.Instance.SellModule(); }));

        // Turret Targeting

        turretTargetingButton.onClick.AddListener((() => {
            // Toggle targeting menu
            if (turretTargetingUI.activeSelf) {
                turretTargetingUI.gameObject.SetActive(false);
            } else {
                turretTargetingUI.gameObject.SetActive(true);
            }
        }));

        firstEnteredButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.FirstEntered); }));

        lastEnteredButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.LastEntered); }));

        closestButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.Closest); }));

        furthestButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.Furthest); }));

        highestHPButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.HighestHealth); }));

        lowestHPButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.LowestHealth); }));
        
        // Confirm Build
        
        confirmBuildButton.onClick.AddListener((() => { BuildManager.Instance.ConfirmBuild(); }));
        
        cancelBuildButton.onClick.AddListener((() => { BuildManager.Instance.CancelBuild(); }));
    }

    private void SetTurretTargetingText(TurretTargetSelection.TargetingPreference newTargetingPreference) {
        lastSelectedTurret.TryGetTargetingSelection(out TurretTargetSelection targetingSelection);
        targetingSelection.SetTargetingPreference(newTargetingPreference);
        currentTargetingText.text = newTargetingPreference.ToModeString();
        turretTargetingUI.SetActive(false);
    }

    private void ShowTurretInfo() {
        if (lastSelectedGridObject == null) {
            Debug.LogWarning("lastSelectedGridObject is null");
            return;
        }

        InfoUI.Instance.SetInfo(lastSelectedGridObject.GetBuiltTurret().GetTurretSO().turretName, lastSelectedGridObject.GetBuiltTurret().GetTurretSO().turretDescription);
    }

    private void Start() {
        GridSelection.Instance.OnSelectGridCell += GridSelection_OnSelectGridCell;
        GridSelection.Instance.OnDeselectGridCell += GridSelection_OnDeselectGridCell;
        GameManager.Instance.OnValueChanged += GameManager_OnValueChanged;
        HideAllUI();
    }

    private void GameManager_OnValueChanged(object sender, EventArgs e) {
        SetInteractable(buildBlasterButton, IsTurretAffordable(buildBlasterButton));
    }

    private bool IsTurretAffordable(Button button) {
        return button.GetComponent<BuildTurretButtonUI>().turretSO.baseCost <= GameManager.Instance.CurrentCredits;
    }
    
    private void SetInteractable(Button button, bool isInteractable) {
        if (isInteractable) {
            button.interactable = true;
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, 0.2f);
        } else {
            button.interactable = false;
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, 0.05f);
        }
    }

    private void GridSelection_OnSelectGridCell(object sender, GridSelection.OnSelectGridCellEventArgs e) {
        GridMapObject selectedGridObject = GridManager.Instance.TryGetMainGrid().GetGridObject(e.x, e.y);
        if (selectedGridObject == null) {
            Debug.LogWarning("GridObject is null");
            return;
        }

        lastSelectedGridObject = selectedGridObject;
        lastSelectedTurret = selectedGridObject.GetBuiltTurret();
        
        //Update the current turret's targeting mode string
        turretTargetingUI.SetActive(false);
        if (lastSelectedTurret) {
            lastSelectedTurret.TryGetTargetingSelection(out TurretTargetSelection targetingSelection);
            currentTargetingText.text = targetingSelection.targetingPreference.ToModeString();
        }
        
        // Show the appropriate UI
        Show();

        bool isTurretBuilt = selectedGridObject.GetBuiltTurret();
        bool isValidModuleBuildLocation = selectedGridObject.GetNodeType() is GridMapObject.NodeType.None;
        bool isValidTurretBuildLocation = selectedGridObject.GetNodeType() is GridMapObject.NodeType.BuiltModule or GridMapObject.NodeType.PermanentModule;
        bool isPermanentModule = selectedGridObject.GetNodeType() is GridMapObject.NodeType.PermanentModule;

        //Button State Logic
        SetInteractable(sellModuleButton, !isPermanentModule && !BuildManager.Instance.IsPreviewing);
        
        // UI
        buildModuleUI.SetActive(!isTurretBuilt && isValidModuleBuildLocation && !isValidTurretBuildLocation && !BuildManager.Instance.IsPreviewing);
        buildTurretUI.SetActive(!isTurretBuilt && isValidTurretBuildLocation && !isValidModuleBuildLocation && !BuildManager.Instance.IsPreviewing);
        turretInfoUI.SetActive(isTurretBuilt && !BuildManager.Instance.IsPreviewing);
        confirmBuildUI.SetActive(BuildManager.Instance.IsPreviewing);

        if (!isTurretBuilt) {
            buildModuleButton.Select();
        }
    }

    private void GridSelection_OnDeselectGridCell(object sender, EventArgs e) {
        HideAllUI();
    }

    private void HideAllUI() {
        confirmBuildUI.SetActive(false);
        turretTargetingUI.SetActive(false);
        buildModuleUI.SetActive(false);
        buildTurretUI.SetActive(false);
        turretInfoUI.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}