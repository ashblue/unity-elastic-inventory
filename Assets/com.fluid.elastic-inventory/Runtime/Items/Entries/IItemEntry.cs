namespace CleverCrow.Fluid.ElasticInventory {
    /// <summary>
    /// A read only safe interface wrapper to prevent accidentally editing item entries outside of inventory instances
    /// </summary>
    public interface IItemEntryReadOnly {
        string Id { get; }
        IItemDefinition Definition { get; }
        int Quantity { get; }
        int CreatedAt { get; }
        int UpdatedAt { get; }

        void UpdateTimeLogs ();
    }

    public interface IItemEntry : IItemEntryReadOnly {
        /// <summary>
        /// Only called once for you automatically. DO NOT call this for any reason in your code. The sky will fall.
        /// </summary>
        void Setup (
            IItemDatabase database,
            IItemDefinition definition,
            int quantity = 1,
            string id = null,
            int? createdAt = null,
            int? updatedAt = null
        );

        T GetDefinition<T> () where T : IItemDefinition;
        void SetQuantity (int quantity);
    }
}
