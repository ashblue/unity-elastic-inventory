namespace CleverCrow.Fluid.ElasticInventory {
    public abstract class ItemEntryBase : IItemEntry {
        public IItemDefinition Definition { get; private set; }
        public int Quantity { get; private set; }
        public string Id { get; private set; }

        public void Setup (IItemDefinition definition, int quantity = 1, string id = null) {
            Id = id ?? System.Guid.NewGuid().ToString();
            Definition = definition;
            Quantity = quantity;
        }

        public void SetQuantity(int quantity) {
            Quantity = quantity;
        }
    }
}
