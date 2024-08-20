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

    [InfoBox("Turret Buttons should be set in the inspector! (For the respective TurretSO Object)")] [Required] [SceneObjectsOnly] [Header("Turret Info")] [SerializeField]
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

    [Required] [SceneObjectsOnly] [Header("Turret Upgrade UI")] [SerializeField]
    private GameObject turretUpgradeUI;

    [SerializeField] private GameObject turretUpgradeInfoUI;
    [SerializeField] private TextMeshProUGUI turretUpgradeInfoTitleText;
    [SerializeField] private TextMeshProUGUI turretUpgradeInfoDescriptionText;
    [SerializeField] private GameObject upgradeButtonContainer;
    [SerializeField] private Button upgradeButtonTemplate;
    [SerializeField] private Button confirmUpgradeButton;
    [SerializeField] private Button cancelUpgradeButton;

    private GridMapObject lastSelectedGridObject;
    private Turret lastSelectedTurret;

    private void Awake() {
        buildModuleButton.onClick.AddListener((() => { BuildManager.Instance.BuildModule(); }));

        turretInfoButton.onClick.AddListener((() => {
            ShowTurretInfo();
            InfoUI.Instance.Toggle();
        }));

        turretSellButton.onClick.AddListener((() => { BuildManager.Instance.SellTurret(); }));

        sellModuleButton.onClick.AddListener((() => { BuildManager.Instance.RemoveModule(); }));

        // Turret Targeting

        turretTargetingButton.onClick.AddListener((() => {
            // Toggle targeting menu
            if (turretTargetingUI.activeSelf) {
                turretTargetingUI.gameObject.SetActive(false);
            } else {
                turretTargetingUI.gameObject.SetActive(true);
                turretUpgradeUI.SetActive(false);
            }
        }));

        firstEnteredButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.FirstEntered); }));

        lastEnteredButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.LastEntered); }));

        closestButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.Closest); }));

        furthestButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.Furthest); }));

        highestHPButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.HighestHealth); }));

        lowestHPButton.onClick.AddListener((() => { SetTurretTargetingText(TurretTargetSelection.TargetingPreference.LowestHealth); }));

        // Turret Upgrade

        turretUpgradeButton.onClick.AddListener((() => {
            if (turretUpgradeUI.activeSelf) {
                turretUpgradeUI.gameObject.SetActive(false);
            } else {
                turretUpgradeUI.gameObject.SetActive(true);
            }
        }));

        confirmUpgradeButton.onClick.AddListener((() => { }));

        cancelUpgradeButton.onClick.AddListener((() => { }));

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
        if (lastSelectedTurret != null) {
            UpdateUpgradeButtons(lastSelectedTurret.GetTurretSO());
        }
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

        if (lastSelectedGridObject != selectedGridObject) {
            HideAllUI();
        }

        lastSelectedGridObject = selectedGridObject;
        lastSelectedTurret = selectedGridObject.GetBuiltTurret();

        if (lastSelectedTurret) {
            UpdateUpgradeButtons(lastSelectedTurret.GetTurretSO());
        }

        //Update the current turret's targeting mode string
        turretTargetingUI.SetActive(false);
        turretUpgradeInfoUI.SetActive(false);
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

    private void UpdateUpgradeButtons(TurretSO turretSO) {
        ClearUpgradeButtons(); // Clear existing buttons before populating new ones

        foreach (BaseTurretUpgradeSO baseTurretUpgradeSO in turretSO.turretUpgradeListSO.possibleUpgradesListSO) {
            // Create the button and set its properties
            Button button = Instantiate(upgradeButtonTemplate, upgradeButtonContainer.transform).GetComponent<Button>();
            button.image.sprite = baseTurretUpgradeSO.upgradeSprite;
            button.GetComponentInChildren<TextMeshProUGUI>().text = baseTurretUpgradeSO.upgradeName + " (" + baseTurretUpgradeSO.creditsCost + ")";

            bool canAfford = GameManager.Instance.CanAfford(baseTurretUpgradeSO.creditsCost);
            button.interactable = canAfford; // Set button interactability based on player's credits

            // Clear any existing listeners on the confirm and cancel buttons
            confirmUpgradeButton.onClick.RemoveAllListeners();
            cancelUpgradeButton.onClick.RemoveAllListeners();

            // Add listener to the upgrade button
            button.onClick.AddListener(() => {
                if (turretUpgradeInfoUI.activeSelf) {
                    turretUpgradeInfoUI.SetActive(false);
                } else {
                    turretUpgradeInfoTitleText.text = "Upgrade " + baseTurretUpgradeSO.upgradeName + "?";
                    turretUpgradeInfoDescriptionText.text = baseTurretUpgradeSO.upgradeDescription;
                    turretUpgradeInfoUI.SetActive(true);

                    // Assign the correct listener for the confirm button for this upgrade
                    confirmUpgradeButton.onClick.RemoveAllListeners();
                    confirmUpgradeButton.onClick.AddListener(() => {
                        BuildManager.Instance.UpgradeTurret(lastSelectedTurret, baseTurretUpgradeSO);
                        turretUpgradeInfoUI.SetActive(false); // Hide upgrade info after confirming
                    });
                }
            });

            cancelUpgradeButton.onClick.AddListener(() => { turretUpgradeInfoUI.SetActive(false); });

            button.gameObject.SetActive(true);
        }
    }


    private void ClearUpgradeButtons() {
        foreach (Transform child in upgradeButtonContainer.transform) {
            if (child == upgradeButtonTemplate.transform)
                continue;
            Destroy(child.gameObject);
        }
    }

    private void GridSelection_OnDeselectGridCell(object sender, EventArgs e) {
        HideAllUI();
    }

    private void HideAllUI() {
        turretUpgradeInfoUI.SetActive(false);
        turretUpgradeUI.SetActive(false);
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