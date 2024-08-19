using System;
using System.Collections.Generic;
using CodeMonkey;
using CodeMonkey.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GridManager : MonoBehaviour {
    public static GridManager Instance { get; private set; }

    private int gridWidth = 1;
    private int gridHeight = 1;
    private float cellSize = 1f;
    [Required] [SerializeField] private GridMapVisual gridMapVisual;

    [Required] [AssetsOnly] [SerializeField]
    public GameObject corePrefab;

    [Required] [AssetsOnly] [SerializeField]
    public GameObject vortexPrefab;

    [AssetsOnly] [Required]
    [SerializeField] private Material pathMaterial;

    [SerializeField] private bool showDebugOnPlay;

    [ShowIf("showDebugOnPlay")] [EnableIf("showDebugOnPlay")] [SerializeField]
    private bool showGridIsBuildableGizmos;

    [ShowIf("showDebugOnPlay")] [EnableIf("showDebugOnPlay")] [SerializeField]
    private bool showCurrentTurretGizmos;
    private Grid<GridMapObject> mainGameGrid; // For camera stuff
    private GridMapObject core; // The core
    private List<GridMapObject> vortexes; // A list of all the vortexes in the level
    private Dictionary<GridMapObject, List<GridMapObject>> vortexPaths; // Store paths for each vortex
    private Dictionary<GridMapObject, LineRenderer> vortexLineRenderers; // Store LineRenderers for each vortex
    private GridMap gridMap; // THE gridmap

    private Vector3 CellOffset { get; set; }

    //Gizmos

    private void OnDrawGizmos() {
        Gizmos.color = Color.white; // Set the color for the grid lines

        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
#if UNITY_EDITOR
                if (showGridIsBuildableGizmos) {
                    Handles.Label(GetWorldPosition(x, y) + new Vector3(cellSize / 2 - .4f, cellSize / 2, 0), $"{gridMap?.GetGrid().GetGridObject(x, y)}");
                }

                if (showCurrentTurretGizmos) {
                    Handles.Label(GetWorldPosition(x, y) + new Vector3(cellSize / 2 - .4f, cellSize / 2, 0), $"{gridMap?.GetGrid().GetGridObject(x, y).GetBuiltTurret()}");
                }
#endif
            }
        }
    }

    public Vector3 GetWorldPosition(GridMapObject gridMapObject) {
        return GetWorldPosition(gridMapObject.x, gridMapObject.y);
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y) * cellSize + Vector3.zero;
    }

    public Vector3 GetWorldPositionWithOffset(GridMapObject gridMapObject) {
        return GetWorldPositionWithOffset(gridMapObject.x, gridMapObject.y);
    }

    public Vector3 GetWorldPositionWithOffset(int x, int y) {
        return new Vector3(x, y) * cellSize + CellOffset;
    }

    public Grid<GridMapObject> TryGetMainGrid() {
        if (mainGameGrid != null) {
            return mainGameGrid;
        }

        Debug.LogError("Main Game Grid not initialized!");
        return null;
    }

    // Initialization

    private void Awake() {
        Instance = this;
        vortexLineRenderers = new Dictionary<GridMapObject, LineRenderer>();
    }

    public void InitializeGrid(int width, int height) {
        gridWidth = width;
        gridHeight = height;

        // Initialize the grid
        gridMap = new GridMap(gridWidth, gridHeight, cellSize, showDebugOnPlay);
        CellOffset = new Vector3(cellSize / 2f, cellSize / 2f, 0f);
        mainGameGrid = gridMap.GetGrid();
        gridMap.SetGridMapVisual(gridMapVisual);
    }

    private GridMapObject.NodeType currentNodeType = GridMapObject.NodeType.PermanentModule;

    private void Update() {
        if (vortexes != null) {
            foreach (GridMapObject vortex in vortexes) {
                if (vortexPaths.TryGetValue(vortex, out List<GridMapObject> path)) {
                    for (int i = 0; i < path.Count - 1; i++) {
                        // Draw a line from the current node to the next node in the path
                        Vector3 start = GetWorldPosition(path[i].x, path[i].y);
                        Vector3 end = GetWorldPosition(path[i + 1].x, path[i + 1].y);
                        Debug.DrawLine(start * cellSize + CellOffset, end * cellSize + CellOffset, Color.red); // Change color as needed
                        UpdateLineRenderer(vortex, path);
                    }
                }
            }
        }

        if (GameManager.Instance.mapEditMode) {
            if (Input.GetMouseButtonDown(1)) {
                Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
                gridMap.GetGrid().GetXY(mousePosition, out int x, out int y);
                GridMapObject node = gridMap.GetNode(x, y);
                if (node != null) {
                    if (node.GetBuiltTurret()) {
                        Turret builtTurret = node.GetBuiltTurret();
                        node.SetBuiltTurret(null);
                        Destroy(builtTurret.gameObject);
                    } else if (node.GetNodeType() == GridMapObject.NodeType.PermanentModule) { // Check if the node is permanent (assuming you have a method to check that)
                        // Set the node type to None if it's permanent
                        node.SetNodeType(GridMapObject.NodeType.None); // Change NodeType.None to the appropriate value for no node type
                    } else {
                        // Otherwise, set the node to not walkable and change its type
                        gridMap.SetNodeType(mousePosition, currentNodeType);
                    }

                    UpdatePathForVortexList();
                }
            }

            if (Input.GetKeyDown(KeyCode.T)) {
                currentNodeType = GridMapObject.NodeType.PermanentModule;
                CMDebug.TextPopupMouse(currentNodeType.ToString());
            }

            if (Input.GetKeyDown(KeyCode.Y)) {
                currentNodeType = GridMapObject.NodeType.BuiltModule;
                CMDebug.TextPopupMouse(currentNodeType.ToString());
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                gridMap.Save();
                CMDebug.TextPopupMouse("SAVED!");
            }
        }
    }
    
    void InitializeLineRenderer(LineRenderer lineRenderer) {
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.material = pathMaterial;
        lineRenderer.sortingOrder = 10;
    }
    
    void UpdateLineRenderer(GridMapObject vortexNode, List<GridMapObject> path) {
        if (vortexLineRenderers.TryGetValue(vortexNode, out LineRenderer lineRenderer)) {
            lineRenderer.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++) {
                lineRenderer.SetPosition(i, GetWorldPositionWithOffset(path[i]));
            }
        }
    }

    // Level Initialization (core and Vortex Logic)

    public void InitializeLevel(LevelDataSO levelDataSO) {
        // Load the saved map data if it exists
        if (levelDataSO.levelFile) {
            gridMap.Load(levelDataSO);
        }

        vortexes = new List<GridMapObject>();
        vortexPaths = new Dictionary<GridMapObject, List<GridMapObject>>();

        // Initialize core
        core = gridMap.GetGrid().GetGridObject(levelDataSO.corePosition.x, levelDataSO.corePosition.y);
        core.SetNodeType(GridMapObject.NodeType.Core);

        // Instantiate the core GameObject
        // Instantiate(corePrefab, GetWorldPosition(core.x, core.y), Quaternion.identity);
        GameObject coreGameObject = Instantiate(corePrefab, GetWorldPositionWithOffset(core.x, core.y), Quaternion.identity, transform);

        // Initialize vortexes
        foreach (Vector2Int vortexPosition in levelDataSO.vortexPositions) {
            GridMapObject vortexNode = gridMap.GetGrid().GetGridObject(vortexPosition.x, vortexPosition.y);
            if (vortexNode == null) {
                Debug.LogError("Vortex Node is null!");
                continue; // Skip this iteration if the node is null
            }

            vortexNode.SetNodeType(GridMapObject.NodeType.Vortex);
            vortexes.Add(vortexNode);

            // Instantiate the vortex GameObject
            // Instantiate(vortexPrefab, GetWorldPosition(vortexNode.x, vortexNode.y), Quaternion.identity);
            Transform vortexTransform = Instantiate(vortexPrefab, GetWorldPositionWithOffset(vortexNode.x, vortexNode.y), Quaternion.identity, transform).transform;
            GameObject lineRendererGO = Instantiate(new GameObject("LineRenderer"), vortexTransform.position, Quaternion.identity, vortexTransform);
            LineRenderer lineRenderer = lineRendererGO.GetComponent<LineRenderer>();
            if (!lineRenderer) {
                lineRenderer = lineRendererGO.AddComponent<LineRenderer>();
            }
            
            InitializeLineRenderer(lineRenderer);
            vortexLineRenderers[vortexNode] = lineRenderer;

            // Mark surrounding tiles as non-buildable
            MarkSurroundingTilesAsNonBuildable(vortexNode);

            // Calculate path for the vortex to the core
            List<GridMapObject> path = gridMap.FindPath(vortexNode.x, vortexNode.y, core.x, core.y);
            if (path != null) {
                vortexPaths[vortexNode] = path; // Store the path for the vortex
            }
        }
    }

    private void MarkSurroundingTilesAsNonBuildable(GridMapObject vortexNode) {
        for (int x = vortexNode.x - 1; x <= vortexNode.x + 1; x++) {
            for (int y = vortexNode.y - 1; y <= vortexNode.y + 1; y++) {
                if (x >= 0 && x < gridMap.GetGrid().GetWidth() && y >= 0 && y < gridMap.GetGrid().GetHeight()) {
                    GridMapObject surroundingNode = gridMap.GetGrid().GetGridObject(x, y);
                    if (surroundingNode != null) {
                        surroundingNode.SetIsBuildable(false); // Mark as non-buildable
                        //surroundingNode.SetNodeType(GridMapObject.NodeType.None); // Change to None or whatever type you want
                    }
                }
            }
        }
    }

    public bool TryUpdatePathForVortexList(GridMapObject testGridObject) {
        if (core == null) {
            Debug.LogError("Core has not been set.");
            return false;
        }

        // Temporarily mark the test grid object as unwalkable
        bool originalIsWalkable = testGridObject.IsWalkable;
        testGridObject.SetIsWalkable(false);

        Dictionary<GridMapObject, List<GridMapObject>> tempVortexPaths = new Dictionary<GridMapObject, List<GridMapObject>>();

        foreach (var vortex in vortexes) {
            if (vortex != null) {
                List<GridMapObject> path = gridMap.FindPath(vortex.x, vortex.y, core.x, core.y);
                if (path != null) {
                    tempVortexPaths[vortex] = path; // Store the temporary path
                    //Debug.Log($"Path found and updated from vortex at ({vortex.x}, {vortex.y}) to core.");
                } else {
                    Debug.LogWarning($"Failed to find a path from vortex at ({vortex.x}, {vortex.y}) to core.");

                    // Revert the test grid object to its original state
                    testGridObject.SetIsWalkable(originalIsWalkable);
                    return false;
                }
            } else {
                Debug.LogWarning("A vortex in the list is null.");

                // Revert the test grid object to its original state
                testGridObject.SetIsWalkable(originalIsWalkable);
                return false;
            }
        }

        // All paths are valid; update the actual vortex paths
        vortexPaths = tempVortexPaths;

        // Revert the test grid object to its original state
        testGridObject.SetIsWalkable(originalIsWalkable);

        //Debug.Log("All paths updated!");
        return true;
    }

    public void UpdatePathForVortexList() {
        if (core == null) {
            Debug.LogError("Core has not been set.");
        }

        vortexPaths.Clear(); // Clear previous paths to update with new ones

        foreach (GridMapObject vortex in vortexes) {
            if (vortex != null) {
                if (core != null) {
                    List<GridMapObject> path = gridMap.FindPath(vortex.x, vortex.y, core.x, core.y);
                    if (path != null) {
                        vortexPaths[vortex] = path; // Update the dictionary with the new path
                        //Debug.Log($"Path found and updated from vortex at ({vortex.x}, {vortex.y}) to core.");
                    } else {
                        Debug.LogWarning($"Failed to find a path from vortex at ({vortex.x}, {vortex.y}) to core.");
                    }
                } else {
                    Debug.LogWarning("Core has not been set.");
                }
            } else {
                Debug.LogWarning("A vortex in the list is null.");
            }
        }
    }
    
    public List<GridMapObject> GetVortexList() {
        return vortexes;
    }

    public GridMapObject GetMainVortexPosition() {
        if (vortexes[0] == null) {
            Debug.LogWarning("Main Vortex cannot be found!");
        }

        return gridMap.GetNode(vortexes[0].x, vortexes[0].y);
    }

    public Dictionary<GridMapObject, List<GridMapObject>> GetVortexPathDictionary() {
        return vortexPaths;
    }

    public float GetCellSize() {
        return cellSize;
    }
}