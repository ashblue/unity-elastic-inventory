using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    // A re-usable class for visually representing item entries in the inspector
    [System.Serializable]
    public class ItemEntryDefinition {
        [SerializeField]
        private ItemDefinitionBase _definition;

        [SerializeField]
        private int _quantity;
        
        public IItemDefinition Definition => _definition;
        public int Quantity => _quantity;
    }
}