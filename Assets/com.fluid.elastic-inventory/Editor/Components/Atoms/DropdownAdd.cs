using System.Collections.Generic;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class DropdownAdd<T> : ComponentBase {
        readonly DropdownField _dropdown;
        readonly List<KeyValuePair<string, T>> _choices;
        readonly string _addText;

        public DropdownAdd (VisualElement container, string addText, List<KeyValuePair<string, T>> choices) : base(container) {
            _choices = choices;
            _addText = addText;
            var keys = choices.ConvertAll(choice => choice.Key);
            keys.Insert(0, addText);

            _dropdown = CreateDropdownField(keys);
        }

        public void BindClick (System.Action<T> callback) {
            _dropdown.RegisterCallback<ChangeEvent<string>>(e => {
                _dropdown.value = _addText;
                if (e.newValue == _addText) return;

                var choice = _choices.Find(c => c.Key == e.newValue);
                callback(choice.Value);
            });
        }

        private DropdownField CreateDropdownField (List<string> keys) {
            var dropdown = new DropdownField() {
                choices = keys,
                value = _addText
            };

            _container.Add(dropdown);

            return dropdown;
        }
    }
}
