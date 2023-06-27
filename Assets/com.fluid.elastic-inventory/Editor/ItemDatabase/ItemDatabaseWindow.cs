using UnityEditor;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class ItemDatabaseWindow : EditorWindow {
        static ItemDatabase _database;
        static PageItemDatabase _page;
        const string _databasePathKey = "ElasticInventoryPath";

        public static bool Dirty { get; set; }

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
            // If the window is marked dirty rebuild it
            if (Dirty) {
                RefreshWindow(true);
                Dirty = false;
            }

            // If the user edits the categories while displaying the window they get out of sync
            // This makes sure that cannot happen
            _page?.SyncCategories();
        }

        private static void RefreshWindow (bool clear = false) {
            var window = GetWindow<ItemDatabaseWindow>("Item Database");
            var root = window.rootVisualElement;

            if (clear) {
                root.Clear();
                _page = null;
            }

            _page = new PageItemDatabase(root, _database);
        }
    }
}
