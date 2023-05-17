using System.Collections.Generic;

namespace CleverCrow.Fluid.ElasticInventory {
    public interface IInventoryInstance {
        IInventoryEvents Events { get; }

        IItemEntryReadOnly Get (IItemDefinition item);
        IItemEntryReadOnly Get (string definitionId);
        IItemEntryReadOnly GetEntry (string entryId);

        List<IItemEntryReadOnly> GetAll (System.Type definitionType = null, string category = null);
        List<T> GetAll<T> (System.Type definitionType = null, string category = null) where T : IItemEntryReadOnly;

        IItemEntryReadOnly Add (IItemDefinition item, int quantity = 1);
        IItemEntryReadOnly Add (string definitionId, int quantity = 1);
        IItemEntryReadOnly AddEntry (IItemEntryReadOnly entry);

        bool Has (IItemDefinition item, int quantity = 1);
        bool Has (string definitionId, int quantity = 1);
        bool HasEntry (string entryId);

        void Remove (IItemDefinition item, int quantity = 1);
        void Remove (string definitionId, int quantity = 1);
        void RemoveEntry (string entryId);

        void Sort (
            List<IItemEntryReadOnly> items,
            ItemSort sort = ItemSort.CreatedAt,
            ItemOrder order = ItemOrder.Ascending,
            ItemSort sortSecondary = ItemSort.Alphabetical,
            ItemOrder orderSecondary = ItemOrder.Ascending,
            List<CategorySort> customCategory = null
        );

        string Save ();
        void Load (string save);
    }
}
