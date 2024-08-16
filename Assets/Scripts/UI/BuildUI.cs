using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour {
    [Header("Building")]
    [SerializeField] private CanvasGroup buildMenuCanvasGroup;
    [SerializeField] private Button buildModuleButton;
    [SerializeField] private Button buildBlasterButton;
    
    [Header("Turret Info")]
    [SerializeField] private CanvasGroup turretInfoCanvasGroup;
    [SerializeField] private Button turretInfoButton;
    [SerializeField] private Button turretUpgradeButton;
    [SerializeField] private Button turretSellButton;

    private void Awake() {
        buildModuleButton.onClick.AddListener((() => {
            ModuleBuilder.Instance.OnBuildModuleButtonClicked();
        }));
        
        buildBlasterButton.onClick.AddListener((() => {
            
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
        Hide();
    }

    private void GridSelection_OnDeselectGridCell(object sender, EventArgs e) {
        Hide();
    }

    private void GridSelection_OnSelectGridCell(object sender, GridSelection.OnSelectGridCellEventArgs e) {
        buildModuleButton.Select();
        Show();
    }
    
    private void Show() {
        gameObject.SetActive(true);
    }
    
    private void Hide() {
        gameObject.SetActive(false);
    }
}
