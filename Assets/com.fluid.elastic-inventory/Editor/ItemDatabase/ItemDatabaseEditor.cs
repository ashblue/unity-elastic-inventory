using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor : Editor {
        public override void OnInspectorGUI () {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            var database = target as ItemDatabase;

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
            }

            if (GUILayout.Button("Edit Database")) {
                ItemDatabaseWindow.ShowWindow(database);
            }
        }
    }
}
