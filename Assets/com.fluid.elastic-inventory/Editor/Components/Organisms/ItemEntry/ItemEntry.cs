using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class ItemEntry : ComponentBase {
        public ItemEntry (VisualElement container, ItemDefinitionBase definition) : base(container) {
            _container.Q<Label>("item-entry__name").text = definition.DisplayName;

            _container.Q<Image>("item-entry__image").image = definition.Image != null
                ? definition.Image.texture
                : AssetDatabase.LoadAssetAtPath<Texture2D>($"{AssetPath.BasePath}/com.fluid.elastic-inventory/Images/PlaceholderImage.png");

            _container.Q<Button>("item-entry__edit").clicked += () => { Selection.activeObject = definition; };
        }
    }
}
