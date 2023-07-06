using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class ItemEntry : ComponentBase {
        public ItemEntry (
            VisualElement container,
            ItemDefinitionBase definition,
            Action<ItemDefinitionBase> deleteCallback
        ) : base(container) {
            var title = _container.Q<Label>("item-entry__name");
            title.text = definition.DisplayName;
            title.bindingPath = "_displayName";
            title.Bind(new SerializedObject(definition));

            _container.Q<Button>("item-entry__edit").clicked += () => { Selection.activeObject = definition; };

            _container.Q<Button>("item-entry__delete").clicked += () => {
                var confirm = EditorUtility.DisplayDialog(
                    "Delete Item Definition",
                    $"Are you sure you want to delete {definition.DisplayName}?",
                    "Delete",
                    "Cancel"
                );
                if (!confirm) return;

                _container.parent.Remove(_container);
                deleteCallback(definition);
            };
        }
    }
}
