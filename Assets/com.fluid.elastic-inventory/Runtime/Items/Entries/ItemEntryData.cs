using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    /// <summary>
    /// A re-usable class for visually representing item entries in the inspector
    /// </summary>
    [System.Serializable]
    public class ItemEntryData {
        [SerializeField]
        private ItemDefinitionBase _definition;

        [SerializeField]
        private int _quantity;
        
        public IItemDefinition Definition => _definition;
        public int Quantity => _quantity;
    }
}