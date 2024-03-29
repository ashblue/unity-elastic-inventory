using NSubstitute;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class ItemEntryReadOnlyBuilder {
        IItemDefinition _definition = A.ItemDefinition().Build();
        int _quantity = 1;

        public IItemEntryReadOnly Build () {
            var entry = Substitute.For<IItemEntryReadOnly>();

            entry.Definition.Returns(_definition);
            entry.Quantity.Returns(_quantity);
            entry.Id.Returns(System.Guid.NewGuid().ToString());
            entry.CreatedAt.Returns(0);
            entry.UpdatedAt.Returns(0);

            return entry;
        }

        public ItemEntryReadOnlyBuilder WithDefinition (IItemDefinition definition) {
            _definition = definition;
            return this;
        }

        public ItemEntryReadOnlyBuilder WithQuantity (int quantity) {
            _quantity = quantity;
            return this;
        }
    }
}
