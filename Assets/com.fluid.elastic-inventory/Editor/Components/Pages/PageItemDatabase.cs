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

        public PageItemDatabase (VisualElement container, ItemDatabase database) : base(container) {
            CreateHeader(container, database);

            _database = database;
            database._definitions.ForEach(AddItemElement);
        }

        void CreateHeader (VisualElement container, ItemDatabase database) {
            container.Q<Label>("title").text = database.name;

            var displayNameToClass = GetDefinitionDisplayNameToClass();
            var dropdownAdd = new DropdownAdd<Type>(container.Q<VisualElement>("add"), "Add", displayNameToClass);
            dropdownAdd.BindClick((Type type) => { CreateItemDefinition(database, type); });

            var dropdownCategory = new DropdownAdd<string>(container.Q<VisualElement>("category"), "Category",
                new List<KeyValuePair<string, string>> {
                    new("Generic", "Generic"),
                    new("Weapon", "Weapon"),
                    new("Armor", "Armor"),
                });

            dropdownCategory.BindClick(category => {
                Debug.Log($"Handle category logic here: {category}");
            });
        }

        static List<KeyValuePair<string, Type>> GetDefinitionDisplayNameToClass () {
            var assembly = Assembly.GetAssembly(typeof(ItemDefinitionBase));

            var keyTypeValueName = assembly.GetTypes()
                .Where(type => type.GetCustomAttribute<ItemDefinitionDetailsAttribute>() != null)
                .Select(type => new KeyValuePair<string, Type>(
                    type.GetCustomAttribute<ItemDefinitionDetailsAttribute>().DisplayName,
                    type
                ))
                .ToList();

            return keyTypeValueName;
        }

        void CreateItemDefinition (ItemDatabase database, Type type) {
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
            serializedObject.FindProperty("_id").stringValue = System.Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            AssetDatabase.CreateAsset(itemDefinition, path);
            database._definitions.Add(itemDefinition as ItemDefinitionBase);
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AddItemElement(itemDefinition as ItemDefinitionBase);
        }

        private void AddItemElement (ItemDefinitionBase itemDefinition) {
            var table = _container.Q<VisualElement>("table");
            new ItemEntry(table, itemDefinition, DeleteItemFromDatabase);
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
    }
}
