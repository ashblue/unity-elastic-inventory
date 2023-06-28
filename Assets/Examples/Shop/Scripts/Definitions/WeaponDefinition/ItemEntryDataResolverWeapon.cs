using CleverCrow.Fluid.ElasticInventory;

namespace CleverCrow.Fluid.Examples {
    /// <summary>
    /// Handle the saving and loading of editable fields for a weapon entry instance
    /// </summary>
    [System.Serializable]
    public class ItemEntryDataResolverWeapon : ItemEntryDataResolverBase<ItemEntryWeapon> {
        public int level;
        public int durability;

        protected override void OnSave (ItemEntryWeapon itemEntry) {
            level = itemEntry.Level;
            durability = itemEntry.Durability;
        }

        protected override void OnLoad (ItemEntryWeapon itemEntry) {
            itemEntry.Level = level;
            itemEntry.Durability = durability;
        }

        protected override void OnReset () {
            level = 0;
            durability = 0;
        }
    }
}
