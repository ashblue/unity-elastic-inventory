using CleverCrow.Fluid.Utilities.UnityEvents;

namespace CleverCrow.Fluid.ElasticInventory {
    public interface IInventoryEvents {
        IUnityEvent<IItemEntryReadOnly> ItemAdded { get; }
        IUnityEvent<IItemEntryReadOnly> ItemRemoved { get; }
        IUnityEvent<IItemEntryReadOnly> ItemChanged { get; }
    }

    public class InventoryEvents : IInventoryEvents {
        public IUnityEvent<IItemEntryReadOnly> ItemAdded { get; } = new UnityEventPlus<IItemEntryReadOnly>();
        public IUnityEvent<IItemEntryReadOnly> ItemRemoved { get; } = new UnityEventPlus<IItemEntryReadOnly>();
        public IUnityEvent<IItemEntryReadOnly> ItemChanged { get; } = new UnityEventPlus<IItemEntryReadOnly>();
    }
}
