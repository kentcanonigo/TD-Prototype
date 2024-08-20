using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class BuildManager : MonoBehaviour {
    public static BuildManager Instance { get; private set; }

    [SerializeField] [Required] [SceneObjectsOnly]
    private Transform turretParent;

    private GridMapObject lastSelectedGridObject;
    private Turret previewTurret;
    public bool IsPreviewing { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GridSelection.Instance.OnSelectGridCell += GridSelection_OnSelectGridCell;
        GridSelection.Instance.OnDeselectGridCell += GridSelection_OnDeselectGridCell;
    }

    private void GridSelection_OnDeselectGridCell(object sender, EventArgs e) {
        if (lastSelectedGridObject?.TryGetBuiltTurret(out Turret turret) == true) {
            HideTurretRange(turret);
        }

        previewTurret = null;
        lastSelectedGridObject = null;
    }

    private void GridSelection_OnSelectGridCell(object sender, GridSelection.OnSelectGridCellEventArgs e) {
        var selectedGridObject = GridManager.Instance.TryGetMainGrid().GetGridObject(e.x, e.y);
        if (selectedGridObject == null) {
            Debug.LogError("Selected grid object is null");
            return;
        }

        if (lastSelectedGridObject != null && selectedGridObject != lastSelectedGridObject) {
            if (lastSelectedGridObject.TryGetBuiltTurret(out Turret lastTurret)) {
                HideTurretRange(lastTurret);
            }
        }

        if (previewTurret != null) {
            UpdateTurretPreviewPosition(selectedGridObject);
        }

        if (selectedGridObject.TryGetBuiltTurret(out Turret turret)) {
            ShowTurretRange(turret);
        }

        lastSelectedGridObject = selectedGridObject;
    }

    private static void ShowTurretRange(Turret turret) {
        if (turret.TryGetRangeVisual(out TurretRangeVisual turretRangeVisual)) {
            turretRangeVisual.ShowRange(turret.Range);
        } else {
            Debug.LogWarning($"Couldn't display turret range for {turret}");
        }
    }

    private static void HideTurretRange(Turret turret) {
        if (turret.TryGetRangeVisual(out TurretRangeVisual turretRangeVisual)) {
            turretRangeVisual.HideRange();
        } else {
            Debug.LogWarning($"Couldn't hide turret range for {turret}");
        }
    }

    public void BuildModule() {
        if (lastSelectedGridObject == null) {
            Debug.LogWarning("No grid object selected.");
            return;
        }

        if (GameManager.Instance.CurrentGameState != GameManager.GameState.BuildPhase) {
            Debug.LogWarning("Can only build in build phase");
            return;
        }

        if (lastSelectedGridObject.GetNodeType() == GridMapObject.NodeType.BuiltModule) {
            ConfirmRemoveModule();
            GameManager.Instance.AddModuleCount(1);
        } else if (lastSelectedGridObject.IsBuildable && GameManager.Instance.CurrentModules > 0) {
            if (GridManager.Instance.TryUpdatePathForVortexList(lastSelectedGridObject)) {
                ConfirmBuildModule(lastSelectedGridObject);
                GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
                GameManager.Instance.DecrementModuleCount();
            } else {
                Debug.LogWarning("Building here would block the path to the core!");
            }
        } else if (lastSelectedGridObject.GetNodeType() == GridMapObject.NodeType.PermanentModule) {
            Debug.LogWarning("Cannot remove a permanent module.");
        } else {
            Debug.LogWarning("Cannot build module here.");
        }
    }
    
    private void ConfirmBuildModule(GridMapObject gridObject) {
        gridObject.SetNodeType(GridMapObject.NodeType.BuiltModule);
        GridManager.Instance.UpdatePathForVortexList();
    }

    public void RemoveModule() {
        if (lastSelectedGridObject == null) {
            Debug.LogWarning("No grid object selected.");
            return;
        }

        if (GameManager.Instance.CurrentGameState != GameManager.GameState.BuildPhase) {
            Debug.LogWarning("Can only sell in build phase");
            return;
        }

        if (lastSelectedGridObject.GetNodeType() == GridMapObject.NodeType.BuiltModule) {
            ConfirmRemoveModule();
            GameManager.Instance.AddModuleCount(1);
            GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
        } else {
            Debug.LogWarning("Cannot sell module here.");
        }
    }
    
    private void ConfirmRemoveModule() {
        lastSelectedGridObject.SetNodeType(GridMapObject.NodeType.None);
        GridManager.Instance.UpdatePathForVortexList();
    }

    private void InstantiateTurretPreview(TurretSO turretSO) {
        var turretPrefab = Instantiate(turretSO.turretPrefab, Vector3.zero, Quaternion.identity, turretParent);
        previewTurret = turretPrefab.GetComponent<Turret>();
        previewTurret.Initialize(turretSO);
        ShowTurretRange(previewTurret);
        previewTurret.DisableAllModules();
        IsPreviewing = true;
        GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
    }

    private void UpdateTurretPreviewPosition(GridMapObject gridObject) {
        if (previewTurret == null || gridObject == null) return;

        previewTurret.gameObject.SetActive(gridObject.GetNodeType() is GridMapObject.NodeType.BuiltModule or GridMapObject.NodeType.PermanentModule);
        previewTurret.transform.position = GridManager.Instance.GetWorldPositionWithOffset(gridObject);
    }

    public void PreviewBuildTurret(TurretSO turretSO) {
        if (previewTurret == null) {
            InstantiateTurretPreview(turretSO);
        }

        if (lastSelectedGridObject != null) {
            UpdateTurretPreviewPosition(lastSelectedGridObject);
        }
    }
    
    public void SellTurret() {
        if (lastSelectedGridObject == null || !lastSelectedGridObject.TryGetBuiltTurret(out Turret builtTurret)) {
            Debug.LogWarning("No turret in this position.");
            return;
        }

        GameManager.Instance.AddCredits(builtTurret.Cost);
        lastSelectedGridObject.SetBuiltTurret(null);
        HideTurretRange(builtTurret);
        Destroy(builtTurret.gameObject);
        GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
    }
    
    public void ConfirmBuild() {
        if (previewTurret == null || lastSelectedGridObject == null || !IsValidBuildLocation(lastSelectedGridObject)) return;
        
        GameManager.Instance.SubtractCredits(previewTurret.Cost);
        BuildTurretOnGridObject(lastSelectedGridObject, previewTurret);
        ResetPreviewState();
    }
    
    public void CancelBuild() {
        if (previewTurret == null) return;

        HideTurretRange(previewTurret);
        Destroy(previewTurret.gameObject);
        ResetPreviewState();
    }

    public void UpgradeTurret(Turret turret, BaseTurretUpgradeSO upgrade) {
        if (lastSelectedGridObject == null) {
            Debug.LogWarning("No turret in this position.");
            return;
        }
        
        if (GameManager.Instance.CanAfford(upgrade.creditsCost)) {
            if (turret.TryAddUpgrade(upgrade)) {
                GameManager.Instance.SubtractCredits(upgrade.creditsCost);
            }
        } else {
            Debug.LogWarning("Cannot afford upgrade.");
            return;
        }
        // Upgrade the current turret
        GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
    }

    private bool IsValidBuildLocation(GridMapObject gridObject) {
        return gridObject.GetNodeType() is GridMapObject.NodeType.BuiltModule or GridMapObject.NodeType.PermanentModule;
    }

    private void BuildTurretOnGridObject(GridMapObject gridObject, Turret turret) {
        gridObject.SetBuiltTurret(turret);
        turret.EnableAllModules();
        GridSelection.Instance.TriggerDeselectGridCell();
    }

    private void ResetPreviewState() {
        previewTurret = null;
        IsPreviewing = false;
        if (lastSelectedGridObject != null) {
            GridSelection.Instance.TriggerSelectGridCell(lastSelectedGridObject.x, lastSelectedGridObject.y);
        }
    }
}
