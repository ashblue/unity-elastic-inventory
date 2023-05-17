namespace CleverCrow.Fluid.ElasticInventory {
    public class ItemEntry : IItemEntry {
        public IItemDefinition Definition { get; }
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