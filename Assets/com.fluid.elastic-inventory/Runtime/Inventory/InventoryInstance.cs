using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public class InventoryInstance {
        private readonly Dictionary<IItemDefinition, IItemEntry> _entries = new Dictionary<IItemDefinition, IItemEntry>();
        private readonly IItemDatabase _database;

        public InventoryInstance (IItemDatabase database) {
            _database = database;
        }

        public IItemEntryReadOnly Get(IItemDefinition item) {
            _entries.TryGetValue(item, out var entry);

            return entry;
        }

        public IItemEntryReadOnly Add(IItemDefinition item, int quantity = 1) {
            if (item == null) return null;

            if (_entries.TryGetValue(item, out var existingEntry)) {
                existingEntry.SetQuantity(existingEntry.Quantity + quantity);
                return existingEntry;
            }

            var entry = item.CreateItemEntry(quantity);
            _entries.Add(item, entry);

            return entry;
        }

        public bool Has (IItemDefinition item, int quantity = 1) {
            var entry = Get(item);
            if (entry == null || entry.Quantity < quantity) return false;

            return true;
        }

        public void Remove (IItemDefinition item) {
            var entry = _entries[item];

            if (entry.Quantity > 1) {
                entry.SetQuantity(entry.Quantity - 1);
                return;
            }

            _entries.Remove(item);
        }

        public string Save () {
            var data = new InventorySaveData {
                items = _entries
                    .Select(e => e.Value.Save())
                    .ToList(),
            };

            return JsonUtility.ToJson(data);
        }
    }
}
