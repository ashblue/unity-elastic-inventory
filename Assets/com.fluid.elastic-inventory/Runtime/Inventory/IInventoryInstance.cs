using System.Collections.Generic;

namespace CleverCrow.Fluid.ElasticInventory {
    public interface IInventoryInstance {
        IInventoryEvents Events { get; }

        IItemEntryReadOnly Get (IItemDefinition item);
        IItemEntryReadOnly GetUnique (string id);
        List<IItemEntryReadOnly> GetAll (System.Type definitionType = null, string category = null);
        List<T> GetAll<T> (System.Type definitionType = null, string category = null) where T : IItemEntryReadOnly;

        IItemEntryReadOnly Add (IItemDefinition item, int quantity = 1);
        IItemEntryReadOnly Add (IItemEntryReadOnly entry);

        bool Has (IItemDefinition item, int quantity = 1);
        bool HasUnique (string id);

        void Remove (IItemDefinition item, int quantity = 1);
        void RemoveUnique (string id);

        string Save ();
        void Load (string save);
    }
}
