using UnityEngine;

public class VortexVisuals : MonoBehaviour {
    
    [SerializeField] public GameObject vortexPrefab;
    
    private void Start() {
        GridManager.Instance.OnVortexInitialized += GridMap_OnVortexInitialized;
    }

    private void GridMap_OnVortexInitialized(object sender, GridManager.OnVortexInitializedEventArgs e) {
        Debug.Log($"Spawning Vortex Object at {e.x},{e.y}");
        GameObject vortexGameObject = Instantiate(vortexPrefab, GridManager.Instance.GetWorldPosition(e.x, e.y) + GridManager.Instance.CellOffset, Quaternion.identity, transform);
    }
}
