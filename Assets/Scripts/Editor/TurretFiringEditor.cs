using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TurretFiring))]
public class TurretFiringEditor : Editor {
    public override void OnInspectorGUI() {
        // Update the serialized object
        serializedObject.Update();

        // Get the serialized properties
        SerializedProperty shootAlwaysProperty = serializedObject.FindProperty("shootAlways");
        SerializedProperty angleThresholdProperty = serializedObject.FindProperty("angleThreshold");

        // Draw the shootAlways field
        EditorGUILayout.PropertyField(shootAlwaysProperty, new GUIContent("Shoot Always"));

        // Conditionally draw the angleThreshold field
        if (!shootAlwaysProperty.boolValue) {
            EditorGUILayout.PropertyField(angleThresholdProperty, new GUIContent("Angle Threshold"));
        }

        // Draw the remaining properties as default, excluding "m_Script", "shootAlways", and "angleThreshold"
        DrawPropertiesExcluding(serializedObject, "m_Script", "shootAlways", "angleThreshold");

        // Apply the modified properties
        serializedObject.ApplyModifiedProperties();
    }
}