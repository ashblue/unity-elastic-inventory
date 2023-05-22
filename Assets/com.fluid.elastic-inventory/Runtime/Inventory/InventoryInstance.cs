using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public class InventoryInstance : IInventoryInstance {
        // @TODO Entry keys will need to support a random ID instead so multiple item entries can be supported (like gear)
        // Or we will need a separate list for storing unique item entries?
        private readonly Dictionary<IItemDefinition, IItemEntry> _entries = new();
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
            return entry != null && entry.Quantity >= quantity;
        }

        public void Remove (IItemDefinition item, int quantity = 1) {
            var entry = _entries[item];

            if (entry.Quantity > quantity) {
                entry.SetQuantity(entry.Quantity - quantity);
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

        public void Load (string save) {
            var data = JsonUtility.FromJson<InventorySaveData>(save);

            data.items.ForEach(itemSave => {
                var entryData = JsonUtility.FromJson<ItemEntrySaveData>(itemSave);
                var definition = _database.Get(entryData.definitionId);
                var entry = definition.CreateItemEntry();
                entry.Load(itemSave, _database);

                _entries.Add(definition, entry);
            });
        }
    }
}
