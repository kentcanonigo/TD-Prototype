using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TurretFiring))]
public class TurretFiringEditor : Editor {
    public override void OnInspectorGUI() {
        // Get the serialized properties
        SerializedProperty shootAlwaysProperty = serializedObject.FindProperty("shootAlways");
        SerializedProperty angleThresholdProperty = serializedObject.FindProperty("angleThreshold");

        // Draw the shootAlways field
        EditorGUILayout.PropertyField(shootAlwaysProperty, new GUIContent("Shoot Always"));

        // Conditionally draw the angleThreshold field
        if (!shootAlwaysProperty.boolValue) {
            EditorGUILayout.PropertyField(angleThresholdProperty, new GUIContent("Angle Threshold"));
        }

        // Apply the modified properties
        serializedObject.ApplyModifiedProperties();
    }
}