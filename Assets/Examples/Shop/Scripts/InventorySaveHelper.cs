using System.Collections.Generic;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    public class InventorySaveHelper : MonoBehaviour {
        [SerializeField]
        List<InventoryHelper> _inventories;

        void Awake () {
            // Please note the execution order of this script. It must be executed before the inventory helper scripts
            // https://docs.unity3d.com/Manual/class-MonoManager.html
            if (!PlayerPrefs.HasKey("save")) return;

            var save = PlayerPrefs.GetString("save");
            GlobalDatabaseManager.Instance.Load(save);
        }

        public void Save () {
            _inventories.ForEach(i => i.Save());
            ItemDatabase.Current.Save();

            var save = GlobalDatabaseManager.Instance.Save();
            PlayerPrefs.SetString("save", save);

            Debug.Log("Saved");
        }

        public void DeleteSaves () {
            PlayerPrefs.DeleteKey("save");
            Debug.Log("Deleted Save Data");
        }
    }
}
