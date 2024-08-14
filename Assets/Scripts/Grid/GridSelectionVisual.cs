using System;
using UnityEngine;

public class GridSelectionVisual : MonoBehaviour {
    [SerializeField] private GameObject highlightSpritePrefab;
    private GameObject highlightSpriteInstance;

    private void Start() {
        GridSelection.Instance.OnSelectGridCell += GridSelection_OnSelectGridCell;
        GridSelection.Instance.OnDeselectGridCell += GridSelection_OnDeselectGridCell;
        highlightSpriteInstance = Instantiate(highlightSpritePrefab);
        highlightSpriteInstance.SetActive(false); // Hide it initially
    }

    private void GridSelection_OnDeselectGridCell(object sender, EventArgs e) {
        ClearHighlight();
    }

    private void GridSelection_OnSelectGridCell(object sender, GridSelection.OnSelectGridCellEventArgs e) {
        // Get the world position of the cell's center
        Vector3 cellCenter = GridManager.Instance.TryGetMainGrid().GetWorldPosition(e.x, e.y) + new Vector3(0.5f, 0.5f) * GridManager.Instance.TryGetMainGrid().GetCellSize();

        // Move the sprite to the cell's center position
        highlightSpriteInstance.transform.position = cellCenter;

        // Make sure the sprite is active and visible
        highlightSpriteInstance.SetActive(true);
    }
    
    private void ClearHighlight() {
        // Hide the sprite and reset the current selected cell
        highlightSpriteInstance.SetActive(false);
    }
}
