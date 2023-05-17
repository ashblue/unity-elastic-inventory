namespace CleverCrow.Fluid.ElasticInventory {
    public interface IItemEntry {
        IItemDefinition Definition { get; }
        int Quantity { get; }

        void SetQuantity (int quantity);
    }
}
    