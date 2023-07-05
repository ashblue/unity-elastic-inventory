using System.Collections.Generic;
using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    public class ExamplePlayerInventory : MonoBehaviour {
        public InventoryHelper inventory;
        public List<ItemDefinitionFantasyBase> itemsToBuy;

        void Start () {
            foreach (var item in itemsToBuy) {
                inventory.Instance.Remove("gold", item.Cost);
                inventory.Instance.Add(item);
            }
        }
    }
}
