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
            if (iterator.name == "turretSO") {
                // Draw the TurretSO field
                EditorGUILayout.PropertyField(iterator, new GUIContent("Turret Stats Scriptable Object"));
                
                // Get the assigned TurretSO
                TurretSO turretSO = (TurretSO)iterator.objectReferenceValue;

                if (turretSO != null) {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Turret Stats (Read-Only)", EditorStyles.boldLabel);

                    // Display the fields from the ScriptableObject as read-only
                    EditorGUI.BeginDisabledGroup(true); // Disable editing
                    EditorGUILayout.TextField("Turret Name", turretSO.turretName);
                    EditorGUILayout.TextField("Turret Description", turretSO.turretDescription);
                    EditorGUILayout.TextField("Prefab", turretSO.turretPrefab.name);
                    EditorGUILayout.IntField("Damage", turretSO.baseDamage);
                    EditorGUILayout.FloatField("Range", turretSO.baseRange);
                    EditorGUILayout.IntField("Cost", turretSO.baseCost);
                    EditorGUILayout.FloatField("Fire Rate", turretSO.baseFireRate);
                    EditorGUILayout.FloatField("Fire Cooldown", turretSO.baseFireCooldown);
                    EditorGUILayout.FloatField("Rotation Speed", turretSO.baseRotationSpeed);
                    EditorGUILayout.FloatField("Projectile Speed", turretSO.baseProjectileSpeed);
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
