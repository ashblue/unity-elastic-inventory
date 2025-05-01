using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public class ItemDatabaseTest {
        private ItemDatabaseInternal Setup (List<IItemDefinition> definitions = null) {
            if (definitions == null) definitions = new List<IItemDefinition>();
            var database = new ItemDatabaseInternal(definitions);

            return database;
        }

        public class Get_Method : ItemDatabaseTest {
            [Test]
            public void It_should_return_the_expected_item () {
                var definition = A.ItemDefinition().Build();

                var database = Setup(definitions: new List<IItemDefinition> { definition });
                var result = database.Get(definition.Id);

                Assert.AreEqual(definition, result);
            }

            [Test]
            public void It_should_not_error_if_the_item_is_not_found () {
                var database = Setup();
                var item = database.Get("test");

                Assert.IsNull(item);
            }
        }

        public class GetUniqueIndex_Method : ItemDatabaseTest {
            [Test]
            public void It_should_return_a_unique_index () {
                var database = Setup();
                var id1 = database.GetUniqueIndex();
                var id2 = database.GetUniqueIndex();

                Assert.AreNotEqual(id1, id2);
            }
        }

        public class Save_Method : ItemDatabaseTest {
            [Test]
            public void It_should_save_the_database () {
                var expected = JsonUtility.ToJson(new ItemDatabaseSaveData {
                    idIndex = 1
                });

                var database = Setup();
                database.GetUniqueIndex();
                var save = database.Save();

                Assert.AreEqual(expected, save);
            }
        }

        public class Load_Method : ItemDatabaseTest {
            [Test]
            public void It_should_restore_a_saved_database () {
                var database = Setup();
                database.GetUniqueIndex();
                var save = database.Save();

                var database2 = Setup();
                database2.Load(save);

                Assert.AreEqual(database.GetUniqueIndex(), database2.GetUniqueIndex());
            }
        }
    }
}
