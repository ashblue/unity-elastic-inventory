using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public abstract class ItemDefinitionBase : ScriptableObject, IItemDefinition {
        public string Id { get; } = System.Guid.NewGuid().ToString();

        public virtual IItemEntry CreateItemEntry (int quantity = 1) {
            return new ItemEntry(this, quantity);
        }
    }
}
