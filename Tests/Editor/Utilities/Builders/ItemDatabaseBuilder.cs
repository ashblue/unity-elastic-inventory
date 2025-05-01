using NSubstitute;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class ItemDatabaseBuilder {
        public IItemDatabase Build () {
            return Substitute.For<IItemDatabase>();
        }
    }
}
