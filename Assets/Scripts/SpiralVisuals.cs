using UnityEngine;

public class SpiralVisuals : MonoBehaviour {
    
    [SerializeField] public GameObject spiralPrefab;
    
    private void Start() {
        GridManager.Instance.OnSpiralInitialized += GridMap_OnSpiralInitialized;
    }

    private void GridMap_OnSpiralInitialized(object sender, GridManager.OnSpiralInitializedEventArgs e) {
        Debug.Log($"Spawning Spiral Object at {e.x},{e.y}");
        GameObject spiralGameObject = Instantiate(spiralPrefab, GridManager.Instance.GetWorldPosition(e.x, e.y) + GridManager.Instance.CellOffset, Quaternion.identity, transform);
    }
}
