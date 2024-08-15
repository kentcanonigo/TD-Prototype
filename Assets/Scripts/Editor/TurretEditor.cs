using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Turret), true)] // This allows it to work with derived classes
public class TurretEditor : Editor {
    public override void OnInspectorGUI() {
        // Get the Turret instance
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
                        turret.TurretName = EditorGUILayout.TextField("Turret Name", turret.TurretName);
                        turret.TurretDescription = EditorGUILayout.TextField("Turret Description", turret.TurretDescription);
                        turret.BaseDamage = EditorGUILayout.IntField("Damage", turret.BaseDamage);
                        turret.BaseRange = EditorGUILayout.FloatField("Range", turret.BaseRange);
                        turret.BaseCost = EditorGUILayout.IntField("Cost", turret.BaseCost);
                        turret.BaseFireRate = EditorGUILayout.FloatField("Fire Rate", turret.BaseFireRate);
                        turret.BaseRotationSpeed = EditorGUILayout.FloatField("Rotation Speed", turret.BaseRotationSpeed);
                        turret.BaseProjectileSpeed = EditorGUILayout.FloatField("Projectile Speed", turret.BaseProjectileSpeed);
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
}
