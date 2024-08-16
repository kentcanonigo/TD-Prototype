using System;
using CodeMonkey.Utils;
using UnityEngine;

public class GridSelection : MonoBehaviour {
    
    public static GridSelection Instance { get; private set; }

    [SerializeField] private bool debugMode;
    
    public Grid<GridMapObject> grid;
    public LayerMask gridLayerMask; // Layer mask to identify the grid
    public Color highlightColor = Color.yellow;
    private Vector2Int selectedGridPosition;
    private bool isGridCellSelected;

    public event EventHandler OnDeselectGridCell;
    public event EventHandler<OnSelectGridCellEventArgs> OnSelectGridCell;
    public class OnSelectGridCellEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GridManager.Instance.OnGridMapInitialized += GridManager_OnGridMapInitialized;
    }

    private void GridManager_OnGridMapInitialized(object sender, EventArgs e) {
        grid = GridManager.Instance.TryGetMainGrid();
    }

    private void Update() {
        // Handle mouse input
        if (Input.GetMouseButtonDown(0)) {
            SelectGridCell();
        }
    }

    private void SelectGridCell() {
        // Check if the pointer is over any UI element
        if (UtilsClass.IsPointerOverUI()) {
            return; // If the pointer is over UI, do nothing
        }
        
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        grid.GetXY(mousePosition, out int x, out int y);

        // Check if the selected cell is valid
        GridMapObject selectedObject = grid.GetGridObject(x, y);
        if (selectedObject != null) {
            Vector2Int newGridPosition = new Vector2Int(x, y);

            // Check if the clicked cell is the already selected cell
            if (isGridCellSelected && selectedGridPosition == newGridPosition) {
                // Deselect the cell
                isGridCellSelected = false;
                selectedGridPosition = Vector2Int.zero;
                OnDeselectGridCell?.Invoke(this, EventArgs.Empty); // Add an event for deselection if needed
                if (debugMode) {
                    Debug.Log($"Deselected grid cell at ({x}, {y})");
                }
            } else {
                // Select the new cell
                selectedGridPosition = newGridPosition;
                isGridCellSelected = true;
                OnSelectGridCell?.Invoke(this, new OnSelectGridCellEventArgs { x = x, y = y });
                if (debugMode) {
                    HighlightSelectedCell(x, y);
                    Debug.Log($"Selected grid cell at ({x}, {y})");
                }
            }
        }
    }

    private void HighlightSelectedCell(int x, int y) {
        // Use Gizmos or another method to highlight the selected cell
        Vector3 cellCenter = grid.GetWorldPosition(x, y) + new Vector3(0.5f, 0.5f) * grid.GetCellSize();
        Debug.DrawLine(cellCenter - Vector3.right * 0.5f, cellCenter + Vector3.right * 0.5f, highlightColor, 0.1f);
        Debug.DrawLine(cellCenter - Vector3.up * 0.5f, cellCenter + Vector3.up * 0.5f, highlightColor, 0.1f);
    }
    
    public void TriggerSelectGridCell(int x, int y) {
        OnSelectGridCell?.Invoke(this, new OnSelectGridCellEventArgs { x = x, y = y });
    }
}