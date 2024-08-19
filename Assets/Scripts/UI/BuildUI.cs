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

    [SerializeField] private Button sellModuleButton;

    [Required] [SceneObjectsOnly] [Header("Turret Info")] [SerializeField]
    private GameObject turretInfoUI;

    [SerializeField] private Button turretInfoButton;
    [SerializeField] private Button turretUpgradeButton;
    [SerializeField] private Button turretSellButton;
    [SerializeField] private Button turretTargetingButton;

    [Required] [SceneObjectsOnly] [Header("Turret Info")] [SerializeField]
    private GameObject turretTargetingUI;

    [SerializeField] private TextMeshProUGUI currentTargetingText;
    [SerializeField] private Button firstEnteredButton;
    [SerializeField] private Button lastEnteredButton;
    [SerializeField] private Button closestButton;
    [SerializeField] private Button furthestButton;
    [SerializeField] private Button highestHPButton;
    [SerializeField] private Button lowestHPButton;

    private GridMapObject lastSelectedGridObject;
    private Turret lastSelectedTurret;

    private void Awake() {
        buildModuleButton.onClick.AddListener((() => { BuildManager.Instance.OnBuildModuleButtonClicked(); }));

        turretInfoButton.onClick.AddListener((() => {
            InfoUI.Instance.SetInfo(lastSelectedGridObject.GetBuiltTurret().GetTurretSO().turretName, lastSelectedGridObject.GetBuiltTurret().GetTurretSO().turretDescription);
            InfoUI.Instance.Toggle();
        }));

        turretUpgradeButton.onClick.AddListener((() => { }));

        turretSellButton.onClick.AddListener((() => { BuildManager.Instance.OnSellTurretButtonClicked(); }));

        sellModuleButton.onClick.AddListener((() => { BuildManager.Instance.OnSellModuleButtonClicked(); }));

        // Turret Targeting

        turretTargetingButton.onClick.AddListener((() => {
            // Toggle targeting menu
            if (turretTargetingUI.activeSelf) {
                turretTargetingUI.gameObject.SetActive(false);
            } else {
                turretTargetingUI.gameObject.SetActive(true);
            }
        }));

        firstEnteredButton.onClick.AddListener((() => { SetTurretTargeting(TurretTargetSelection.TargetingPreference.FirstEntered); }));

        lastEnteredButton.onClick.AddListener((() => { SetTurretTargeting(TurretTargetSelection.TargetingPreference.LastEntered); }));

        closestButton.onClick.AddListener((() => { SetTurretTargeting(TurretTargetSelection.TargetingPreference.Closest); }));

        furthestButton.onClick.AddListener((() => { SetTurretTargeting(TurretTargetSelection.TargetingPreference.Furthest); }));

        highestHPButton.onClick.AddListener((() => { SetTurretTargeting(TurretTargetSelection.TargetingPreference.HighestHealth); }));

        lowestHPButton.onClick.AddListener((() => { SetTurretTargeting(TurretTargetSelection.TargetingPreference.LowestHealth); }));
    }

    private void SetTurretTargeting(TurretTargetSelection.TargetingPreference newTargetingPreference) {
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
        HideAllUI();
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
        
        Show();

        bool isTurretBuilt = selectedGridObject.GetBuiltTurret();
        bool isValidModuleBuildLocation = selectedGridObject.GetNodeType() is GridMapObject.NodeType.None;
        bool isValidTurretBuildLocation = selectedGridObject.GetNodeType() is GridMapObject.NodeType.BuiltModule or GridMapObject.NodeType.PermanentModule;
        bool isPermanentModule = selectedGridObject.GetNodeType() is GridMapObject.NodeType.PermanentModule;

        sellModuleButton.gameObject.SetActive(!isPermanentModule);
        buildModuleUI.SetActive(!isTurretBuilt && isValidModuleBuildLocation && !isValidTurretBuildLocation);
        buildTurretUI.SetActive(!isTurretBuilt && isValidTurretBuildLocation && !isValidModuleBuildLocation);
        turretInfoUI.SetActive(isTurretBuilt);

        if (!isTurretBuilt) {
            buildModuleButton.Select();
        }
    }

    private void GridSelection_OnDeselectGridCell(object sender, EventArgs e) {
        HideAllUI();
    }

    private void HideAllUI() {
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