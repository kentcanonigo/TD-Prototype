using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy), true)] // This allows it to work with derived classes
public class EnemyEditor : Editor {
    public override void OnInspectorGUI() {
        // Get the Enemy instance
        Enemy enemy = (Enemy)target;

        // Start iterating over the properties of the serialized object
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true); // Skip the "Script" field

        while (iterator.NextVisible(false)) {
            if (iterator.name == "enemySO") {
                // Draw the EnemySO field
                EditorGUILayout.PropertyField(iterator, new GUIContent("Enemy Stats Scriptable Object"));
                
                // Get the assigned EnemySO
                EnemySO enemySO = (EnemySO)iterator.objectReferenceValue;

                if (enemySO != null) {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Enemy Stats (Read-Only)", EditorStyles.boldLabel);

                    // Display the fields from the ScriptableObject as read-only
                    EditorGUI.BeginDisabledGroup(true); // Disable editing
                    EditorGUILayout.TextField("Enemy Name", enemySO.enemyName);
                    EditorGUILayout.TextField("Enemy Description", enemySO.enemyDescription);
                    EditorGUILayout.TextField("Prefab", enemySO.enemyPrefab.name);
                    EditorGUILayout.IntField("Health", enemySO.healthPoints);
                    EditorGUILayout.IntField("Armor", enemySO.armor);
                    EditorGUILayout.FloatField("Speed", enemySO.speed);
                    EditorGUILayout.IntField("Damage to Core", enemySO.damageToCore);
                    EditorGUILayout.FloatField("Size Multiplier", enemySO.sizeMultiplier);
                    EditorGUI.EndDisabledGroup(); // Re-enable editing
                } else {
                    EditorGUILayout.HelpBox("EnemySO is not assigned.", MessageType.Warning);
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