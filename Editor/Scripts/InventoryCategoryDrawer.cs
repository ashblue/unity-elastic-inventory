using System;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    [CustomPropertyDrawer(typeof(InventoryCategory))]
    public class InventoryCategoryDrawer : PropertyDrawer {
        string[] _options {
            get {
                var database = Resources.Load<ItemDatabase>("ItemDatabase");
                if (database == null) {
                    Debug.LogWarning("Could not find ItemDatabase. Please create one at `Resources/ItemDatabase'.");
                    return Array.Empty<string>();
                }

                return database.Categories.ToArray();
            }
        }

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            property.intValue = EditorGUI.Popup(position, property.intValue, _options);

            EditorGUI.EndProperty();
        }
    }
}
