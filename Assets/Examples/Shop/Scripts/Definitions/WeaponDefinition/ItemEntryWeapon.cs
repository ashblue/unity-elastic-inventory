using CleverCrow.Fluid.ElasticInventory;

namespace CleverCrow.Fluid.Examples {
    /// <summary>
    /// Declare editable fields for spawned item instances
    /// </summary>
    public class ItemEntryWeapon : ItemEntryBase {
        public int Level { get; set; } = 1;
        public int Durability { get; set; } = 1000;
    }
}
