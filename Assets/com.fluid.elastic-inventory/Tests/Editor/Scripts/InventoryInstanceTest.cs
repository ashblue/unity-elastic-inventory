using System;
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

        public class Get_Method {
            public class DefaultItems : InventoryInstanceTest {
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

            public class UniqueItems : InventoryInstanceTest {
                [Test]
                public void It_should_return_an_item_instance_by_id_after_adding_it () {
                    var item = A.ItemDefinition().WithUnique(true).Build();
                    var inventory = Setup();

                    var instance = inventory.Add(item);
                    var result = inventory.GetEntry(instance.Id);

                    Assert.AreEqual(instance, result);
                }

                [Test]
                public void It_should_return_an_item_instance_by_definition_after_adding_it () {
                    var item = A.ItemDefinition().WithUnique(true).Build();
                    var inventory = Setup();

                    var instance = inventory.Add(item);
                    var result = inventory.Get(item);

                    Assert.AreEqual(instance, result);
                }
            }
        }

        public class Add_Method {
            public class Default_Inventory_Items : InventoryInstanceTest {
                [Test]
                public void It_should_not_error_if_null () {
                    var inventory = Setup();

                    Assert.DoesNotThrow(() => {
                        inventory.AddEntry(null);
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
                    var database = A.ItemDatabase().Build();

                    var inventory = Setup(database);
                    inventory.Add(item, 2);

                    item.Received().CreateItemEntry(database, 2);
                }

                [Test]
                public void Adding_zero_items_should_not_add_any_items () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();

                    var instance = inventory.Add(item, 0);

                    Assert.IsNull(instance);
                }

                [Test]
                public void It_should_add_an_item_entry_object () {
                    var entry = A.ItemEntry().WithQuantity(2).Build();

                    var inventory = Setup();
                    inventory.AddEntry(entry);
                    var result = inventory.Get(entry.Definition);

                    Assert.AreEqual(entry.Quantity, result.Quantity);
                }
            }

            public class ReadOnly_Items : InventoryInstanceTest {
                [Test]
                public void It_should_not_error_when_a_read_only_item_is_passed () {
                    var inventory = Setup();
                    var item = A.ItemEntryReadOnly().Build();

                    Assert.DoesNotThrow(() => {
                        inventory.AddEntry(item);
                    });
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

                [Test]
                public void It_should_add_a_unique_item_entry_as_an_individual_entry () {
                    var definition = A.ItemDefinition().WithUnique(true).Build();
                    var entry = A.ItemEntry().WithDefinition(definition).Build();
                    var inventory = Setup();

                    inventory.AddEntry(entry);
                    var result = inventory.GetEntry(entry.Id);

                    Assert.AreEqual(entry, result);
                }
            }

            public class Events : InventoryInstanceTest {
                [Test]
                public void It_should_trigger_an_event_with_the_item_entry_when_an_item_is_added () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();
                    var eventTriggered = false;

                    inventory.Events.ItemAdded.AddListener((entry) => {
                        eventTriggered = true;
                        Assert.AreEqual(item, entry.Definition);
                    });

                    inventory.Add(item);

                    Assert.IsTrue(eventTriggered);
                }

                [Test]
                public void It_should_trigger_an_event_when_a_unique_item_is_added () {
                    var item = A.ItemDefinition().WithUnique(true).Build();
                    var inventory = Setup();
                    var eventTriggered = false;

                    inventory.Events.ItemAdded.AddListener((entry) => {
                        eventTriggered = true;
                        Assert.AreEqual(item, entry.Definition);
                    });

                    inventory.Add(item);

                    Assert.IsTrue(eventTriggered);
                }

                [Test]
                public void It_should_trigger_an_event_when_items_are_added_to_an_existing_entry () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();
                    var eventTriggered = false;

                    inventory.Add(item);
                    inventory.Events.ItemAdded.AddListener((entry) => {
                        eventTriggered = true;
                        Assert.AreEqual(item, entry.Definition);
                    });

                    inventory.Add(item);

                    Assert.IsTrue(eventTriggered);
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
                    var result = inventory.HasEntry(entry.Id);

                    Assert.IsTrue(result);
                }

                [Test]
                public void It_should_return_true_if_the_item_is_in_the_inventory_by_definition () {
                    var item = A.ItemDefinition().WithUnique(true).Build();
                    var inventory = Setup();

                    inventory.Add(item);
                    var result = inventory.Has(item);

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
                    inventory.RemoveEntry(instance.Id);

                    Assert.IsFalse(inventory.HasEntry(instance.Id));
                }

                [Test]
                public void It_should_throw_an_error_if_you_try_to_remove_a_unique_item_by_definition () {
                    var definition = A.ItemDefinition().WithUnique(true).Build();
                    var inventory = Setup();

                    Assert.Throws<ArgumentException>(() => inventory.Remove(definition));
                }
            }

            public class Events : InventoryInstanceTest {
                [Test]
                public void It_should_trigger_a_remove_event_when_removing_an_item () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();
                    var eventTriggered = false;

                    inventory.Events.ItemRemoved.AddListener((entry) => {
                        eventTriggered = true;
                        Assert.AreEqual(item, entry.Definition);
                    });

                    inventory.Add(item);
                    inventory.Remove(item);

                    Assert.IsTrue(eventTriggered);
                }

                [Test]
                public void It_should_trigger_a_remove_event_when_removing_a_single_item_in_a_stack () {
                    var item = A.ItemDefinition().Build();
                    var inventory = Setup();
                    var eventTriggered = false;

                    inventory.Events.ItemRemoved.AddListener((entry) => {
                        eventTriggered = true;
                        Assert.AreEqual(item, entry.Definition);
                    });

                    inventory.Add(item, 10);
                    inventory.Remove(item, 1);

                    Assert.IsTrue(eventTriggered);
                }

                [Test]
                public void It_should_trigger_a_remove_event_when_removing_a_unique_item () {
                    var item = A.ItemDefinition().WithUnique(true).Build();
                    var inventory = Setup();
                    var eventTriggered = false;

                    inventory.Events.ItemRemoved.AddListener((entry) => {
                        eventTriggered = true;
                        Assert.AreEqual(item, entry.Definition);
                    });

                    var instance = inventory.Add(item);
                    inventory.RemoveEntry(instance.Id);

                    Assert.IsTrue(eventTriggered);
                }
            }
        }

        public class GetAll_Method {
            public class Defaults : InventoryInstanceTest {
                [Test]
                public void It_should_return_all_added_item_entries () {
                    var definition = A.ItemDefinition().Build();
                    var inventory = Setup();

                    inventory.Add(definition);
                    var entries = inventory.GetAll();

                    Assert.AreEqual(1, entries.Count);
                }

                [Test]
                public void It_should_get_all_items_by_definition_type () {
                    var definition = A.ItemDefinition().Build();
                    var inventory = Setup();

                    inventory.Add(definition);
                    var entries = inventory.GetAll(typeof(IItemDefinition));

                    Assert.AreEqual(1, entries.Count);
                }

                [Test]
                public void It_should_get_all_items_by_category () {
                    var category = "Weapon";
                    var definitionA = A.ItemDefinition().WithCategory(category).Build();
                    var definitionB = A.ItemDefinition().Build();
                    var inventory = Setup();

                    inventory.Add(definitionA);
                    inventory.Add(definitionB);
                    var entries = inventory.GetAll(category: category);

                    Assert.AreEqual(1, entries.Count);
                    Assert.AreEqual(definitionA, entries[0].Definition);
                }
            }

            public class WithCustomData : InventoryInstanceTest {
                public class TEST_ItemEntryCustom : ItemEntryBase {
                }

                public class TEST_ItemEntryDataResolverCustom : ItemEntryDataResolverBase<TEST_ItemEntryCustom> {
                }

                public class TEST_ItemDefinitionCustom : ItemDefinitionBase {
                    public override string DisplayName { get; }
                    public override string Category { get; }

                    public override IItemEntryDataResolver DataResolver => new TEST_ItemEntryDataResolverCustom();

                    public override IItemEntry CreateItemEntry (IItemDatabase database, int quantity = 1, string id = null, int? createdAt = null, int? updatedAt = null) {
                        // We have to initialize a new implementation of the entry here
                        // This is because the database doesn't know about our custom entry type
                        var entry = new TEST_ItemEntryCustom();
                        entry.Setup(database, this, quantity, id, createdAt, updatedAt);

                        return entry;
                    }
                }

                [Test]
                public void It_should_not_fail_on_a_custom_item_entry_type () {
                    var definitionGeneric = A.ItemDefinition().Build();
                    var definition = ScriptableObject.CreateInstance<TEST_ItemDefinitionCustom>();
                    var inventory = Setup();

                    inventory.Add(definitionGeneric);
                    inventory.Add(definition);

                    Assert.DoesNotThrow(() => {
                        var entries = inventory.GetAll<TEST_ItemEntryCustom>();
                        Assert.AreEqual(1, entries.Count);
                    });
                }
            }
        }

        public class Sort_Method : InventoryInstanceTest {
            [Test]
            public void It_should_sort_the_items_by_date_added_from_most_recent_to_latest () {
                var entryA = A.ItemEntry().WithCreatedAt(0).Build();
                var entryB = A.ItemEntry().WithCreatedAt(1).Build();
                var order = ItemOrder.Descending;
                var sort = ItemSort.CreatedAt;

                var inventory = Setup();
                inventory.AddEntry(entryA);
                inventory.AddEntry(entryB);
                var items = inventory.GetAll();
                inventory.Sort(items, sort, order);

                Assert.AreEqual(entryB, items[0]);
                Assert.AreEqual(entryA, items[1]);
            }

            [Test]
            public void It_should_sort_the_items_alphabetically_by_definition_name () {
                var definitionA = A.ItemDefinition().WithDisplayName("A").Build();
                var entryA = A.ItemEntry().WithDefinition(definitionA).Build();

                var definitionB = A.ItemDefinition().WithDisplayName("B").Build();
                var entryB = A.ItemEntry().WithDefinition(definitionB).Build();

                var order = ItemOrder.Ascending;
                var sort = ItemSort.Alphabetical;

                var inventory = Setup();
                inventory.AddEntry(entryA);
                inventory.AddEntry(entryB);
                var items = inventory.GetAll();
                inventory.Sort(items, sort, order);

                Assert.AreEqual(entryA, items[0]);
                Assert.AreEqual(entryB, items[1]);
            }

            [Test]
            public void It_should_sort_items_by_category () {
                var categoryA = "B";
                var definitionA = A.ItemDefinition().WithCategory(categoryA).Build();
                var entryA = A.ItemEntry().WithDefinition(definitionA).Build();

                var categoryB = "A";
                var definitionB = A.ItemDefinition().WithCategory(categoryB).Build();
                var entryB = A.ItemEntry().WithDefinition(definitionB).Build();

                var order = ItemOrder.Ascending;
                var sort = ItemSort.Category;

                var inventory = Setup();
                inventory.AddEntry(entryA);
                inventory.AddEntry(entryB);
                var items = inventory.GetAll();
                inventory.Sort(items, sort, order);

                Assert.AreEqual(entryA, items[1]);
                Assert.AreEqual(entryB, items[0]);
            }

            [Test]
            public void It_should_sort_by_category_then_alphabetically () {
                var category = "A";
                var definitionA = A.ItemDefinition().WithCategory(category).WithDisplayName("B").Build();
                var entryA = A.ItemEntry().WithDefinition(definitionA).Build();

                var definitionB = A.ItemDefinition().WithCategory(category).WithDisplayName("A").Build();
                var entryB = A.ItemEntry().WithDefinition(definitionB).Build();

                var order = ItemOrder.Ascending;
                var sort = ItemSort.Category;
                var sortSecondary = ItemSort.Alphabetical;
                var orderSecondary = ItemOrder.Ascending;

                var inventory = Setup();
                inventory.AddEntry(entryA);
                inventory.AddEntry(entryB);
                var items = inventory.GetAll();
                inventory.Sort(items, sort, order, sortSecondary, orderSecondary);

                Assert.AreEqual(entryA, items[1]);
                Assert.AreEqual(entryB, items[0]);
            }

            [Test]
            public void It_should_handle_a_custom_category_order_when_sorting () {
                var categoryA = "A";
                var definitionA = A.ItemDefinition().WithCategory(categoryA).Build();
                var entryA = A.ItemEntry().WithDefinition(definitionA).Build();

                var categoryB = "B";
                var definitionB = A.ItemDefinition().WithCategory(categoryB).Build();
                var entryB = A.ItemEntry().WithDefinition(definitionB).Build();

                var customCategorySort = new List<CategorySort> {
                    new(categoryA, 1),
                    new(categoryB, 0),
                };

                var order = ItemOrder.Ascending;
                var sort = ItemSort.Category;
                var sortSecondary = ItemSort.Alphabetical;
                var orderSecondary = ItemOrder.Ascending;

                var inventory = Setup();
                inventory.AddEntry(entryA);
                inventory.AddEntry(entryB);
                var items = inventory.GetAll();
                inventory.Sort(items, sort, order, sortSecondary, orderSecondary, customCategorySort);
            }

            [Test]
            public void It_should_sort_items_by_updated_at () {
                var entryA = A.ItemEntry().WithUpdatedAt(0).Build();
                var entryB = A.ItemEntry().WithUpdatedAt(1).Build();
                var order = ItemOrder.Descending;
                var sort = ItemSort.UpdatedAt;

                var inventory = Setup();
                inventory.AddEntry(entryA);
                inventory.AddEntry(entryB);
                var items = inventory.GetAll();
                inventory.Sort(items, sort, order);

                Assert.AreEqual(entryB, items[0]);
                Assert.AreEqual(entryA, items[1]);
            }
        }

        public class Save_Method : InventoryInstanceTest {
            [Test]
            public void It_should_save_the_inventory_entries_to_a_string () {
                var id = "a";
                var quantity = 1;
                var definition = A.ItemDefinition().WithId(id).Build();

                var inventory = Setup();
                var entry = inventory.Add(definition, quantity);
                var save = inventory.Save();

                // Must be below the add due to the unique ID
                var expectedSaveData = new InventorySaveData {
                    items = new List<string> {
                        entry.Definition.DataResolver.Save(entry),
                    },
                };
                var expectedSave = JsonUtility.ToJson(expectedSaveData);

                Assert.IsNotEmpty(save);
                Assert.AreEqual(expectedSave, save);
            }
        }

        public class Load_Method : InventoryInstanceTest {
            [Test]
            public void It_should_load_inventory_items_from_a_save () {
                var id = "a";
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

            [Test]
            public void It_should_load_unique_inventory_items_from_a_save () {
                var id = "a";
                var definition = A.ItemDefinition().WithUnique(true).WithId(id).Build();

                var database = Substitute.For<IItemDatabase>();
                database.Get(id).Returns(definition);

                var inventory = Setup(database);
                var oldItem = inventory.Add(definition);
                var save = inventory.Save();

                var newInventory = Setup(database);
                newInventory.Load(save);

                Assert.IsNotNull(newInventory.GetEntry(oldItem.Id));
            }

            [Test]
            public void It_should_restore_multiple_unique_entries () {
                var id = "a";
                var definition = A.ItemDefinition().WithUnique(true).WithId(id).Build();

                var database = Substitute.For<IItemDatabase>();
                database.Get(id).Returns(definition);

                var inventory = Setup(database);
                var oldItem = inventory.Add(definition);
                var oldItem2 = inventory.Add(definition);
                var save = inventory.Save();

                var newInventory = Setup(database);
                newInventory.Load(save);

                Assert.IsNotNull(newInventory.GetEntry(oldItem.Id));
                Assert.IsNotNull(newInventory.GetEntry(oldItem2.Id));
            }

            [Test]
            public void It_should_restore_entry_CreatedAt_timestamps () {
                var id = "a";

                var definition = A.ItemDefinition().Build();
                var entry = A.ItemEntry()
                    .WithId(id)
                    .WithCreatedAt(-1)
                    .WithDefinition(definition)
                    .Build();

                var database = Substitute.For<IItemDatabase>();
                database.Get(definition.Id).Returns(definition);

                var inventory = Setup(database);
                inventory.AddEntry(entry);
                var save = inventory.Save();

                var newInventory = Setup(database);
                newInventory.Load(save);
                var loadedEntry = newInventory.GetEntry(id);

                Assert.AreEqual(entry.CreatedAt.ToString(), loadedEntry.CreatedAt.ToString());
            }
        }
    }
}

