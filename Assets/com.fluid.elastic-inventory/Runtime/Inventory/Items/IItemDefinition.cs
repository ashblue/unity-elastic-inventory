namespace CleverCrow.Fluid.ElasticInventory {
    public interface IItemDefinition {
        IItemEntry CreateItemEntry (int quantity = 1);
    }
}