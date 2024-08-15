using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TurretEnemyDetection))]
public class TurretEnemyDetectionEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        // Request a repaint of the scene view
        SceneView.RepaintAll();
    }
}