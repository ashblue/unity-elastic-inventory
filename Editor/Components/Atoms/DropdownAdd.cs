using System.Collections.Generic;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class DropdownAdd<T> : ComponentBase {
        readonly DropdownField _dropdown;
        readonly List<KeyValuePair<string, T>> _choices;
        readonly string _addText;
        readonly VisualElement _containerParent;
        readonly bool _resetOnInteract;

        // @TODO Make addText optional so the categories will update on click
        public DropdownAdd (
            VisualElement containerParent,
            string addText,
            List<KeyValuePair<string, T>> choices,
            bool resetOnInteract = true
        ) : base(containerParent) {
            _containerParent = containerParent;
            _choices = choices;
            _addText = addText;
            _resetOnInteract = resetOnInteract;

            var keys = choices.ConvertAll(choice => choice.Key);
            keys.Insert(0, addText);

            _dropdown = CreateDropdownField(keys);
        }

        public void BindClick (System.Action<T> callback) {
            _dropdown.RegisterCallback<ChangeEvent<string>>(e => {
                if (_resetOnInteract) _dropdown.value = _addText;
                if (e.newValue == _addText) {
                    callback(default);
                    return;
                }

                var choice = _choices.Find(c => c.Key == e.newValue);
                callback(choice.Value);
            });
        }

        private DropdownField CreateDropdownField (List<string> keys) {
            var dropdown = new DropdownField() {
                choices = keys,
                value = _addText
            };

            _containerParent.Add(dropdown);

            return dropdown;
        }

        public void SetValue (int indexOf) {
            if (indexOf < 0 || indexOf >= _choices.Count) return;
            _dropdown.value = _choices[indexOf].Key;
        }
    }
}
