using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Turret), true)] // This allows it to work with derived classes
public class TurretEditor : Editor {
    public override void OnInspectorGUI() {
        // Get the TurretBase instance
        Turret turret = (Turret)target;

        // Start iterating over the properties of the serialized object
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true); // Skip the "Script" field

        while (iterator.NextVisible(false)) {
            if (iterator.name == "TurretSO") {
                // Draw the TurretSO field
                EditorGUILayout.PropertyField(iterator, new GUIContent("Turret Stats Scriptable Object"));
                
                // Get the assigned TurretSO
                TurretSO TurretSO = (TurretSO)iterator.objectReferenceValue;

                if (TurretSO != null) {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Turret Stats (Read-Only)", EditorStyles.boldLabel);

                    // Display the fields from the ScriptableObject as read-only
                    EditorGUI.BeginDisabledGroup(true); // Disable editing
                    EditorGUILayout.TextField("Turret Name", TurretSO.turretName);
                    EditorGUILayout.TextField("Turret Description", TurretSO.turretDescription);
                    EditorGUILayout.IntField("Damage", TurretSO.baseDamage);
                    EditorGUILayout.FloatField("Range", TurretSO.baseRange);
                    EditorGUILayout.IntField("Cost", TurretSO.baseCost);
                    EditorGUILayout.FloatField("Fire Rate", TurretSO.baseFireRate);
                    EditorGUILayout.FloatField("Rotation Speed", TurretSO.baseRotationSpeed);
                    EditorGUI.EndDisabledGroup(); // Re-enable editing
                } else {
                    EditorGUILayout.HelpBox("TurretSO is not assigned.", MessageType.Warning);
                }
            } else {
                // Draw other properties normally
                EditorGUILayout.PropertyField(iterator, true);
            }
        }

        // Apply any property modifications
        serializedObject.ApplyModifiedProperties();
    }
}
