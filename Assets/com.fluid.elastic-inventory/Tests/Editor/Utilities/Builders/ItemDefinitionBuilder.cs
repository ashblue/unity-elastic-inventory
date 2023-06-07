using NSubstitute;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class ItemDefinitionBuilder {
        IItemEntry _itemEntry;
        string _id = System.Guid.NewGuid().ToString();
        bool _unique;

        public IItemDefinition Build () {
            var definition = Substitute.For<IItemDefinition>();

            definition.Id.Returns(_id);
            definition.Unique.Returns(_unique);

            definition.CreateItemEntry(Arg.Any<int>())
                .Returns(info => _itemEntry ?? new ItemEntry(definition, info.Arg<int>()));

            return definition;
        }

        public ItemDefinitionBuilder WithItemEntry (IItemEntry entry) {
            _itemEntry = entry;
            return this;
        }

        public ItemDefinitionBuilder WithId (string id) {
            _id = id;
            return this;
        }

        public ItemDefinitionBuilder WithUnique (bool unique) {
            _unique = unique;
            return this;
        }
    }
}
