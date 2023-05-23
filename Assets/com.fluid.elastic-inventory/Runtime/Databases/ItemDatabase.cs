using System.Collections.Generic;
using CleverCrow.Fluid.SimpleSettings;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Elastic Inventory/Item Database")]
    public class ItemDatabase : SettingsBase<ItemDatabase>, IItemDatabase {
        private readonly Dictionary<string, ItemDefinitionBase> _idToDefinition = new();
        public List<ItemDefinitionBase> _definitions = new();

        public IItemDefinition Get (string id) {
            return _idToDefinition[id];
        }

        protected override void OnSetup () {
            _definitions.ForEach(d => _idToDefinition.Add(d.Id, d));
        }
    }
}
