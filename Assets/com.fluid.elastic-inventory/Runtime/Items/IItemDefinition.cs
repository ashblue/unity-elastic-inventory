namespace CleverCrow.Fluid.ElasticInventory {
    public interface IItemDefinition {
        string Id { get; }
        string DisplayName { get; }

        IItemEntry CreateItemEntry (int quantity = 1);
    }
}
