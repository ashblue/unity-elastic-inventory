using System.IO;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public static class ConfigWizard {
        [InitializeOnLoadMethod]
        private static void CreateInitialDatabase () {
            if (HasDatabase()) return;

            var inventoryPath = "Assets/ElasticInventory";
            var resourcesPath = $"{inventoryPath}/Resources";

            if (!EditorUtility.DisplayDialog("Create Item Database",
                    $"Your project is missing an Item Database. Would you like to run the wizard? This will setup your project at {inventoryPath} with everything needed to run. This is required for Elastic Inventory to work.",
                    "Yes",
                    "No")) {
                return;
            }

            // Create an inventory folder if one doesn't exist
            if (!AssetDatabase.IsValidFolder(inventoryPath)) {
                AssetDatabase.CreateFolder("Assets", "ElasticInventory");
            }

            // Create a resources folder if one doesn't exist
            if (!AssetDatabase.IsValidFolder(resourcesPath)) {
                AssetDatabase.CreateFolder(inventoryPath, "Resources");
            }

            // Create the database
            var path = $"{resourcesPath}/ItemDatabase.asset";
            var asset = ScriptableObject.CreateInstance<ItemDatabase>();
            AssetDatabase.CreateAsset(asset, path);
            CreateFirstItemScript(inventoryPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Show an alert to the user that the database was created
            EditorUtility.DisplayDialog("Item Database Created",
                $"An Item Database was created at {resourcesPath}/ItemDatabase. You can edit this database by selecting it in the Project window and clicking the Edit button in the inspector. Please note that this file must be in a resources folder and keep the same name.\n\nWe've also added a start sample item script for you. We recommend reviewing ItemDefinition before you start adding new definitions to your database.",
                "OK");
        }

        private static bool HasDatabase() {
            // @NOTE This does not work on a newly cloned project, as the asset database has not been built yet on initial load
            var database = AssetDatabase.FindAssets("t:ItemDatabase");
            if (database.Length > 0) return true;

            // Do a hard file search for the database. Resource intensive but will only happen on initial load after cloning a project
            // @TODO Optimize for larger projects. Should exit after finding the first file. Tricky with potential symlinks
            var files = Directory.GetFiles(Application.dataPath, "ItemDatabase.asset", SearchOption.AllDirectories);
            return files.Length > 0;
        }

        private static void CreateFirstItemScript(string path) {
            path = Path.Combine(path, "ItemDefinition.cs");
            string scriptContents = @"using UnityEngine;
using CleverCrow.Fluid.ElasticInventory;

// This is a sample item definition. You can create as many of these as you want and edit them as you see fit.
// For customization details please visit the docs at https://github.com/ashblue/unity-elastic-inventory

// @IMPORTANT You may want to add a namespace here before creating definitions to avoid potential naming conflicts
// that could break your item definitions long term.

[ItemDefinitionDetails(""Generic"")]
public class ItemDefinition : ItemDefinitionBase {
    [SerializeField]
    string _displayName;

    [InventoryCategory]
    [SerializeField]
    int _category;

    // You can override the display name if you want by hard coding it here
    public override string DisplayName => _displayName;

    // You can hard set a category name here instead if you don't want it to be selectable in the inspector
    public override string Category => GetCategoryByIndex(_category);
}
";

            File.WriteAllText(path, scriptContents);

            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
    }
}
