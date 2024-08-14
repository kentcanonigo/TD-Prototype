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
    }

    public void OnBuildModuleButtonClicked() {
        if (selectedGridObject != null) {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.BuildPhase) {
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
