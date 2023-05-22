namespace CleverCrow.Fluid.ElasticInventory {
    // @TODO Item entry will need to come from a base class with overridable methods
    // This way the save, load, and additional fields can be handled on a case by case basis
    public class ItemEntry : IItemEntry {
        public IItemDefinition Definition { get; private set; }
        public int Quantity { get; private set; }

        public ItemEntry(IItemDefinition definition, int quantity = 1) {
            Definition = definition;
            Quantity = quantity;
        }

        public void SetQuantity(int quantity) {
            Quantity = quantity;
        }
    }
}
