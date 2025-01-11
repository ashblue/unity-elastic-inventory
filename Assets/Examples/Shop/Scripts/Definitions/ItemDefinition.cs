using UnityEngine;
using CleverCrow.Fluid.ElasticInventory;

namespace CleverCrow.Fluid.Examples {
    [ItemDefinitionDetails("Generic")]
    public class ItemDefinition : ItemDefinitionFantasyBase {
        [SerializeField]
        string _displayName;

        [InventoryCategory]
        [SerializeField]
        int _category;

        public override string DisplayName => _displayName;
        public override string Category => GetCategoryByIndex(_category);
    }
}
