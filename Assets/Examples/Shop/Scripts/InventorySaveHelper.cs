using System.Collections.Generic;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    /// <summary>
    /// Please note this script must be executed before the inventory helper and item database scripts
    /// You'll need to manually set the index in the script execution order window
    /// https://docs.unity3d.com/Manual/class-MonoManager.html
    /// </summary>
    public class InventorySaveHelper : MonoBehaviour {
        const string SAVE_KEY = "GLOBAL_DATABASE_SAVE";

        [SerializeField]
        List<InventoryHelper> _inventories;

        void Awake () {
            if (!PlayerPrefs.HasKey(SAVE_KEY)) return;

            var save = PlayerPrefs.GetString(SAVE_KEY);
            GlobalDatabaseManager.Instance.Load(save);
        }

        public void Save () {
            _inventories.ForEach(i => i.Save());
            ItemDatabase.Current.Save();

            var save = GlobalDatabaseManager.Instance.Save();
            PlayerPrefs.SetString(SAVE_KEY, save);

            Debug.Log("Saved");
        }

        public void DeleteSaves () {
            PlayerPrefs.DeleteKey(SAVE_KEY);
            Debug.Log("Deleted Save Data");
        }
    }
}
