using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour {
    
    public static InfoUI Instance { get; private set; }
    
    [SerializeField]
    [SceneObjectsOnly]
    [Required]
    [Header("Info Panel Container")]
    private GameObject infoUI;

    [SerializeField] private Button infoCloseButton;
    [SerializeField] private TextMeshProUGUI infoTitleText;
    [SerializeField] private TextMeshProUGUI infoDescriptionText;

    private string defaultTitleString;
    private string defaultDescriptionString;
    
    private void Awake() {
        Instance = this;
        
        infoCloseButton.onClick.AddListener(Hide);
        
        Hide();
    }

    private void Start() {
        defaultTitleString = infoTitleText.text;
        defaultDescriptionString = infoDescriptionText.text;
        GridSelection.Instance.OnSelectGridCell += GridSelection_OnSelectGridCell;
        GridSelection.Instance.OnDeselectGridCell += GridSelection_OnDeselectGridCell;
    }

    private void GridSelection_OnDeselectGridCell(object sender, EventArgs e) {
        SetInfo(defaultTitleString, defaultDescriptionString);
    }

    private void GridSelection_OnSelectGridCell(object sender, GridSelection.OnSelectGridCellEventArgs e) {
        GridMapObject selectedGridObject = GridManager.Instance.TryGetMainGrid().GetGridObject(e.x, e.y);
        if (selectedGridObject == null) {
            Debug.LogWarning("GridObject is null");
            return;
        }
        
        // Try getting the selected grid object's built turret
        if (selectedGridObject.TryGetBuiltTurret(out Turret turret)) {
            float dps = turret.CalculateDPS();
            string description = $"{turret.GetTurretSO().turretDescription}\n\n" +
                $"Damage: {turret.Damage}\n" +
                $"Range: {turret.Range}\n" +
                $"Fire Rate: {turret.FireRate}\n" +
                $"DPS: {dps:F2}\n";
            SetInfo(turret.GetTurretSO().turretName, description);
        } else {
            // Try getting the grid object's node type if turret doesn't exist
            switch (selectedGridObject.GetNodeType()) {
                case GridMapObject.NodeType.Core:
                    // Display Info about the core
                    SetInfo("Core", "Protect the core at all costs!");
                    break;
                case GridMapObject.NodeType.Vortex:
                    // Display Info about the vortex
                    SetInfo("Vortex", "This is where the enemies are.");
                    break;
                case GridMapObject.NodeType.BuiltModule:
                    // Display Info about the built module
                    SetInfo("Built Module", "A removable module used to delay the enemies. Place these strategically!");
                    break;
                case GridMapObject.NodeType.PermanentModule:
                    // Display Info about the permanent module
                    SetInfo("Permanent Module", "A module that cannot be removed.");
                    break;
                default:
                    SetInfo(defaultTitleString, defaultDescriptionString);
                    break;
            }
        }
    }

    public void SetInfo(string title, string description) {
        infoTitleText.text = title;
        infoDescriptionText.text = description;
    }

    public void Toggle() {
        if (infoUI.activeSelf) {
            Hide();
        } else {
            Show();
        }
    }
    
    public void Show() {
        infoUI.SetActive(true);
    }
    
    public void Hide() {
        infoUI.SetActive(false);
    }
}
