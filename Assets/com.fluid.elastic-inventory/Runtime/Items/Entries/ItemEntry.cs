namespace CleverCrow.Fluid.ElasticInventory {
    // @TODO Item entry will need to come from a base class with overridable methods
    // This way the save, load, and additional fields can be handled on a case by case basis
    public class ItemEntry : IItemEntry {
        public IItemDefinition Definition { get; }
        public int Quantity { get; private set; }
        public string Id { get; }

        public ItemEntry(IItemDefinition definition, int quantity = 1, string id = null) {
            Id = id ?? System.Guid.NewGuid().ToString();
            Definition = definition;
            Quantity = quantity;
        }

        public void SetQuantity(int quantity) {
            Quantity = quantity;
        }
    }
}
