using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeableTurret), true)] // This allows it to work with derived classes
public class UpgradeableTurretEditor : Editor {
    public override void OnInspectorGUI() {
        // Get the Turret instance
        UpgradeableTurret turret = (UpgradeableTurret)target;

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
                    EditorGUILayout.LabelField("Turret Stats", EditorStyles.boldLabel);

                    if (!EditorApplication.isPlaying) {
                        EditorGUI.BeginDisabledGroup(true);
                        // Display values from the ScriptableObject when not in play mode
                        EditorGUILayout.TextField("Turret Name", turretSO.turretName);
                        EditorGUILayout.TextField("Turret Description", turretSO.turretDescription);
                        EditorGUILayout.IntField("Damage", turretSO.baseDamage);
                        EditorGUILayout.FloatField("Range", turretSO.baseRange);
                        EditorGUILayout.IntField("Cost", turretSO.baseCost);
                        EditorGUILayout.FloatField("Fire Rate", turretSO.baseFireRate);
                        EditorGUILayout.FloatField("Rotation Speed", turretSO.baseRotationSpeed);
                        EditorGUILayout.FloatField("Projectile Speed", turretSO.baseProjectileSpeed);
                        EditorGUI.EndDisabledGroup();
                    } else {
                        // Allow editing of live values in play mode
                        EditorGUI.BeginChangeCheck();

                        EditorGUI.BeginDisabledGroup(true);
                        turret.TurretName = EditorGUILayout.TextField("Turret Name", turret.TurretName);
                        turret.TurretDescription = EditorGUILayout.TextField("Turret Description", turret.TurretDescription);
                        EditorGUI.EndDisabledGroup();
                        turret.BaseDamage = EditorGUILayout.IntField("Damage", turret.BaseDamage);
                        turret.BaseRange = EditorGUILayout.FloatField("Range", turret.BaseRange);
                        EditorGUILayout.LabelField("The upgrade cost is absolute when saved!", EditorStyles.boldLabel);
                        turret.BaseCost = EditorGUILayout.IntField("Cost", turret.BaseCost);
                        EditorGUILayout.Space();
                        turret.BaseFireRate = EditorGUILayout.FloatField("Fire Rate", turret.BaseFireRate);
                        turret.BaseRotationSpeed = EditorGUILayout.FloatField("Rotation Speed", turret.BaseRotationSpeed);
                        turret.BaseProjectileSpeed = EditorGUILayout.FloatField("Projectile Speed", turret.BaseProjectileSpeed);

                        if (EditorGUI.EndChangeCheck()) {
                            // Mark the turret as dirty if any field has been modified
                            EditorUtility.SetDirty(turret);
                        }

                        // Show the export button
                        if (GUILayout.Button("Export Turret Upgrade")) {
                            ExportTurretUpgrade(turret);
                        }
                    }
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

    private void ExportTurretUpgrade(Turret turret) {
        // Create a new instance of the TurretUpgradeSO
        TurretUpgradeSO newUpgrade = ScriptableObject.CreateInstance<TurretUpgradeSO>();

        // Calculate bonuses by subtracting base values from current turret values
        newUpgrade.upgradeCost = turret.TurretSO.baseCost;
        newUpgrade.bonusDamage = turret.BaseDamage - turret.TurretSO.baseDamage;
        newUpgrade.bonusRange = turret.BaseRange - turret.TurretSO.baseRange;
        newUpgrade.bonusFireRate = turret.BaseFireRate - turret.TurretSO.baseFireRate;
        newUpgrade.bonusRotationSpeed = turret.BaseRotationSpeed - turret.TurretSO.baseRotationSpeed;
        newUpgrade.bonusProjectileSpeed = turret.BaseProjectileSpeed - turret.TurretSO.baseProjectileSpeed;

        // Save the new asset to the project
        string path = $"Assets/ScriptableObjects/Turrets/{turret.TurretName} Upgrade.asset";
        AssetDatabase.CreateAsset(newUpgrade, path);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("Export Complete", $"Turret Upgrade exported to {path}", "OK");
    }
}
