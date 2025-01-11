using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public abstract class ItemDefinitionBase : ScriptableObject, IItemDefinition {
        [SerializeField]
        string _id;

        public string Id => _id;
        public abstract string DisplayName { get; }
        public abstract string Category { get; }
        public virtual bool Unique => false;
        public virtual IItemEntryDataResolver DataResolver { get; } = new ItemEntryDataResolver();

        public virtual IItemEntry CreateItemEntry (IItemDatabase database, int quantity = 1, string id = null, int? createdAt = null, int? updatedAt = null) {
            var entry = new ItemEntry();
            entry.Setup(database, this, quantity, id, createdAt, updatedAt);

            return entry;
        }

        /// <summary>
        /// Maps the category index to the database category list. Works with the editor and runtime.
        ///
        /// Categories on generic items are saved using an index that maps to the database category list. If you change the name of the corresponding category index it will change the category name of every associated item.
        /// </summary>
        protected string GetCategoryByIndex (int index) {
            IItemDatabase database = null;

            if (Application.isPlaying) {
                database = ItemDatabase.Current;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying) {
                database = Resources.Load<ItemDatabase>("ItemDatabase");

                if (database == null) {
                    Debug.LogWarning("Could not find ItemDatabase. Please create one at `Resources/ItemDatabase'.");
                    return "Default";
                }
            }
#endif

            if (database.Categories.Count <= index) {
                Debug.LogWarning($"Category index {index} is out of range. Please check your database. Returning `Default' category.");
                return "Default";
            }

            return database.Categories[index];
        }
    }
}
