namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class ItemEntryBuilder {
        private IItemDefinition _definition;
        private int _quantity = 1;
        private string _id;

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

        public ItemEntry Build () {
            var entry = new ItemEntry();
            entry.Setup(_definition, _quantity, _id);

            return entry;
        }
    }
}
