using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.SimpleSettings;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    /// <summary>
    /// The item database manages all of your item definitions. It's important to know that it needs to be saved
    /// and loaded between sessions. This is because it generates unique IDs for each item definition. If you don't
    /// items will get incorrect created at and modified at indices.
    ///
    /// Note that when initially called with ItemDatabase.Current, it will automatically generate a new instance at runtime
    /// if one doesn't already exist
    /// </summary>
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Elastic Inventory/Item Database")]
    public class ItemDatabase : SettingsBase<ItemDatabase>, IItemDatabase {
        ItemDatabaseInternal _internal;

        [Tooltip("Automatically load save data from the global database manager at initial creation. Should only disable if you want to implement your own save and load system for items")]
        [SerializeField]
        bool _autoLoad = true;

        [Tooltip("DO NOT edit this directly. Must be public for Unity serialization purposes")]
        public List<ItemDefinitionBase> _definitions = new();

        [Tooltip("Add new categories here. You can also use this to filter items in the inventory UI")]
        [SerializeField]
        List<string> _categories = new() {
            "Default",
        };

        public List<string> Categories => _categories;

        protected override void OnSetup () {
            _internal = new ItemDatabaseInternal(_definitions.Cast<IItemDefinition>().ToList());
            if (_autoLoad) Load();
        }

        public IItemDefinition Get (string id) {
            return _internal.Get(id);
        }

        /// <summary>
        /// Generates a unique index every time it's called incrementing by 1. This is used to generate new index IDs on the fly
        /// </summary>
        public int GetUniqueIndex () {
            return _internal.GetUniqueIndex();
        }

        /// <summary>
        /// Helper method to save to the global database manager
        /// </summary>
        public void Save () {
            var save = SaveManual();
            GlobalDatabaseManager.Instance.Database.Strings.Set("ItemDatabase", save);
        }

        /// <summary>
        /// Save method so you can handle the save data however you want
        /// </summary>
        public string SaveManual () {
            return _internal.Save();
        }

        /// <summary>
        /// Helper method to load from the global database manager
        /// </summary>
        public void Load () {
            var save = GlobalDatabaseManager.Instance.Database.Strings.Get("ItemDatabase");
            if (save != null) LoadManual(save);
        }

        /// <summary>
        /// Load method so you can inject save data manually
        /// </summary>
        public void LoadManual (string save) {
            _internal.Load(save);
        }
    }


    public class ItemDatabaseInternal {
        readonly Dictionary<string, IItemDefinition> _idToDefinition;
        int _idIndex;

        public ItemDatabaseInternal (List<IItemDefinition> definitions) {
            _idToDefinition = definitions.ToDictionary(d => d.Id);
        }

        public int GetUniqueIndex () {
            var currentIndex = _idIndex;
            _idIndex++;
            return currentIndex;
        }

        public string Save () {
            return JsonUtility.ToJson(new ItemDatabaseSaveData {
                idIndex = _idIndex
            });
        }

        public void Load (string save) {
            var data = JsonUtility.FromJson<ItemDatabaseSaveData>(save);
            _idIndex = data.idIndex;
        }

        public IItemDefinition Get (string id) {
            if (!_idToDefinition.ContainsKey(id)) return null;

            return _idToDefinition[id];
        }
    }
}
