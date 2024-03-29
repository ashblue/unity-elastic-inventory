namespace CleverCrow.Fluid.ElasticInventory {
    public interface IItemDefinition {
        /// <summary>
        /// A unique ID generated at the creation of this entry. It will persist on save and load.
        /// </summary>
        string Id { get; }

        string DisplayName { get; }

        string Category { get; }

        /// <summary>
        /// Unique items will always have their own unique entry with a quantity of 1. Please note
        /// you cannot stack or adjust the quantity of these items
        /// </summary>
        bool Unique { get; }

        IItemEntryDataResolver DataResolver { get; }

        IItemEntry CreateItemEntry (
            IItemDatabase database,
            int quantity = 1,
            string id = null,
            int? createdAt = null,
            int? updatedAt = null
        );
    }
}
