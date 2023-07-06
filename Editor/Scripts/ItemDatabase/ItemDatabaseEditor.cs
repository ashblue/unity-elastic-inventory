using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor : Editor {
        public override void OnInspectorGUI () {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            var database = target as ItemDatabase;

            if (GUILayout.Button("Edit Database")) {
                ItemDatabaseWindow.ShowWindow(database);
            }

            if (GUILayout.Button("Sync")) {
                var assets = AssetDatabase.FindAssets("t:ItemDefinitionBase");

                database._definitions.Clear();
                foreach (var asset in assets) {
                    var path = AssetDatabase.GUIDToAssetPath(asset);
                    var definition = AssetDatabase.LoadAssetAtPath<ItemDefinitionBase>(path);
                    database._definitions.Add(definition);
                }

                EditorUtility.SetDirty(database);
                AssetDatabase.SaveAssets();
                ItemDatabaseWindow.ShowWindow(database);

                ItemDatabaseWindow.Dirty = true;
            }

            if (GUILayout.Button("Repair IDs")) {
                FixDuplicateItemIds();
            }
        }

        private void FixDuplicateItemIds () {
            if (!EditorUtility.DisplayDialog(
                    "Confirm Fix IDs",
                    "Are you sure you want to fix duplicate item definition IDs? This will randomize the ID of any duplicates found and print the details to the console. This may affect save data as these IDs are used to restore inventory saves. It is highly recommended you backup your project before proceeding.",
                    "Yes",
                    "Cancel"
                )) return;

            var count = 0;
            Debug.Log("BEGIN: Fixing duplicate item definition IDs.");

            var ids = new HashSet<string>();
            var itemDefinitions = AssetDatabase.FindAssets("t:ItemDefinitionBase");
            foreach (var itemDefinition in itemDefinitions) {
                var path = AssetDatabase.GUIDToAssetPath(itemDefinition);
                var definition = AssetDatabase.LoadAssetAtPath<ItemDefinitionBase>(path);

                if (ids.Contains(definition.Id)) {
                    var newId = System.Guid.NewGuid().ToString();
                    Debug.LogWarning($"Duplicate ID {definition.Id} in {path}. Randomizing the ID to {newId}.");

                    var so = new SerializedObject(definition);
                    var idField = so.FindProperty("_id");
                    idField.stringValue = newId;

                    so.ApplyModifiedProperties();
                    EditorUtility.SetDirty(definition);

                    count++;
                } else {
                    ids.Add(definition.Id);
                }
            }

            if (count > 0) {
                Debug.Log($"Fixed {count} duplicate item definition ID(s).");
                ItemDatabaseWindow.Dirty = true;
            } else {
                Debug.Log("No duplicate item definition IDs found. Nothing to fix.");
            }

            Debug.Log("END: Fixing duplicate item definition IDs.");
        }
    }
}

