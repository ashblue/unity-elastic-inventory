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

            var dropdownAdd = container.Q<DropdownField>("add");
            dropdownAdd.RegisterCallback<ChangeEvent<string>>((e) => {
                // @TODO Pull up the location saver
                Debug.Log(e.newValue);
                dropdownAdd.value = "Add";
            });
        }
    }
}
