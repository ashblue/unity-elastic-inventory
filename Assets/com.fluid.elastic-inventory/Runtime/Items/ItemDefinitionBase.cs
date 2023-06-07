using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public abstract class ItemDefinitionBase : ScriptableObject, IItemDefinition {
        [SerializeField]
        string _id;

        [SerializeField]
        string _displayName;

        [SerializeField]
        Sprite _image;

        public string Id => _id;
        public string DisplayName => _displayName;
        public Sprite Image => _image;
        public bool Unique => false;

        public virtual IItemEntry CreateItemEntry (int quantity = 1, string id = null) {
            return new ItemEntry(this, quantity, id);
        }
    }
}
