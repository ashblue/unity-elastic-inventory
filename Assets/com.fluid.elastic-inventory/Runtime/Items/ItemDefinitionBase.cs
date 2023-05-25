using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public abstract class ItemDefinitionBase : ScriptableObject, IItemDefinition {
        [SerializeField]
        string _id;

        [SerializeField]
        string _displayName;

        public string Id => _id;
        public string DisplayName => _displayName;

        public virtual IItemEntry CreateItemEntry (int quantity = 1) {
            return new ItemEntry(this, quantity);
        }
    }
}
