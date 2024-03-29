using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    [CustomEditor(typeof(InventoryHelper))]
    public class InventoryHelperEditor : Editor {
        public override void OnInspectorGUI () {
            DrawDefaultInspector();

            if (!Application.isPlaying) return;
            var inventoryHelper = (InventoryHelper)target;
            if (inventoryHelper.Instance == null) return;

            var allItems = inventoryHelper.Instance.GetAll();
            EditorGUILayout.LabelField("Current Inventory:");

            foreach (var item in allItems) {
                var quantity = item.Quantity > 1 ? item.Quantity + " " : "";
                EditorGUILayout.LabelField($"- {quantity}{item.Definition.DisplayName}");
            }
        }
    }
}
