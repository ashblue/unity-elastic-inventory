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
        }
    }
}
