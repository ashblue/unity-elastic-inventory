namespace CleverCrow.Fluid.ElasticInventory {
    /// <summary>
    /// A read only safe interface wrapper to prevent accidentally editing item entries outside of inventory instances
    /// </summary>
    public interface IItemEntryReadOnly {
        string Id { get; }
        IItemDefinition Definition { get; }
        int Quantity { get; }
        System.DateTime CreatedAt { get; }
    }

    public interface IItemEntry : IItemEntryReadOnly {
        /// <summary>
        /// Only called once for you automatically. DO NOT call this for any reason in your code. The sky will fall.
        /// </summary>
        void Setup (IItemDefinition definition, int quantity = 1, string id = null, System.DateTime? createdAt = null);

        T GetDefinition<T> () where T : IItemDefinition;
        void SetQuantity (int quantity);
    }
}
