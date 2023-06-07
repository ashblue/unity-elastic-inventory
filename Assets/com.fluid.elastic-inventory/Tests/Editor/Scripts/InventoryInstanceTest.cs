using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class InventoryInstanceTest {

        private InventoryInstance Setup(IItemDatabase database = null) {
            if (database == null) {
                database = Substitute.For<IItemDatabase>();
            }

            return new InventoryInstance(database);
        }

        public class Get_Method : InventoryInstanceTest {
            [Test]
            public void It_should_get_the_item_entry_after_adding_it () {
                var item = A.ItemDefinition().Build();
                var inventory = Setup();

                inventory.Add(item);
                var entry = inventory.Get(item);

                Assert.AreEqual(item, entry.Definition);
            }

            [Test]
            public void It_should_get_the_item_quantity_of_2_after_adding_it_twice () {
                var item = A.ItemDefinition().Build();
                var inventory = Setup();

                inventory.Add(item);
                inventory.Add(item);
                var entry = inventory.Get(item);

                Assert.AreEqual(item, entry.Definition);
            }

            [Test]
            public void It_should_get_the_item_quantity_of_1_after_adding_it () {
                var item = A.ItemDefinition().Build();
                var inventory = Setup();

                inventory.Add(item);
                var entry = inventory.Get(item);

                Assert.AreEqual(1, entry.Quantity);
            }

            [Test]
            public void It_should_not_fail_when_getting_a_missing_entry () {
                var item = A.ItemDefinition().Build();
                var inventory = Setup();

                var entry = inventory.Get(item);

                Assert.IsNull(entry);
            }
        }

        public class Add_Method {
            public class Default_Inventory_Items : InventoryInstanceTest {
                [Test]
                public void It_should_not_error_if_null () {
                    var inventory = Setup();

                    Assert.DoesNotThrow(() => {
                        inventory.Add(null);
                    });
                }

                [Test]
                public void It_should_not_add_items_with_zero_quantity () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();

                    inventory.Add(item, 0);

                    Assert.IsFalse(inventory.Has(item));
                }

                [Test]
                public void It_should_add_an_item_entry_returned_from_the_definition_create_item_entry_method () {
                    var itemEntry = Substitute.For<IItemEntry>();
                    var item = A.ItemDefinition().WithItemEntry(itemEntry).Build();

                    var inventory = Setup();
                    inventory.Add(item);
                    var result = inventory.Get(item);

                    Assert.AreEqual(itemEntry, result);
                }

                [Test]
                public void It_should_call_create_item_entry_with_the_passed_quantity () {
                    var item = A.ItemDefinition().Build();

                    var inventory = Setup();
                    inventory.Add(item, 2);

                    item.Received().CreateItemEntry(2);
                }
            }

            public class Unique_Items : InventoryInstanceTest {
                [Test]
                public void It_should_add_two_unique_items_as_individual_entries () {
                    var definition = A.ItemDefinition().WithUnique(true).Build();
                    var inventory = Setup();

                    inventory.Add(definition);
                    inventory.Add(definition);

                    Assert.AreEqual(2, inventory.GetAll().Count);
                }
            }
        }

        public class Has_Method {
            public class DefaultItems : InventoryInstanceTest {
                [Test]
                public void It_should_return_true_if_the_item_is_in_the_inventory () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();

                    inventory.Add(item);
                    var result = inventory.Has(item);

                    Assert.IsTrue(result);
                }

                [Test]
                public void It_should_return_false_if_the_item_is_not_in_the_inventory () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();

                    var result = inventory.Has(item);

                    Assert.IsFalse(result);
                }

                [Test]
                public void It_should_return_false_if_the_item_quantity_is_not_in_the_inventory () {
                    var item = A.ItemDefinition().Build();
                    var quantity = 2;
                    var inventory = Setup();

                    inventory.Add(item);
                    var result = inventory.Has(item, quantity);

                    Assert.IsFalse(result);
                }
            }

            public class UniqueItems : InventoryInstanceTest {
                [Test]
                public void It_should_return_true_if_the_item_is_in_the_inventory () {
                    var item = A.ItemDefinition().WithUnique(true).Build();
                    var inventory = Setup();

                    var entry = inventory.Add(item);
                    var result = inventory.Has(entry.Id);

                    Assert.IsTrue(result);
                }
            }
        }

        public class Remove_Method {
            public class DefaultItems : InventoryInstanceTest {
                [Test]
                public void It_should_remove_the_item_entry () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();

                    inventory.Add(item);
                    inventory.Remove(item);

                    Assert.IsFalse(inventory.Has(item));
                }

                [Test]
                public void It_should_reduce_the_item_count_when_removing_two () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();

                    inventory.Add(item, 2);
                    inventory.Remove(item);

                    Assert.IsTrue(inventory.Has(item));
                }

                [Test]
                public void It_should_remove_multiple_items () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();

                    inventory.Add(item, 2);
                    inventory.Remove(item, 2);

                    Assert.IsFalse(inventory.Has(item));
                }

                [Test]
                public void It_should_remove_the_item_entry_when_removing_all_items_of_two_quantity () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();

                    inventory.Add(item, 2);
                    inventory.Remove(item, 2);

                    Assert.IsFalse(inventory.Has(item, 0));
                }
            }

            public class UniqueItems : InventoryInstanceTest {
                [Test]
                public void It_should_remove_the_item_by_entry_id () {
                    var definition = A.ItemDefinition().WithUnique(true).Build();
                    var inventory = Setup();

                    var instance = inventory.Add(definition);
                    inventory.Remove(instance.Id);

                    Assert.IsFalse(inventory.Has(instance.Id));
                }
            }
        }

        public class GetAll_Method : InventoryInstanceTest {
            [Test]
            public void It_should_return_all_added_item_entries () {
                var definition = A.ItemDefinition().Build();
                var inventory = Setup();

                inventory.Add(definition);
                var entries = inventory.GetAll();

                Assert.AreEqual(1, entries.Count);
            }
        }

        public class Save_Method : InventoryInstanceTest {
            [Test]
            public void It_should_save_the_inventory_entries_to_a_string () {
                var id = "a";
                var quantity = 1;
                var definition = A.ItemDefinition().WithId(id).Build();

                var saveData = new InventorySaveData {
                    items = new List<ItemEntrySaveData> {
                        new() {
                            definitionId = id,
                            quantity = quantity,
                        },
                    },
                };
                var expectedSave = JsonUtility.ToJson(saveData);

                var inventory = Setup();
                inventory.Add(definition, quantity);
                var save = inventory.Save();

                Assert.IsNotEmpty(save);
                Assert.AreEqual(expectedSave, save);
            }
        }

        public class Load_Method : InventoryInstanceTest {
            [Test]
            public void It_should_load_inventory_items_from_a_save () {
                var id = "a";
                var quantity = 1;
                var definition = A.ItemDefinition().WithId(id).Build();

                var database = Substitute.For<IItemDatabase>();
                database.Get(id).Returns(definition);

                var inventory = Setup(database);
                inventory.Add(definition);
                var save = inventory.Save();

                var newInventory = Setup(database);
                newInventory.Load(save);

                Assert.IsTrue(newInventory.Has(definition));
            }
        }
    }
}

