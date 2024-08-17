using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour {
    [Required]
    [SceneObjectsOnly]
    [Header("Module Building")]
    [SerializeField] private GameObject buildModuleUI;
    [SerializeField] private Button buildModuleButton;
    
    [Required]
    [Header("Turret Building")]
    [SerializeField] private GameObject buildTurretUI;
    [SerializeField] private Button sellModuleButton;
    
    
    [Required]
    [SceneObjectsOnly]
    [Header("Turret Info")]
    [SerializeField] private GameObject turretInfoUI;
    [SerializeField] private Button turretInfoButton;
    [SerializeField] private Button turretUpgradeButton;
    [SerializeField] private Button turretSellButton;

    private GridMapObject lastSelectedGridObject;

    private void Awake() {
        buildModuleButton.onClick.AddListener((() => {
            BuildManager.Instance.OnBuildModuleButtonClicked();
        }));
        
        turretInfoButton.onClick.AddListener((() => {
            
        }));
        
        turretUpgradeButton.onClick.AddListener((() => {
            
        }));
        
        turretSellButton.onClick.AddListener((() => {
            BuildManager.Instance.OnSellTurretButtonClicked();
        }));
        
        sellModuleButton.onClick.AddListener((() => {
            BuildManager.Instance.OnSellModuleButtonClicked();
        }));
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
        Show();

        bool isTurretBuilt = selectedGridObject.GetBuiltTurret();
        bool isValidModuleBuildLocation = selectedGridObject.GetNodeType() is GridMapObject.NodeType.None;
        bool isValidTurretBuildLocation = selectedGridObject.GetNodeType() is GridMapObject.NodeType.BuiltModule or GridMapObject.NodeType.PermanentModule;

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
        buildModuleUI.SetActive(false);
        buildTurretUI.SetActive(false);
        turretInfoUI.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
