using System;

namespace CleverCrow.Fluid.ElasticInventory {
    [AttributeUsage(AttributeTargets.Class)]
    public class ItemDefinitionDetailsAttribute : Attribute {
        public string DisplayName { get; private set; }

        public ItemDefinitionDetailsAttribute (string displayName) {
            DisplayName = displayName;
        }
    }
}
