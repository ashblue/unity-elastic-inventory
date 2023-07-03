using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    /// <summary>
    /// A re-usable class for visually representing item entries in the inspector
    /// </summary>
    [System.Serializable]
    public class ItemEntryData : IItemEntryReadOnly {
        [SerializeField]
        private ItemDefinitionBase _definition;

        [SerializeField]
        private int _quantity = 1;

        public IItemDefinition Definition => _definition;

        public string Id => null;
        // This should never be less than zero. The minimum value will always be 1
        public int Quantity => _quantity < 1 ? 1 : _quantity;
        public int CreatedAt => 0;
        public int UpdatedAt => 0;

        public void UpdateTimeLogs () {
            throw new System.NotImplementedException();
        }
    }
}
