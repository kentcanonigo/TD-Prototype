using UnityEngine;

public class ModuleBuilder : MonoBehaviour {
    public static ModuleBuilder Instance { get; private set; }

    private GridMapObject selectedGridObject;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GridSelection.Instance.OnSelectGridCell += GridSelection_OnSelectGridCell;
    }

    private void GridSelection_OnSelectGridCell(object sender, GridSelection.OnSelectGridCellEventArgs e) {
        selectedGridObject = GridManager.Instance.TryGetMainGrid().GetGridObject(e.x, e.y);
        switch (selectedGridObject.GetNodeType()) {
            case GridMapObject.NodeType.None:
                break;
            case GridMapObject.NodeType.BuiltModule:
                break;
            case GridMapObject.NodeType.PermanentModule:
                break;
            case GridMapObject.NodeType.Core:
                break;
            case GridMapObject.NodeType.Vortex:
                break;
        }
    }

    public void OnBuildModuleButtonClicked() {
        if (selectedGridObject != null) {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.BuildPhase) {
                // In build phase
                if (selectedGridObject.GetNodeType() == GridMapObject.NodeType.BuiltModule) {
                    // If the selected object is a BuiltModule, remove it
                    RemoveModule();
                    GameManager.Instance.AddModuleCount(1); // Add the module back to the GameManager
                } else if (selectedGridObject.IsBuildable) {
                    // If the selected object is buildable
                    if (GameManager.Instance.CurrentModules > 0) {
                        // Player has enough modules to build
                        if (GridManager.Instance.TryUpdatePathForVortexList(selectedGridObject)) {
                            // Building does not block the path
                            BuildModule(selectedGridObject);
                            GameManager.Instance.DecrementModuleCount(); // Decrement the module count after building
                        } else {
                            Debug.LogWarning("Building here would block the path to the core!");
                        }
                    } else {
                        Debug.LogWarning("Not enough modules to build.");
                    }
                } else if (selectedGridObject.GetNodeType() == GridMapObject.NodeType.PermanentModule) {
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

    public void OnBuildTurretButtonClicked(TurretSO turretSO) {
        if (selectedGridObject != null) {
            if (selectedGridObject.GetNodeType() is GridMapObject.NodeType.BuiltModule or GridMapObject.NodeType.PermanentModule) {
                // TODO: Check if player has enough money
                // If the selected object is a BuiltModule or PermanentModule
                // Instantiate the turret prefab
                GameObject turretPrefab = Instantiate(turretSO.turretPrefab, GridManager.Instance.GetWorldPosition(selectedGridObject), Quaternion.identity);
                Turret turret = turretPrefab.GetComponent<Turret>();

                // Link the turret to the grid system
                selectedGridObject.SetBuiltTurret(turret);

                // Additional setup (e.g., positioning, initialization)
                turret.Initialize(turretSO);
            } else {
                Debug.LogWarning("Cannot build a turret here.");
            }
        } else {
            Debug.LogWarning("No grid object selected.");
        }
    }

    private void RemoveModule() {
        selectedGridObject.SetNodeType(GridMapObject.NodeType.None);
        GridManager.Instance.UpdatePathForVortexList(); //Update the vortex paths
        //Debug.Log("Module removed and added back to the inventory.");
    }

    private void BuildModule(GridMapObject gridObject) {
        gridObject.SetNodeType(GridMapObject.NodeType.BuiltModule);
        GridManager.Instance.UpdatePathForVortexList(); //Update the vortex paths
        Debug.Log("Module built!");
    }
}