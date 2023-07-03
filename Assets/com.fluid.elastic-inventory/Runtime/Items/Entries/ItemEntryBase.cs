namespace CleverCrow.Fluid.ElasticInventory {
    public abstract class ItemEntryBase : IItemEntry {
        IItemDatabase _database;

        public IItemDefinition Definition { get; private set; }
        public int Quantity { get; private set; }
        public int CreatedAt { get; private set; }
        public string Id { get; private set; }
        public int UpdatedAt { get; private set; }

        public void Setup (
            IItemDatabase database,
            IItemDefinition definition,
            int quantity = 1,
            string id = null,
            int? createdAt = null,
            int? lastUpdatedAt = null
        ) {
            _database = database;
            Id = id ?? System.Guid.NewGuid().ToString();
            Definition = definition;
            Quantity = quantity;
            CreatedAt = createdAt ?? database.GetUniqueIndex();
            UpdatedAt = lastUpdatedAt ?? database.GetUniqueIndex();
        }

        public void SetQuantity(int quantity) {
            Quantity = quantity;
            UpdateTimeLogs();
        }

        /// <summary>
        /// Helper method to retrieve a definition as the expected type
        /// </summary>
        public T GetDefinition<T> () where T : IItemDefinition {
            // @NOTE Should be able to automatically cast this via generic class type,
            // but it creates a reciprocal reference loop with the definition
            return (T)Definition;
        }

        /// <summary>
        /// Manually call this whenever you change you change any values on a custom entry to update timestamps
        /// </summary>
        public void UpdateTimeLogs () {
            UpdatedAt = _database.GetUniqueIndex();
        }
    }
}
