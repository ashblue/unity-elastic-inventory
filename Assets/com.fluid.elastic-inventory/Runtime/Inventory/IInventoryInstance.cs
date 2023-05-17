namespace CleverCrow.Fluid.ElasticInventory {
    public interface IInventoryInstance {
        IItemEntry Get (IItemDefinition item);
        void Add (IItemDefinition item, int quantity = 1);
        // void Remove (IItemDefinition item);
        // bool Has (IItemDefinition item);

        // string Save ();
        // void Load (string save);
        // List<IItemEntry> GetAll ();
    }
}
