using System;
using NSubstitute;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class ItemDefinitionBuilder {
        IItemEntry _itemEntry;
        string _id = System.Guid.NewGuid().ToString();
        bool _unique;
        string _category = "Generic";
        string _displayName = "Item";

        public IItemDefinition Build () {
            var definition = Substitute.For<IItemDefinition>();

            definition.Id.Returns(_id);
            definition.Unique.Returns(_unique);
            definition.Category.Returns(_category);
            definition.DisplayName.Returns(_displayName);

            _itemEntry?.Definition.Returns(definition);
            definition.CreateItemEntry(Arg.Any<int>(), Arg.Any<string>())
                .Returns(info => _itemEntry ?? A.ItemEntry()
                    .WithDefinition(definition)
                    .WithQuantity(info.Arg<int>())
                    .WithId(info.Arg<string>())
                    .WithCreatedAt(DateTime.Now)
                    .Build()
                );

            definition.CreateItemEntry(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<DateTime>())
                .Returns(info => _itemEntry ?? A.ItemEntry()
                    .WithDefinition(definition)
                    .WithQuantity(info.Arg<int>())
                    .WithId(info.Arg<string>())
                    .WithCreatedAt(info.Arg<DateTime>())
                    .Build()
                );

            var dataResolver = new ItemEntryDataResolver();
            definition.DataResolver.Returns(dataResolver);

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

        public ItemDefinitionBuilder WithCategory (string category) {
            _category = category;
            return this;
        }

        public ItemDefinitionBuilder WithDisplayName (string displayName) {
            _displayName = displayName;
            return this;
        }
    }
}
