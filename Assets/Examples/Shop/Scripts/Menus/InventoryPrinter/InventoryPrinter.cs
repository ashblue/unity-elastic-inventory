using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    public class InventoryPrinter : MonoBehaviour {
        List<ItemCategoryButton> _categories;

        [SerializeField]
        InventoryHelper _inventory;

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

            ClearCategoryActive();
            _categories[0].SetActive(true);
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

        void PrintItems (List<IItemEntryReadOnly> items) {
            foreach (var item in items) {
                if (item.Definition.Category == "Hide") continue;
                var itemPrinter = Instantiate(_itemPrefab, _output);
                itemPrinter.Setup(item.Definition as ItemDefinitionFantasyBase, item.Quantity);
                itemPrinter.AddClick(() => _context.Set(item.Definition as ItemDefinitionFantasyBase));
            }
        }
    }
}
