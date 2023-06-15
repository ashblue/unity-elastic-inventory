using CleverCrow.Fluid.Utilities.UnityEvents;

namespace CleverCrow.Fluid.ElasticInventory {
    public interface IInventoryEvents {
        IUnityEvent<IItemEntryReadOnly> ItemAdded { get; }
    }

    public class InventoryEvents : IInventoryEvents {
        public IUnityEvent<IItemEntryReadOnly> ItemAdded { get; set; } = new UnityEventPlus<IItemEntryReadOnly>();
    }
}
