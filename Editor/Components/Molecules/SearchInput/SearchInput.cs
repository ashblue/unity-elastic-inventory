using UnityEngine.UIElements;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class SearchInput : ComponentBase {
        public SearchInput (VisualElement container) : base(container) {
        }

        public void BindChange (System.Action<string> callback) {
            _container
                .Q<TextField>("search-input__text")
                .RegisterValueChangedCallback(evt => {
                    callback(evt.newValue);
                });
        }
    }
}
