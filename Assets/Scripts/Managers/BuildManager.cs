using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public class BuildManager : MonoBehaviour {
    public static BuildManager Instance { get; private set; }

    [SerializeField] [Required] [SceneObjectsOnly]
    private Transform turretParent;

    private GridMapObject lastSelectedGridObject;
    private Turret previewTurret;
    private TurretSO currentTurretSO;
    public bool IsPreviewing { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GridSelection.Instance.OnSelectGridCell += GridSelection_OnSelectGridCell;
        GridSelection.Instance.OnDeselectGridCell += GridSelection_OnDeselectGridCell;
    }

    private void GridSelection_OnDeselectGridCell(object sender, EventArgs e) {
        if (lastSelectedGridObject.TryGetBuiltTurret(out Turret turret)) {
            HideTurretRange(turret);
        }
    }

    private void GridSelection_OnSelectGridCell(object sender, GridSelection.OnSelectGridCellEventArgs e) {
        GridMapObject selectedGridObject = GridManager.Instance.TryGetMainGrid().GetGridObject(e.x, e.y);
        
        if (selectedGridObject != null) {

            if (lastSelectedGridObject != null) {
                if (selectedGridObject != lastSelectedGridObject) {
                    if (lastSelectedGridObject.TryGetBuiltTurret(out Turret lastTurret)) {
                        HideTurretRange(lastTurret);
                    }
                }
            }
            
            if (previewTurret) {
                UpdateTurretPreviewPosition(selectedGridObject);
            }
        
            //Show turret range
            if (selectedGridObject.TryGetBuiltTurret(out Turret turret)) {
                ShowTurretRange(turret);
            }
            
            lastSelectedGridObject = selectedGridObject;
        } else {
            Debug.LogError("Last selected grid object is null");
            return;
        }
    }

    private static void ShowTurretRange(Turret turret) {
        if (turret.TryGetRangeVisual(out TurretRangeVisual turretRangeVisual)) {
            turretRangeVisual.ShowRange(turret.BaseRange);
            //Debug.Log($"Showing range for {turret}");
        } else {
            Debug.LogWarning($"Couldn't display turret range for {turret}");
        }
    }
    
    private static void HideTurretRange(Turret turret) {
        if (turret.TryGetRangeVisual(out TurretRangeVisual turretRangeVisual)) {
            turretRangeVisual.HideRange();
            //Debug.Log($"Hiding range for {turret}");
        } else {
            Debug.LogWarning($"Couldn't hide turret range for {turret}");
        }
    }

    public void OnBuildModuleButtonClicked() {
        if (lastSelectedGridObject != null) {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.BuildPhase) {
                // In build phase
                if (lastSelectedGridObject.GetNodeType() == GridMapObject.NodeType.BuiltModule) {
                    // If the selected object is a BuiltModule, remove it
                    RemoveModule();
                    GameManager.Instance.AddModuleCount(1); // Add the module back to the GameManager
                } else if (lastSelectedGridObject.IsBuildable) {
                    // If the selected object is buildable
                    if (GameManager.Instance.CurrentModules > 0) {
                        // Player has enough modules to build
                        if (GridManager.Instance.TryUpdatePathForVortexList(lastSelectedGridObject)) {
                            // Building does not block the path
                            BuildModule(lastSelectedGridObject);
                            GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
                            GameManager.Instance.DecrementModuleCount(); // Decrement the module count after building
                        } else {
                            Debug.LogWarning("Building here would block the path to the core!");
                        }
                    } else {
                        Debug.LogWarning("Not enough modules to build.");
                    }
                } else if (lastSelectedGridObject.GetNodeType() == GridMapObject.NodeType.PermanentModule) {
                    Debug.LogWarning("Cannot remove a permanent module.");
                } else {
                    Debug.LogWarning("Cannot build module here.");
                }
            } else {
                Debug.LogWarning("Can only build in build phase");
            }
        } else {
            Debug.LogWarning("No grid object selected.");
        }
    }

    public void OnSellModuleButtonClicked() {
        if (lastSelectedGridObject != null) {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.BuildPhase) {
                if (lastSelectedGridObject.GetNodeType() == GridMapObject.NodeType.BuiltModule) {
                    RemoveModule();
                
                    GameManager.Instance.AddModuleCount(1); // Add the module back to the GameManager
                
                    GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
                } else {
                    Debug.LogWarning("Cannot sell module here.");
                }
            } else {
                Debug.LogWarning("Can only sell in build phase");
            }
        } else {
            Debug.LogWarning("No grid object selected.");
        }
    }

    public void OnBuildTurretButtonClicked(TurretSO turretSO) {
        currentTurretSO = turretSO;
        if (previewTurret == null) {
            // Instantiate the turret preview
            GameObject turretPrefab = Instantiate(turretSO.turretPrefab, Vector3.zero, Quaternion.identity, turretParent);
            previewTurret = turretPrefab.GetComponent<Turret>();
            previewTurret.Initialize(turretSO);
            ShowTurretRange(previewTurret);
            previewTurret.DisableAllModules();
            IsPreviewing = true;
            GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
        }
        if (lastSelectedGridObject != null) {
            UpdateTurretPreviewPosition(lastSelectedGridObject);
        }
    }
    
    private void UpdateTurretPreviewPosition(GridMapObject gridObject) {
        if (previewTurret && gridObject != null) {
            switch (gridObject.GetNodeType()) {
                case GridMapObject.NodeType.BuiltModule:
                case GridMapObject.NodeType.PermanentModule:
                    previewTurret.gameObject.SetActive(true);
                    break;
                case GridMapObject.NodeType.None:
                case GridMapObject.NodeType.Core:
                case GridMapObject.NodeType.Vortex:
                    previewTurret.gameObject.SetActive(false);
                    break;
            }
            previewTurret.transform.position = GridManager.Instance.GetWorldPositionWithOffset(gridObject);
        }
    }

    public void OnSellTurretButtonClicked() {
        if (lastSelectedGridObject != null) {
            if (lastSelectedGridObject.TryGetBuiltTurret(out Turret builtTurret)) {
                GameManager.Instance.AddCredits(builtTurret.BaseCost);
                
                lastSelectedGridObject.SetBuiltTurret(null);
                
                HideTurretRange(builtTurret);
                
                Destroy(builtTurret.gameObject);
                
                GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
                
            } else {
                Debug.LogWarning("No turret in this position.");
            }
        } else {
            Debug.LogWarning("No grid object selected.");
        }
    }

    private void RemoveModule() {
        lastSelectedGridObject.SetNodeType(GridMapObject.NodeType.None);
        GridManager.Instance.UpdatePathForVortexList(); //Update the vortex paths
        //Debug.Log("Module removed and added back to the inventory.");
    }

    private void BuildModule(GridMapObject gridObject) {
        gridObject.SetNodeType(GridMapObject.NodeType.BuiltModule);
        GridManager.Instance.UpdatePathForVortexList(); //Update the vortex paths
        //Debug.Log("Module built!");
    }

    public void OnConfirmBuildButtonClicked() {
        if (previewTurret != null && lastSelectedGridObject != null) {
            if (lastSelectedGridObject.GetNodeType() is GridMapObject.NodeType.BuiltModule or GridMapObject.NodeType.PermanentModule) {
                lastSelectedGridObject.SetBuiltTurret(previewTurret);
                previewTurret.EnableAllModules();
                previewTurret = null; // Clear the preview turret
                IsPreviewing = false;
            } else {
                Debug.LogWarning("Cannot build a turret here.");
            }
            GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
        }
    }

    public void OnCancelBuildButtonClicked() {
        if (previewTurret != null) {
            HideTurretRange(previewTurret);
            Destroy(previewTurret.gameObject);
            previewTurret = null;
            currentTurretSO = null;
            IsPreviewing = false;
        }
        GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
    }
}