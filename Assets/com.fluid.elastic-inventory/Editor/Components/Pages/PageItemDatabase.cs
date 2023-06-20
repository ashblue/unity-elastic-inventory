using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class PageItemDatabase : ComponentBase {
        readonly ItemDatabase _database;
        string _category;
        string _search;
        readonly VisualElement _containerParent;

        public PageItemDatabase (VisualElement container, ItemDatabase database) : base(container) {
            _database = database;
            _containerParent = container;

            CreateHeader(container, database);
            PrintItems();
        }

        void PrintItems () {
            ClearItems();

            var definitions = _database._definitions.ToList();

            // Make sure the definitions are sorted in reverse so the newest items are first
            definitions.Reverse();

            definitions.ForEach(item => {
                if (_category != null && item.Category != _category) return;

                if (_search != null && !item.DisplayName.Replace(" ", "").ToLower().Contains(_search)) {
                    return;
                }

                AddItemElement(item);
            });
        }

        void CreateHeader (VisualElement container, ItemDatabase database) {
            container.Q<Label>("title").text = database.name;

            // Create SearchInput
            var searchInput = new SearchInput(container.Q<VisualElement>("search"));
            searchInput.BindChange(search => {
                _search = string.IsNullOrEmpty(search) ? null : search.ToLower();
                PrintItems();
            });

            // Create dropdown add
            var displayNameToClass = GetDefinitionDisplayNameToClass();
            var dropdownAdd = new DropdownAdd<Type>(container.Q<VisualElement>("add"), "Add", displayNameToClass);
            dropdownAdd.BindClick(type => { CreateItemDefinition(database, type); });

            SyncCategories();
        }

        static List<KeyValuePair<string, Type>> GetDefinitionDisplayNameToClass () {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var keyToType = new List<KeyValuePair<string, Type>>();
            foreach (var assembly in allAssemblies) {
                keyToType.AddRange(assembly.GetTypes()
                    .Where(type => type.GetCustomAttribute<ItemDefinitionDetailsAttribute>() != null)
                    .Select(type => new KeyValuePair<string, Type>(
                        type.GetCustomAttribute<ItemDefinitionDetailsAttribute>().DisplayName,
                        type
                    )));
            }

            return keyToType;
        }

        void CreateItemDefinition (ItemDatabase database, Type type) {
            if (type == null) {
                return;
            }

            var projectWindowPath = GetSelectedFolderPath();
            var path = EditorUtility.SaveFilePanelInProject(
                "Create Item Definition",
                "ItemDefinition",
                "asset",
                "Enter a file name for your item",
                projectWindowPath
            );

            if (string.IsNullOrEmpty(path)) return;

            var itemDefinition = ScriptableObject.CreateInstance(type);

            var serializedObject = new SerializedObject(itemDefinition);
            var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            serializedObject.FindProperty("_displayName").stringValue = MakeSpacedWord(fileName);
            // @TODO Duplicate IDs are inevitable due to object cloning, repair them automatically when refreshing asset references
            serializedObject.FindProperty("_id").stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            AssetDatabase.CreateAsset(itemDefinition, path);
            database._definitions.Insert(0, itemDefinition as ItemDefinitionBase);
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AddItemElement(itemDefinition as ItemDefinitionBase);
            Selection.activeObject = itemDefinition;
        }

        private void ClearItems () {
            var table = _container.Q<VisualElement>("table");
            table.Clear();
        }

        private void AddItemElement (ItemDefinitionBase itemDefinition) {
            var table = _container.Q<VisualElement>("table");
            var item = new ItemEntry(table, itemDefinition, DeleteItemFromDatabase);

            // move the item to be the first child so users don't have to hunt for it at the bottom
            table.Remove(item.Container);
            table.Insert(0, item.Container);
        }

        private void DeleteItemFromDatabase (ItemDefinitionBase itemDefinition) {
            _database._definitions.Remove(itemDefinition);
            EditorUtility.SetDirty(_database);

            var path = AssetDatabase.GetAssetPath(itemDefinition);
            AssetDatabase.DeleteAsset(path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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

        public void SyncCategories () {
            _containerParent.Q<VisualElement>("category").Clear();

            var dropdownCategory = new DropdownAdd<int>(
                _containerParent.Q<VisualElement>("category"),
                "Category",
                _database.Categories.Select((category, index) => new KeyValuePair<string, int>(category, index + 1)).ToList(),
                resetOnInteract: false
            );

            if (_category != null) {
                dropdownCategory.SetValue(_database.Categories.IndexOf(_category));
            }

            dropdownCategory.BindClick(category => {
                if (category == 0) _category = null;
                else _category = _database.Categories[category - 1];

                PrintItems();
            });
        }
    }
}
