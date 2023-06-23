using System;
using NSubstitute;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class ItemEntryBuilder {
        private IItemDefinition _definition = A.ItemDefinition().Build();
        private int _quantity = 1;
        private string _id = Guid.NewGuid().ToString();
        private DateTime _createdAt = DateTime.Now;
        DateTime _updatedAt = DateTime.Now;

        public IItemEntry Build () {
            var entry = Substitute.For<IItemEntry>();
            entry.Definition.Returns(_definition);
            entry.Quantity.Returns(_quantity);
            entry.Id.Returns(_id);
            entry.CreatedAt.Returns(_createdAt);
            entry.UpdatedAt.Returns(_updatedAt);

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

        public ItemEntryBuilder WithUpdatedAt (DateTime updatedAt) {
            _updatedAt = updatedAt;
            return this;
        }
    }
}
