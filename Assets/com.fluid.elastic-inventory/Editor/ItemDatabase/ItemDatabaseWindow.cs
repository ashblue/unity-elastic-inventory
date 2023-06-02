using UnityEditor;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class ItemDatabaseWindow : EditorWindow {
        static ItemDatabase _database;
        const string _databasePathKey = "ElasticInventoryPath";

        [MenuItem("Window/Elastic Inventory/Item Database")]
        public static void ShowWindow (ItemDatabase database) {
            _database = database;
            var assetPath = AssetDatabase.GetAssetPath(database);
            EditorPrefs.SetString(_databasePathKey, assetPath);

            RefreshWindow();
        }

        private void OnEnable () {
            var assetPath = EditorPrefs.GetString(_databasePathKey);
            if (!string.IsNullOrEmpty(assetPath)) {
                _database = AssetDatabase.LoadAssetAtPath<ItemDatabase>(assetPath);
            }

            RefreshWindow();
        }

        private static void RefreshWindow () {
            var window = GetWindow<ItemDatabaseWindow>("Item Database");

            window.rootVisualElement.Clear();
            var root = window.rootVisualElement;
            new PageItemDatabase(root, _database);
        }
    }
}
