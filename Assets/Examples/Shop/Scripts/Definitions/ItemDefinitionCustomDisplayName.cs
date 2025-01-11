using CleverCrow.Fluid.ElasticInventory;

namespace CleverCrow.Fluid.Examples {
    /// <summary>
    /// Simulates creating a custom display name and category for an item definition
    /// </summary>
    [ItemDefinitionDetails("Custom Display Details")]
    public class ItemDefinitionCustomDisplayName : ItemDefinitionBase {
        public override string DisplayName => "Custom Display Name";
        public override string Category => "Custom Category";
    }
}
