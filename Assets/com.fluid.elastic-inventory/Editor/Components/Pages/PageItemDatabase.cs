using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class PageItemDatabase : ComponentBase {
        public PageItemDatabase (VisualElement container, ItemDatabase database) : base(container) {
            container.Q<Label>("title").text = database.name;

            var allImages = container.Query<Image>("imageElement").ToList();
            foreach (Image e in allImages) {
                e.image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/PlaceholderImage.png");
            }

            // @TODO Inject dropdown names dynamically
            // @TODO Add button functionality should be based on a collected child classes
            var dropdownAdd = container.Q<DropdownField>("add");
            dropdownAdd.RegisterCallback<ChangeEvent<string>>((e) => {
                dropdownAdd.value = "Add";
                if (e.newValue == "Add") return;

                var projectWindowPath = GetSelectedFolderPath();
                var path = EditorUtility.SaveFilePanelInProject(
                    "Create Item Definition",
                    "ItemDefinition",
                    "asset",
                    "Enter a file name for your item",
                    projectWindowPath
                );

                if (string.IsNullOrEmpty(path)) return;

                var itemDefinition = ScriptableObject.CreateInstance<ItemDefinition>();

                var serializedObject = new SerializedObject(itemDefinition);
                var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                serializedObject.FindProperty("_displayName").stringValue = MakeSpacedWord(fileName);
                // @TODO Duplicate IDs are inevitable due to object cloning, repair them automatically when refreshing asset references
                serializedObject.FindProperty("_id").stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedPropertiesWithoutUndo();

                AssetDatabase.CreateAsset(itemDefinition, path);
                database._definitions.Add(itemDefinition);
                EditorUtility.SetDirty(database);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            });
        }

        private static string GetSelectedFolderPath () {
            var selectedObjects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

            if (selectedObjects.Length <= 0) return "Assets";
            var selectedPath = AssetDatabase.GetAssetPath(selectedObjects[0]);

            if (!string.IsNullOrEmpty(selectedPath) && System.IO.Directory.Exists(selectedPath)) {
                return selectedPath;
            }

            return "Assets";
        }

        private string MakeSpacedWord (string word) {
            var spacedWord = string.Empty;

            for (var i = 0; i < word.Length; i++) {
                if (i > 0 && char.IsUpper(word[i])) {
                    spacedWord += " ";
                }

                spacedWord += word[i];
            }

            return spacedWord;
        }
    }
}
