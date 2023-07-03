using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    public class InventoryPrinter : MonoBehaviour {
        ItemPrinter _activeItem;
        List<ItemCategoryButton> _categories;

        // Trigger a refresh when the inventory is shown and hidden
        bool _refresh;

        [Tooltip("Actively displayed inventory")]
        [SerializeField]
        InventoryHelper _inventory;

        [Tooltip("Inventory that is currently shopping")]
        [SerializeField]
        InventoryHelper _shopper;

        [SerializeField]
        ItemPrinter _itemPrefab;

        [SerializeField]
        Transform _output;

        [SerializeField]
        ItemContextPrinter _context;

        [SerializeField]
        private RectTransform _categoriesContainer;


        void Start () {
            _categories = _categoriesContainer.GetComponentsInChildren<ItemCategoryButton>().ToList();
            _categories.ForEach(c => c.Setup(this));

            var items = _inventory.Instance.GetAll();
            _inventory.Instance.Sort(items, sort: ItemSort.UpdatedAt, order: ItemOrder.Descending);
            PrintItems(items);

            // Setup the category
            ClearCategoryActive();
            _categories[0].SetActive(true);

            // Bind item purchasing logic
            _context.OnPurchase.AddListener(PurchaseItem);
        }

        void OnDisable () {
            _refresh = true;
        }

        void OnEnable () {
            if (!_refresh) return;

            ClearCategoryActive();
            _categories[0].SetActive(true);
            SetCategory("All");
        }

        public void SetCategory (string category) {
            var items = _inventory.Instance.GetAll(category: category == "All" ? null : category);
            _inventory.Instance.Sort(items, sort: ItemSort.UpdatedAt, order: ItemOrder.Descending);

            foreach (Transform child in _output) {
                Destroy(child.gameObject);
            }

            PrintItems(items);
        }

        public void ClearCategoryActive () {
            _categories.ForEach((c) => c.SetActive(false));
        }

        void PurchaseItem (IItemEntryReadOnly item) {
            var definition = item.Definition as ItemDefinitionFantasyBase;

            // If you assign a definition a specific ID, you can quickly reference it with methods like this
            // Use with caution as this can blow up your save and load data
            if (!_shopper.Instance.Has("gold", definition.Cost)) {
                // @NOTE Simple debug to demonstrate feedback. Might wanna implement a UI popup or hide the button instead
                Debug.Log("You don't have enough gold");
                return;
            }

            // Unique Item entries must be removed with this method. Also works for removing normal items
            // Full store implementations should handle unique vs non-unique items so quantity can be used
            _inventory.Instance.RemoveEntry(item.Id);

            _shopper.Instance.Remove("gold", definition.Cost);

            if (item.Definition.Unique) {
                // Unique items often have transferable data, so we need to transfer the entire item entry
                _shopper.Instance.AddEntry(item);
            } else {
                // Add quantity based items normally
                _shopper.Instance.Add(item.Definition);
            }

            // Remove the item from the UI if the player has purchased the last one
            if (!_inventory.Instance.Has(item.Definition) || item.Definition.Unique) {
                Destroy(_activeItem.gameObject);
                _context.Clear();
            } else {
                _activeItem.SetQuantity(_inventory.Instance.Get(item.Definition).Quantity);
            }

            Debug.Log($"Purchase success {item.Definition.DisplayName}");
        }

        void PrintItems (List<IItemEntryReadOnly> items) {
            foreach (var item in items) {
                if (item.Definition.Category == "Hide") continue;

                var itemPrinter = Instantiate(_itemPrefab, _output);
                itemPrinter.Setup(item.Definition as ItemDefinitionFantasyBase, item.Quantity);
                itemPrinter.AddClick(() => {
                    _activeItem = itemPrinter;
                    _context.Set(item);
                });
            }
        }
    }
}
