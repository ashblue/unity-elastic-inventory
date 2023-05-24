using UnityEditor;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class ItemDatabaseWindow : EditorWindow {
        static ItemDatabase _database;
        const string _databasePathKey = "ElasticInventoryPath";
        PageItemDatabase _page;

        [MenuItem("Window/Elastic Inventory/Item Database")]
        public static void ShowWindow (ItemDatabase database) {
            _database = database;
            var assetPath = AssetDatabase.GetAssetPath(database);
            EditorPrefs.SetString(_databasePathKey, assetPath);

            GetWindow<ItemDatabaseWindow>("Item Database");

        }

        private void OnEnable () {
            var assetPath = EditorPrefs.GetString(_databasePathKey);
            if (!string.IsNullOrEmpty(assetPath)) {
                _database = AssetDatabase.LoadAssetAtPath<ItemDatabase>(assetPath);
            }

            rootVisualElement.Clear();
            var root = rootVisualElement;
            _page = new PageItemDatabase(root, _database);
        }

        // private void OnGUI () {
        //     if (_database == null) return;
        //
        //     EditorGUILayout.LabelField(_database.name);
        //
        //     var items = _database._definitions;
        //     foreach (var item in items) {
        //         EditorGUILayout.LabelField(item.name);
        //     }
        // }
    }
}
