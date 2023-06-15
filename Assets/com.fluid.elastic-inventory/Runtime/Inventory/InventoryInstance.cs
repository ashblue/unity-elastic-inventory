using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public class InventoryInstance : IInventoryInstance {
        private readonly Dictionary<IItemDefinition, IItemEntry> _entries = new();
        private readonly List<IItemEntry> _uniqueEntries = new();
        private readonly IItemDatabase _database;

        public IInventoryEvents Events { get; } = new InventoryEvents();

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
            if (item == null || quantity < 1) return null;

            IItemEntryReadOnly entry;
            if (item.Unique) {
                entry = item.CreateItemEntry();
                AddEntry((IItemEntry)entry);
            } else if (_entries.TryGetValue(item, out var existingEntry)) {
                existingEntry.SetQuantity(existingEntry.Quantity + quantity);
                entry = existingEntry;
            } else {
                entry = item.CreateItemEntry(quantity);
                AddEntry((IItemEntry)entry);
            }

            Events.ItemAdded.Invoke(entry);

            return entry;
        }

        public IItemEntryReadOnly Add(IItemEntryReadOnly entry) {
            if (entry == null) return null;

            if (entry.Definition.Unique) {
                AddEntry(entry as IItemEntry);
                return entry;
            }

            return Add(entry.Definition, entry.Quantity);
        }

        private void AddEntry (IItemEntry entry) {
            if (entry.Definition.Unique) {
                _uniqueEntries.Add(entry);
                return;
            }

            _entries.Add(entry.Definition, entry);
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
                items = GetAll<IItemEntry>()
                    .Select(e => e.Definition.DataResolver.Save(e))
                    .ToList(),
            };

            return JsonUtility.ToJson(data);
        }

        public void Load (string save) {
            var data = JsonUtility.FromJson<InventorySaveData>(save);

            // Create a fake data resolver so we can pull out the definition from the raw JSON
            var resolver = new ItemEntryDataResolver();

            data.items.ForEach(json => {
                // Create a fake object so we can get to the definition from the save data
                var tmpEntry = resolver.Load(json, _database);
                var entry = tmpEntry.Definition.DataResolver.Load(json, _database);

                AddEntry(entry);
            });
        }

        public List<T> GetAll<T> () where T : IItemEntryReadOnly {
            var allEntries = new List<T>();
            allEntries.AddRange(_uniqueEntries.Cast<T>());
            allEntries.AddRange(_entries.Values.Cast<T>());

            return allEntries;
        }

        /// <summary>
        /// Returns all unique and non-unique items in one list
        /// </summary>
        public List<IItemEntryReadOnly> GetAll () {
            return GetAll<IItemEntryReadOnly>();
        }

        public List<T> GetAllByDefinitionType<T> (System.Type type) where T : IItemEntryReadOnly {
            return GetAll<T>().Where(e => type.IsInstanceOfType(e.Definition)).ToList();
        }

        /// <summary>
        /// Get everything associated with a specific definition type (weapon, armor, etc)
        /// </summary>
        public List<IItemEntryReadOnly> GetAllByDefinitionType (System.Type type) {
            return GetAllByDefinitionType<IItemEntryReadOnly>(type);
        }
    }
}
