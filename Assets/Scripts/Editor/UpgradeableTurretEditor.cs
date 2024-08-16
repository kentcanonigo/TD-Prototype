using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeableTurret), true)]
public class UpgradeableTurretEditor : Editor {
    public override void OnInspectorGUI() {
        UpgradeableTurret turret = (UpgradeableTurret)target;

        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true); // Skip the "Script" field

        while (iterator.NextVisible(false)) {
            if (iterator.name == "turretSO") {
                EditorGUILayout.PropertyField(iterator, new GUIContent("Turret Stats Scriptable Object"));

                TurretSO turretSO = (TurretSO)iterator.objectReferenceValue;

                if (turretSO) {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Turret Stats", EditorStyles.boldLabel);

                    if (!EditorApplication.isPlaying) {
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.BeginDisabledGroup(true);
                        turret.TurretName = EditorGUILayout.TextField("Turret Name", turretSO.turretName);
                        turret.TurretDescription = EditorGUILayout.TextField("Turret Description", turretSO.turretDescription);
                        turret.BaseDamage = EditorGUILayout.IntField("Damage", turret.BaseDamage);
                        turret.BaseRange = EditorGUILayout.FloatField("Range", turret.BaseRange);
                        turret.BaseCost = EditorGUILayout.IntField("Cost", turret.BaseCost);
                        turret.BaseFireRate = EditorGUILayout.FloatField("Fire Rate", turret.BaseFireRate);
                        turret.BaseRotationSpeed = EditorGUILayout.FloatField("Rotation Speed", turret.BaseRotationSpeed);
                        turret.BaseProjectileSpeed = EditorGUILayout.FloatField("Projectile Speed", turret.BaseProjectileSpeed);
                        
                        if (EditorGUI.EndChangeCheck()) {
                            EditorUtility.SetDirty(turret);
                            serializedObject.ApplyModifiedProperties();
                        }
                        EditorGUI.EndDisabledGroup();
                    } else {
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.BeginDisabledGroup(true);
                        turret.TurretName = EditorGUILayout.TextField("Turret Name", turret.TurretName);
                        turret.TurretDescription = EditorGUILayout.TextField("Turret Description", turret.TurretDescription);
                        EditorGUI.EndDisabledGroup();
                        turret.BaseDamage = EditorGUILayout.IntField("Damage", turret.BaseDamage);
                        turret.BaseRange = EditorGUILayout.FloatField("Range", turret.BaseRange);
                        turret.BaseCost = EditorGUILayout.IntField("Cost", turret.BaseCost);
                        turret.BaseFireRate = EditorGUILayout.FloatField("Fire Rate", turret.BaseFireRate);
                        turret.BaseRotationSpeed = EditorGUILayout.FloatField("Rotation Speed", turret.BaseRotationSpeed);
                        turret.BaseProjectileSpeed = EditorGUILayout.FloatField("Projectile Speed", turret.BaseProjectileSpeed);

                        if (EditorGUI.EndChangeCheck()) {
                            EditorUtility.SetDirty(turret);
                        }

                        if (GUILayout.Button("Export Turret Upgrade")) {
                            ExportTurretUpgrade(turret);
                        }
                    }
                } else {
                    EditorGUILayout.HelpBox("TurretSO is not assigned.", MessageType.Warning);
                }
            } else if (iterator.name == "turretUpgradeListSO") {
                EditorGUILayout.PropertyField(iterator, new GUIContent("Turret Upgrade List SO"));
                TurretUpgradeListSO turretUpgradeListSO = (TurretUpgradeListSO)iterator.objectReferenceValue;

                if (turretUpgradeListSO) {
                    EditorGUILayout.Space();
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.IntField("Number of Upgrades", turretUpgradeListSO.turretUpgradeSOList.Count);
                    EditorGUILayout.IntField("Current Upgrade", turret.appliedTurretUpgradeSOList.Count);
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.Space();

                    if (GUILayout.Button("Upgrade Turret")) {
                        if (turret.Upgrade()) {
                            Debug.Log("Turret Upgraded");
                            UpdateTurretAndGizmos(turret);
                        }
                    }

                    if (GUILayout.Button("Downgrade Turret")) {
                        if (turret.Downgrade()) {
                            Debug.Log("Turret Downgraded");
                            UpdateTurretAndGizmos(turret);
                        }
                    }

                    EditorGUILayout.Space();
                } else {
                    EditorGUILayout.HelpBox("TurretUpgradeListSO is not assigned.", MessageType.Warning);
                }
            } else {
                EditorGUILayout.PropertyField(iterator, true);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ExportTurretUpgrade(Turret turret) {
        TurretUpgradeSO newUpgrade = ScriptableObject.CreateInstance<TurretUpgradeSO>();

        newUpgrade.upgradeCost = turret.TurretSO.baseCost;
        newUpgrade.bonusDamage = turret.BaseDamage - turret.TurretSO.baseDamage;
        newUpgrade.bonusRange = turret.BaseRange - turret.TurretSO.baseRange;
        newUpgrade.bonusFireRate = turret.BaseFireRate - turret.TurretSO.baseFireRate;
        newUpgrade.bonusRotationSpeed = turret.BaseRotationSpeed - turret.TurretSO.baseRotationSpeed;
        newUpgrade.bonusProjectileSpeed = turret.BaseProjectileSpeed - turret.TurretSO.baseProjectileSpeed;

        string path = $"Assets/ScriptableObjects/Turrets/{turret.TurretName}UpgradeSO_.asset";
        AssetDatabase.CreateAsset(newUpgrade, path);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("Export Complete", $"Turret Upgrade exported to {path}", "OK");
    }
    
    private void UpdateTurretAndGizmos(UpgradeableTurret turret) {
        EditorUtility.SetDirty(turret);
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();

        // Trigger the gizmo update
        TurretEnemyDetection detection = turret.GetComponent<TurretEnemyDetection>();
        if (detection) {
            detection.UpdateGizmos();
        }

        // Optionally force the scene to repaint so the gizmos update visually
        Repaint();
        SceneView.RepaintAll();
        EditorApplication.QueuePlayerLoopUpdate();
    }    
}
