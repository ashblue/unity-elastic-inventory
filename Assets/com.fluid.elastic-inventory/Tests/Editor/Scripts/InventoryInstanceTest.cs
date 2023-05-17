using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class InventoryInstanceTest {
        private class Options {
            public IItemDatabase database = Substitute.For<IItemDatabase>();
        }

        private InventoryInstance Setup(IItemDatabase database = null) {
            if (database == null) {
                database = Substitute.For<IItemDatabase>();
            }

            return new InventoryInstance(database);
        }

        public class Get_Method : InventoryInstanceTest {
            [Test]
            public void It_should_get_the_item_entry_after_adding_it () {
                var item = Substitute.For<IItemDefinition>();
                var inventory = Setup();

                inventory.Add(item);
                var entry = inventory.Get(item);

                Assert.AreEqual(item, entry.Definition);
            }

            [Test]
            public void It_should_get_the_item_quantity_of_2_after_adding_it_twice () {
                var item = Substitute.For<IItemDefinition>();
                var inventory = Setup();

                inventory.Add(item);
                inventory.Add(item);
                var entry = inventory.Get(item);

                Assert.AreEqual(item, entry.Definition);
            }

            [Test]
            public void It_should_get_the_item_quantity_of_1_after_adding_it () {
                var item = Substitute.For<IItemDefinition>();
                var inventory = Setup();

                inventory.Add(item);
                var entry = inventory.Get(item);

                Assert.AreEqual(1, entry.Quantity);
            }

            [Test]
            public void It_should_not_fail_when_getting_a_missing_entry () {
                var item = Substitute.For<IItemDefinition>();
                var inventory = Setup();

                var entry = inventory.Get(item);

                Assert.IsNull(entry);
            }
        }

        public class Add_Method : InventoryInstanceTest {
            [Test]
            public void It_should_not_error_if_null () {
                var inventory = Setup();

                Assert.DoesNotThrow(() => {
                    inventory.Add(null);
                });
            }

            [Test]
            public void It_should_not_add_items_with_zero_quantity () {
                var item = Substitute.For<IItemDefinition>();
                var inventory = Setup();

                inventory.Add(item, 0);

                Assert.IsFalse(inventory.Has(item));
            }
        }

        public class Has_Method : InventoryInstanceTest {
            [Test]
            public void It_should_return_true_if_the_item_is_in_the_inventory () {
                var item = Substitute.For<IItemDefinition>();
                var inventory = Setup();

                inventory.Add(item);
                var result = inventory.Has(item);

                Assert.IsTrue(result);
            }

            [Test]
            public void It_should_return_false_if_the_item_is_not_in_the_inventory () {
                var item = Substitute.For<IItemDefinition>();
                var inventory = Setup();

                var result = inventory.Has(item);

                Assert.IsFalse(result);
            }

            [Test]
            public void It_should_return_false_if_the_item_quantity_is_not_in_the_inventory () {
                var item = Substitute.For<IItemDefinition>();
                var quantity = 2;
                var inventory = Setup();

                inventory.Add(item);
                var result = inventory.Has(item, quantity);

                Assert.IsFalse(result);
            }
        }
    }
}

