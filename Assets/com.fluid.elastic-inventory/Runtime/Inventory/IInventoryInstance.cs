namespace CleverCrow.Fluid.ElasticInventory {
    public interface IInventoryInstance {
        IItemEntryReadOnly Get (IItemDefinition item);
        IItemEntryReadOnly Add (IItemDefinition item, int quantity = 1);
        void Remove (IItemDefinition item, int quantity = 1);
        bool Has (IItemDefinition item, int quantity = 1);
        string Save ();
        // void Load (string save);
        // List<IItemEntry> GetAll ();
    }
}
