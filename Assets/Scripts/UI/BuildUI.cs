using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour {
    [Header("Building")]
    [SerializeField] private CanvasGroup buildMenuCanvasGroup;
    [SerializeField] private Button buildModuleButton;
    
    [Header("Turret Info")]
    [SerializeField] private CanvasGroup turretInfoCanvasGroup;
    [SerializeField] private Button turretInfoButton;
    [SerializeField] private Button turretUpgradeButton;
    [SerializeField] private Button turretSellButton;

    private GridMapObject lastSelectedGridObject;

    private void Awake() {
        buildModuleButton.onClick.AddListener((() => {
            ModuleBuilder.Instance.OnBuildModuleButtonClicked();
        }));
        
        turretInfoButton.onClick.AddListener((() => {
            
        }));
        
        turretUpgradeButton.onClick.AddListener((() => {
            
        }));
        
        turretSellButton.onClick.AddListener((() => {
            
        }));
    }

    private void Start() {
        GridSelection.Instance.OnSelectGridCell += GridSelection_OnSelectGridCell;
        GridSelection.Instance.OnDeselectGridCell += GridSelection_OnDeselectGridCell;
        HideAllUI();
    }

    private void GridSelection_OnSelectGridCell(object sender, GridSelection.OnSelectGridCellEventArgs e) {
        GridMapObject selectedGridObject = GridManager.Instance.TryGetMainGrid().GetGridObject(e.x, e.y);
        Show();

        bool isTurretBuilt = selectedGridObject.GetBuiltTurret();
        bool isValidBuildLocation = selectedGridObject.GetNodeType() is GridMapObject.NodeType.BuiltModule or GridMapObject.NodeType.PermanentModule or GridMapObject.NodeType.None;

        buildMenuCanvasGroup.alpha = !isTurretBuilt || isValidBuildLocation ? 1f : 0f;
        turretInfoCanvasGroup.alpha = isTurretBuilt ? 1f : 0f;
        
        if (!isTurretBuilt) {
            buildModuleButton.Select();
        }
    }

    private void GridSelection_OnDeselectGridCell(object sender, EventArgs e) {
        HideAllUI();
    }
    
    private void HideAllUI() {
        buildMenuCanvasGroup.alpha = 0f;
        turretInfoCanvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
