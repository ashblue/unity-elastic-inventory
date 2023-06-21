namespace CleverCrow.Fluid.ElasticInventory.Samples {
    [ItemDefinitionDetails("Weapon")]
    public class ItemDefinitionWeapon : ItemDefinitionBase {
        public override bool Unique => true;
        public override IItemEntryDataResolver DataResolver => new ItemEntryDataResolverWeapon();

        // Please note this DOES NOT automatically add the category to the dropdown of available categories
        // To do that you must add the category to the ItemDatabase
        // You don't need to do this if you're not planning to make this category available to swappable category items
        public override string Category => "Weapon";

        public override IItemEntry CreateItemEntry (int quantity = 1, string id = null, System.DateTime? createdAt = null) {
            var entry = new ItemEntryWeapon();
            entry.Setup(this, quantity, id, createdAt);

            return entry;
        }
    }
}
