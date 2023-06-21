using System;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class ItemEntryBuilder {
        private IItemDefinition _definition = A.ItemDefinition().Build();
        private int _quantity = 1;
        private string _id;
        private DateTime _createdAt = DateTime.Now;

        public ItemEntry Build () {
            var entry = new ItemEntry();
            entry.Setup(_definition, _quantity, _id, _createdAt);

            return entry;
        }

        public ItemEntryBuilder WithDefinition (IItemDefinition definition) {
            _definition = definition;
            return this;
        }

        public ItemEntryBuilder WithQuantity (int quantity) {
            _quantity = quantity;
            return this;
        }

        public ItemEntryBuilder WithId (string id) {
            _id = id;
            return this;
        }

        public ItemEntryBuilder WithCreatedAt (DateTime createdAt) {
            _createdAt = createdAt;
            return this;
        }
    }
}
