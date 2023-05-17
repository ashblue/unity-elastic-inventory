using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public class InventoryInstance {
        private readonly Dictionary<IItemDefinition, IItemEntry> _entries = new Dictionary<IItemDefinition, IItemEntry>();
        private readonly IItemDatabase _database;

        public InventoryInstance (IItemDatabase database) {
            _database = database;
        }

        public IItemEntry Get(IItemDefinition item) {
            _entries.TryGetValue(item, out var entry);
            return entry;
        }

        public void Add(IItemDefinition item, int quantity = 1) {
            if (item == null || quantity < 1) return;

            if (_entries.TryGetValue(item, out var existingEntry)) {
                existingEntry.SetQuantity(existingEntry.Quantity + quantity);
                return;
            }

            _entries.Add(item, new ItemEntry(item, quantity));
        }

        public bool Has (IItemDefinition item, int quantity = 1) {
            var entry = Get(item);
            if (entry == null || entry.Quantity < quantity) return false;
            
            return true;
        }
    }
}
