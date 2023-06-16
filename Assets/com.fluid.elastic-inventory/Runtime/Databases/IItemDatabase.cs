using System.Collections.Generic;

namespace CleverCrow.Fluid.ElasticInventory {
    public interface IItemDatabase {
        List<string> Categories { get; }

        IItemDefinition Get (string id);
    }
}
