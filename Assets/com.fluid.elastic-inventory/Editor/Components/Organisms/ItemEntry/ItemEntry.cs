using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class ItemEntry : ComponentBase {
        public ItemEntry (VisualElement container, ItemDefinitionBase definition) : base(container) {
            var title = _container.Q<Label>("item-entry__name");
            title.text = definition.DisplayName;
            title.bindingPath = "_displayName";
            title.Bind(new SerializedObject(definition));

            _container.Q<Button>("item-entry__edit").clicked += () => { Selection.activeObject = definition; };
        }
    }
}
