using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    /// <summary>
    /// A universal interface to handle saving and loading item entries
    /// </summary>
    public interface IItemEntryDataResolver {
        string Save (IItemEntryReadOnly entry);
        IItemEntry Load (string json, IItemDatabase database);
    }

    public abstract class ItemEntryDataResolverBase<T> : IItemEntryDataResolver where T : IItemEntryReadOnly {
        public string definitionId;
        public string uniqueId;
        public int quantity;

        public string Save (IItemEntryReadOnly entry) {
            Reset();

            definitionId = entry.Definition.Id;
            uniqueId = entry.Id;
            quantity = entry.Quantity;

            OnSave((T)entry);

            return JsonUtility.ToJson(this);
        }

        public IItemEntry Load (string json, IItemDatabase database) {
            JsonUtility.FromJsonOverwrite(json, this);

            var entry = database.Get(definitionId).CreateItemEntry(quantity, uniqueId);
            OnLoad((T)entry);

            return entry;
        }

        private void Reset () {
            definitionId = null;
            uniqueId = null;
            quantity = 0;

            OnReset();
        }

        protected virtual void OnSave (T entry) {
        }

        protected virtual void OnLoad (T entry) {
        }

        protected virtual void OnReset () {
        }
    }
}
