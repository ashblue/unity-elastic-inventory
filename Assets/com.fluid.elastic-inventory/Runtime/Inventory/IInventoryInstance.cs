using System.Collections.Generic;

namespace CleverCrow.Fluid.ElasticInventory {
    public interface IInventoryInstance {
        IInventoryEvents Events { get; }

        IItemEntryReadOnly Get (IItemDefinition item);
        IItemEntryReadOnly Get (string entryId);
        List<IItemEntryReadOnly> GetAll (System.Type definitionType = null, string category = null);
        List<T> GetAll<T> (System.Type definitionType = null, string category = null) where T : IItemEntryReadOnly;

        IItemEntryReadOnly Add (IItemDefinition item, int quantity = 1);
        IItemEntryReadOnly Add (IItemEntryReadOnly entry);

        bool Has (IItemDefinition item, int quantity = 1);
        bool Has (string entryId);

        void Remove (IItemDefinition item, int quantity = 1);
        void Remove (string entryId);

        string Save ();
        void Load (string save);
    }
}
