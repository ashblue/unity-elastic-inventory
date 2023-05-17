using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    public class PrintItemQuantity : MonoBehaviour {
        [SerializeField]
        private TMPro.TextMeshProUGUI _text;

        [SerializeField]
        private InventoryHelper _inventory;

        [SerializeField]
        private ItemDefinitionBase _itemDefinition;

        void Start () {
            var entry = _inventory.Instance.Get(_itemDefinition);
            _text.text = entry != null ? entry.Quantity.ToString() : "0";

            _inventory.Instance.Events.ItemChanged.AddListener(OnItemChange);
        }

        void OnDestroy () {
            _inventory.Instance.Events.ItemChanged.RemoveListener(OnItemChange);
        }

        void OnItemChange (IItemEntryReadOnly entry) {
            if ((ItemDefinitionBase)entry.Definition != _itemDefinition) return;
            _text.text = entry.Quantity.ToString();
        }
    }
}
