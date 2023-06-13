using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public class InventoryInstance : IInventoryInstance {
        // @TODO Entry keys will need to support a random ID instead so multiple item entries can be supported (like gear)
        // Or we will need a separate list for storing unique item entries?
        private readonly Dictionary<IItemDefinition, IItemEntry> _entries = new();
        private readonly List<IItemEntry> _uniqueEntries = new();
        private readonly IItemDatabase _database;

        public InventoryInstance (IItemDatabase database) {
            _database = database;
        }

        public IItemEntryReadOnly Get(IItemDefinition item) {
            _entries.TryGetValue(item, out var entry);
            return entry ?? _uniqueEntries.FirstOrDefault(e => e.Definition == item);
        }

        public IItemEntryReadOnly GetUnique(string id) {
            return _uniqueEntries.FirstOrDefault(e => e.Id == id);
        }

        public IItemEntryReadOnly Add(IItemDefinition item, int quantity = 1) {
            if (item == null) return null;

            if (item.Unique) {
                var unique = item.CreateItemEntry();
                _uniqueEntries.Add(unique);

                return unique;
            }

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

        public bool HasUnique (string id) {
            var entry = _uniqueEntries.FirstOrDefault(e => e.Id == id);
            return entry != null;
        }

        public void Remove (IItemDefinition item, int quantity = 1) {
            if (item.Unique) throw new System.ArgumentException("Unique items cannot be removed by definition. Use an ID instead");

            var entry = _entries[item];

            if (entry.Quantity > quantity) {
                entry.SetQuantity(entry.Quantity - quantity);
                return;
            }

            _entries.Remove(item);
        }

        public void RemoveUnique (string id) {
            _uniqueEntries.Remove(_uniqueEntries.First(e => e.Id == id));
        }

        public string Save () {
            var data = new InventorySaveData {
                items = _entries
                    .Select(e => new ItemEntrySaveData {
                        definitionId = e.Value.Definition.Id,
                        quantity = e.Value.Quantity,
                    })
                    .ToList(),
            };

            return JsonUtility.ToJson(data);
        }

        public void Load (string save) {
            var data = JsonUtility.FromJson<InventorySaveData>(save);

            data.items.ForEach(entryData => {
                var definition = _database.Get(entryData.definitionId);
                var entry = definition.CreateItemEntry(entryData.quantity);

                _entries.Add(definition, entry);
            });
        }

        public List<IItemEntryReadOnly> GetAll () {
            var allEntries = new List<IItemEntryReadOnly>();
            allEntries.AddRange(_uniqueEntries);
            allEntries.AddRange(_entries.Values.ToList<IItemEntryReadOnly>());

            return allEntries;
        }
    }
}
