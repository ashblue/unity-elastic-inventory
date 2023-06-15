using System.Collections.Generic;

namespace CleverCrow.Fluid.ElasticInventory {
    public interface IInventoryInstance {
        IInventoryEvents Events { get; }

        IItemEntryReadOnly Get (IItemDefinition item);
        IItemEntryReadOnly GetUnique (string id);
        List<IItemEntryReadOnly> GetAll ();
        List<T> GetAll<T> () where T : IItemEntryReadOnly;
        List<T> GetAllByDefinitionType<T> (System.Type type) where T : IItemEntryReadOnly;
        List<IItemEntryReadOnly> GetAllByDefinitionType (System.Type type);

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
