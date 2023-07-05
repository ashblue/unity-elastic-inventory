using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    public class CustomEntrySaveLoadTest : MonoBehaviour {
        public InventoryHelper inventory;
        public ItemDefinitionWeapon weapon;
        public int changeDurability = 22;

        private void Start () {
            // Create the item and modify the durability
            var entry = inventory.Instance.Add(weapon) as ItemEntryWeapon;
            Debug.Log("New item durability: " + entry.Durability);

            entry.Durability = changeDurability;
            Debug.Log("Changed item durability: " + entry.Durability);

            // Load the item from save data
            var save = inventory.Instance.Save();
            var newInventory = new InventoryInstance(ItemDatabase.Current);
            newInventory.Load(save);

            // Print the durability of the loaded item
            var restoredItem = newInventory.GetEntry(entry.Id) as ItemEntryWeapon;
            Debug.Log("Restored item durability: " + restoredItem.Durability);
        }
    }
}
