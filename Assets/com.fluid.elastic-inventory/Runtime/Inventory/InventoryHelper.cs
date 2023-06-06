using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    /// <summary>
    /// This wrapper class automatically manages a simple implementation of an inventory instance for you with an injected database.
    /// It's designed to only include the minimal logic required to get an inventory up and running. If you need more control, it's
    /// recommended to write logic around this class or re-implement instead of overriding it
    /// </summary>
    public class InventoryHelper : MonoBehaviour {
        InventoryHelperInternal _internal;

        [SerializeField]
        string _id = Guid.NewGuid().ToString();

        [SerializeField]
        List<ItemEntryData> _startingItems;

        public IInventoryInstance Instance => _internal.Instance;

        void Start () {
            _internal = new InventoryHelperInternal(_id, _startingItems.ToList<IItemEntryReadOnly>(), ItemDatabase.Current, GlobalDatabaseManager.Instance.Database);
        }
    }

    public class InventoryHelperInternal {
        public IInventoryInstance Instance { get; }

        public InventoryHelperInternal (string id, List<IItemEntryReadOnly> startingItems, IItemDatabase itemDatabase, IDatabaseInstance globalDatabase) {
            Instance = new InventoryInstance(itemDatabase);

            var save = globalDatabase.Strings.Get(id);
            if (!string.IsNullOrEmpty(save)) {
                Instance.Load(save);
            } else {
                startingItems.ForEach(i => Instance.Add(i.Definition, i.Quantity));
            }
        }
    }
}
