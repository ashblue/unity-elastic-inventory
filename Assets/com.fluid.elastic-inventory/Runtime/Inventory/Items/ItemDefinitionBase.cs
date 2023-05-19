namespace CleverCrow.Fluid.ElasticInventory {
    public abstract class ItemDefinitionBase : IItemDefinition {
        public virtual IItemEntry CreateItemEntry (int quantity = 1) {
            return new ItemEntry(this, quantity);
        }
    }
}
