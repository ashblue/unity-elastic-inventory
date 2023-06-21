namespace CleverCrow.Fluid.ElasticInventory {
    public abstract class ItemEntryBase : IItemEntry {
        public IItemDefinition Definition { get; private set; }
        public int Quantity { get; private set; }
        public System.DateTime CreatedAt { get; private set; }
        public string Id { get; private set; }

        public void Setup (IItemDefinition definition, int quantity = 1, string id = null, System.DateTime? createdAt = null) {
            Id = id ?? System.Guid.NewGuid().ToString();
            Definition = definition;
            Quantity = quantity;
            CreatedAt = createdAt ?? System.DateTime.Now;
        }

        public void SetQuantity(int quantity) {
            Quantity = quantity;
        }

        /// <summary>
        /// Helper method to retrieve a definition as the expected type
        /// </summary>
        public T GetDefinition<T> () where T : IItemDefinition {
            // @NOTE Should be able to automatically cast this via generic class type,
            // but it creates a reciprocal reference loop with the definition
            return (T)Definition;
        }
    }
}
