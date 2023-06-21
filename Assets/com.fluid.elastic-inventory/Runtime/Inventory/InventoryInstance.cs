using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public class InventoryInstance : IInventoryInstance {
        private readonly Dictionary<string, IItemEntry> _idToEntry = new();
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

        public IItemEntryReadOnly Get(string entryId) {
            return _idToEntry.TryGetValue(entryId, out var entry) ? entry : null;
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

            // Unique items and items without existing quantities can be added directly
            if (entry.Definition.Unique || !Has(entry.Definition)) {
                AddEntry(entry as IItemEntry);
                return entry;
            }

            return Add(entry.Definition, entry.Quantity);
        }

        private void AddEntry (IItemEntry entry) {
            if (entry.Definition.Unique) {
                _uniqueEntries.Add(entry);
            } else {
                _entries.Add(entry.Definition, entry);
            }

            _idToEntry.Add(entry.Id, entry);
        }

        public bool Has (IItemDefinition item, int quantity = 1) {
            var entry = Get(item);
            return entry != null && entry.Quantity >= quantity;
        }

        public bool Has (string entryId) {
            return _idToEntry.ContainsKey(entryId);
        }

        public void Remove (IItemDefinition item, int quantity = 1) {
            if (item.Unique) throw new System.ArgumentException("Unique items cannot be removed by definition. Use an ID instead");

            var entry = _entries[item];

            if (entry.Quantity > quantity) {
                entry.SetQuantity(entry.Quantity - quantity);
            } else {
                _entries.Remove(item);
                _idToEntry.Remove(entry.Id);
            }

            Events.ItemRemoved.Invoke(entry);
        }

        public void Remove (string entryId) {
            var entry = _idToEntry[entryId];

            if (entry.Definition.Unique) {
                _uniqueEntries.Remove(entry);
            } else {
                _entries.Remove(entry.Definition);
            }

            _idToEntry.Remove(entry.Id);

            Events.ItemRemoved.Invoke(entry);
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

        /// <summary>
        /// Get all item entries of the specified criteria
        /// </summary>
        /// <param name="definitionType">
        /// Use this to retrieve only items created from a specific definition type (Weapons, Armor, etc)
        /// </param>
        /// <param name="category">
        /// Only return items from a specific category
        /// </param>
        /// <typeparam name="T">
        /// Declare a custom item entry type to be returned. Useful for items that have an editable state
        /// </typeparam>
        /// <returns></returns>
        public List<T> GetAll<T> (
            System.Type definitionType = null,
            string category = null
        ) where T : IItemEntryReadOnly {
            var allEntries = new List<T>();
            allEntries.AddRange(_uniqueEntries.Cast<T>());
            allEntries.AddRange(_entries.Values.Cast<T>());

            if (definitionType != null) {
                allEntries = allEntries.Where(e => definitionType.IsInstanceOfType(e.Definition)).ToList();
            }

            if (category != null) {
                allEntries = allEntries.Where(e => e.Definition.Category == category).ToList();
            }

            return allEntries;
        }

        /// <summary>
        /// Simplified wrapper for GetAll that returns a non-editable item entry. See GetAll with generic type for full documentation
        /// </summary>
        public List<IItemEntryReadOnly> GetAll (
            System.Type definitionType = null,
            string category = null
        ) {
            return GetAll<IItemEntryReadOnly>(definitionType, category);
        }
    }
}
