using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    [ItemDefinitionDetails("Generic")]
    public class ItemDefinition : ItemDefinitionBase {
        [InventoryCategory]
        [SerializeField]
        int _category;

        public override string Category => GetCategoryByIndex(_category);
    }
}
