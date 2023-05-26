using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory {
    public interface IItemDefinition {
        string Id { get; }
        string DisplayName { get; }
        Sprite Image { get; }

        IItemEntry CreateItemEntry (int quantity = 1);
    }
}
