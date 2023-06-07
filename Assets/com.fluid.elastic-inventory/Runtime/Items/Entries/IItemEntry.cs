namespace CleverCrow.Fluid.ElasticInventory {
    /// <summary>
    /// A read only safe interface wrapper to prevent accidentally editing item entries outside of inventory instances
    /// </summary>
    public interface IItemEntryReadOnly {
        string Id { get; }
        IItemDefinition Definition { get; }
        int Quantity { get; }
    }

    public interface IItemEntry : IItemEntryReadOnly {
        void SetQuantity (int quantity);
    }
}
