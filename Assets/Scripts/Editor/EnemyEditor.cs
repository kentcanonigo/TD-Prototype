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
                    EditorGUILayout.LabelField("Enemy Stats (Editable)", EditorStyles.boldLabel);
                    EditorGUI.BeginDisabledGroup(true); // Disable editing
                    EditorGUILayout.TextField("Enemy Name", enemySO.enemyName);
                    EditorGUILayout.TextField("Enemy Description", enemySO.enemyDescription);
                    EditorGUI.EndDisabledGroup(); // Re-enable editing
                    // Allow editing of live values in play mode
                    enemySO.healthPoints = EditorGUILayout.IntField("Health", enemySO.healthPoints);
                    enemySO.armor = EditorGUILayout.IntField("Armor", enemySO.armor);
                    enemySO.speed = EditorGUILayout.FloatField("Speed", enemySO.speed);
                    enemySO.damageToCore = EditorGUILayout.IntField("Damage to Core", enemySO.damageToCore);
                    enemySO.sizeMultiplier = EditorGUILayout.FloatField("Size Multiplier", enemySO.sizeMultiplier);

                    if (EditorApplication.isPlaying) {
                        // Show the Apply Changes button
                        if (GUILayout.Button("Apply Changes")) {
                            ApplyNewEnemyStats(enemy, enemySO.healthPoints, enemySO.armor, enemySO.speed, enemySO.damageToCore, enemySO.sizeMultiplier);
                        }
                    }
                   
                    // Add Save Changes button
                    if (GUILayout.Button("Save Changes")) {
                        SaveEnemyStats(enemySO);
                    }
                } else {
                    EditorGUILayout.HelpBox("EnemySO is not assigned.", MessageType.Warning);
                }
            } else {
                // Draw other properties normally
                EditorGUILayout.PropertyField(iterator, true);
            }
        }

        // Check for Collider2D and Rigidbody2D in child objects
        Collider2D collider = enemy.GetComponentInChildren<Collider2D>();
        Rigidbody2D rigidbody = enemy.GetComponentInChildren<Rigidbody2D>();

        if (collider == null || rigidbody == null) {
            EditorGUILayout.HelpBox("Warning: The child object of this enemy is missing a Collider2D and/or Rigidbody2D component.", MessageType.Warning);
        }

        // Apply any property modifications
        serializedObject.ApplyModifiedProperties();
    }

    private void ApplyNewEnemyStats(Enemy enemy, int health, int armor, float speed, int damageToCore, float sizeMultiplier) {
        enemy.SetHealth(health);
        enemy.SetArmor(armor);
        enemy.SetSpeed(speed);
        enemy.SetDamageToCore(damageToCore);
        enemy.SetSizeMultiplier(sizeMultiplier);

        // Mark the enemy object as dirty
        EditorUtility.SetDirty(enemy);
    }

    private void SaveEnemyStats(EnemySO enemySO) {
        // Debug.Log("Saving enemy stats...");
        // Mark the ScriptableObject as dirty so changes are saved
        EditorUtility.SetDirty(enemySO);
        AssetDatabase.SaveAssets(); // Save the asset to disk
        EditorUtility.DisplayDialog("Save Complete", $"Enemy stats exported to {AssetDatabase.GetAssetPath(enemySO)}", "OK");
    }
}