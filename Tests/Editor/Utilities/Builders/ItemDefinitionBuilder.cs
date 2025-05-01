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

            definition.CreateItemEntry(
                Arg.Any<IItemDatabase>(),
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<int?>(),
                Arg.Any<int?>()
            ).Returns(info => {
                if (_itemEntry == null) {
                    var entry = new ItemEntry();
                    IItemDatabase db = info.ArgAt<IItemDatabase>(0);
                    int arg0 = info.ArgAt<int>(1);
                    string arg1 = info.ArgAt<string>(2);
                    int? arg2 = info.ArgAt<int?>(3);
                    int? arg3 = info.ArgAt<int?>(4);

                    if (!arg2.HasValue && !arg3.HasValue) {
                        entry.Setup(db, definition, arg0, arg1);
                    } else if (!arg3.HasValue) {
                        entry.Setup(db, definition, arg0, arg1, arg2.Value);
                    } else {
                        entry.Setup(db, definition, arg0, arg1, arg2.Value, arg3.Value);
                    }

                    return entry;
                }

                return _itemEntry;
            });

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
