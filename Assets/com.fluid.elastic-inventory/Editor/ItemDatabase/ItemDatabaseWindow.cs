using UnityEditor;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class ItemDatabaseWindow : EditorWindow {
        static ItemDatabase _database;
        static PageItemDatabase _page;
        const string _databasePathKey = "ElasticInventoryPath";

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

            RefreshWindow();
        }

        void OnFocus () {
            // If the user edits the categories while displaying the window they get out of sync
            // This makes sure that cannot happen
            _page?.SyncCategories();
        }

        private static void RefreshWindow () {
            var window = GetWindow<ItemDatabaseWindow>("Item Database");

            window.rootVisualElement.Clear();
            var root = window.rootVisualElement;
            _page = new PageItemDatabase(root, _database);
        }
    }
}
