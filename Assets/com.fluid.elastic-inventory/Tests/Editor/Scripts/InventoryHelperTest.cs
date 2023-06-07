using System.Collections.Generic;
using CleverCrow.Fluid.Databases;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class InventoryHelperTest {
        private InventoryHelperInternal Setup (
            string id = "id",
            List<IItemEntryReadOnly> startingItems = null,
            IItemDatabase itemDatabase = null,
            IDatabaseInstance globalDatabase = null
        ) {
            if (startingItems == null) startingItems = new List<IItemEntryReadOnly>();
            if (itemDatabase == null) itemDatabase = Substitute.For<IItemDatabase>();
            if (globalDatabase == null) globalDatabase = Substitute.For<IDatabaseInstance>();

            return new InventoryHelperInternal(id, startingItems, itemDatabase, globalDatabase);
        }

        public class Init : InventoryHelperTest {
            [Test]
            public void It_should_add_a_list_of_item_entries_to_the_an_inventory_instance () {
                var startingItems = new List<IItemEntryReadOnly> {
                    A.ItemEntryReadOnly().Build(),
                    A.ItemEntryReadOnly().Build(),
                    A.ItemEntryReadOnly().Build(),
                };

                var helper = Setup(startingItems: startingItems);
                var inventory = helper.Instance;

                Assert.AreEqual(3, inventory.GetAll().Count);

                foreach (var item in startingItems) {
                    Assert.AreEqual(item.Definition, inventory.Get(item.Definition).Definition);
                    Assert.AreEqual(item.Quantity, inventory.Get(item.Definition).Quantity);
                }
            }

            [Test]
            public void It_should_overwrite_items_if_data_exists_in_the_global_database () {
                var instanceId = "inventory_instance";

                var item = A.ItemDefinition().Build();
                var itemDatabase = Substitute.For<IItemDatabase>();
                itemDatabase.Get(item.Id).Returns(item);

                var existingInventory = new InventoryInstance(itemDatabase);
                existingInventory.Add(item);
                var save = existingInventory.Save();

                var globalDatabase = Substitute.For<IDatabaseInstance>();
                globalDatabase.Strings.Get(instanceId).Returns(save);

                var helper = Setup(id: instanceId, itemDatabase: itemDatabase, globalDatabase: globalDatabase);

                Assert.AreEqual(1, helper.Instance.GetAll().Count);
                Assert.AreEqual(item, helper.Instance.Get(item).Definition);
            }

            [Test]
            public void It_should_not_load_items_on_top_of_existing_items () {
                var instanceId = "inventory_instance";
                var startingItems = new List<IItemEntryReadOnly> {
                    A.ItemEntryReadOnly().Build(),
                };

                var item = A.ItemDefinition().Build();
                var itemDatabase = Substitute.For<IItemDatabase>();
                itemDatabase.Get(item.Id).Returns(item);

                var existingInventory = new InventoryInstance(itemDatabase);
                existingInventory.Add(item);
                var save = existingInventory.Save();

                var globalDatabase = Substitute.For<IDatabaseInstance>();
                globalDatabase.Strings.Get(instanceId).Returns(save);

                var helper = Setup(id: instanceId, startingItems: startingItems, itemDatabase: itemDatabase, globalDatabase: globalDatabase);

                Assert.AreEqual(1, helper.Instance.GetAll().Count);
                Assert.AreEqual(item, helper.Instance.Get(item).Definition);
            }
        }

        public class SaveMethod : InventoryHelperTest {
            [Test]
            public void It_should_save_the_current_inventory_data_to_the_database () {
                var instanceId = "inventory_instance";
                var globalDatabase = Substitute.For<IDatabaseInstance>();
                var item = A.ItemDefinition().Build();
                var itemDatabase = Substitute.For<IItemDatabase>();
                itemDatabase.Get(item.Id).Returns(item);

                var helper = Setup(id: instanceId, itemDatabase: itemDatabase, globalDatabase: globalDatabase);
                helper.Instance.Add(item);
                helper.Save();

                globalDatabase.Strings.Received(1).Set(instanceId, helper.Instance.Save());
            }
        }
    }
}
