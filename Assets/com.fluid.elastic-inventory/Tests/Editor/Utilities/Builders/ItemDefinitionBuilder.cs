using NSubstitute;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class ItemDefinitionBuilder {
        IItemEntry _itemEntry;

        public IItemDefinition Build () {
            var definition = Substitute.For<IItemDefinition>();
            
            definition.CreateItemEntry(Arg.Any<int>()).Returns(info => {
                if (_itemEntry != null) return _itemEntry;

                var entry = Substitute.For<IItemEntry>();
                entry.Definition.Returns(definition);
                entry.Quantity.Returns(info.Arg<int>());
                
                entry.When(x => x.SetQuantity(Arg.Any<int>())).Do(info => {
                    entry.Quantity.Returns(info.Arg<int>());
                });

                return entry;
            });

            return definition;
        }

        public ItemDefinitionBuilder WithItemEntry (IItemEntry entry) {
            _itemEntry = entry;
            return this;
        }
    }
}