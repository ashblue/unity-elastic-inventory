using UnityEngine;
using CleverCrow.Fluid.ElasticInventory;

namespace CleverCrow.Fluid.Examples {
    [ItemDefinitionDetails("Generic")]
    public class ItemDefinition : ItemDefinitionFantasyBase {
        [InventoryCategory]
        [SerializeField]
        int _category;

        public override string Category => GetCategoryByIndex(_category);
    }
}
