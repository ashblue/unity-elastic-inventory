namespace CleverCrow.Fluid.ElasticInventory.Samples {
    public class ItemDefinitionWeapon : ItemDefinitionBase {
        public override bool Unique => true;

        public override IItemEntry CreateItemEntry (int quantity = 1, string id = null) {
            var entry = new ItemEntryWeapon();
            entry.Setup(this, quantity, id);

            return entry;
        }
    }
}
