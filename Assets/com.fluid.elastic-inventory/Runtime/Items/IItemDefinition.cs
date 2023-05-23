namespace CleverCrow.Fluid.ElasticInventory {
    public interface IItemDefinition {
        string Id { get; }

        IItemEntry CreateItemEntry (int quantity = 1);
    }
}
