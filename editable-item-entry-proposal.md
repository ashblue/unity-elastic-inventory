# Editable Item Entry Proposal

To create an editable an item that saves and loads you'll need to extend the ItemEntryBase class to include your editable fields. We will also need to create our weapon definition and connect it.

```csharp
// This is the data layer that is safe to edit at runtime
public class ItemEntryWeapon : ItemEntryBase<ItemDefinitionWeapon> {
    public override bool Unique { get; } = true;

    public int Level { get; set; } = 1;
    public int Durability { get; set; } = 1000;
}

// Our definition should have readonly data (static), never edit it at runtime
public class ItemDefinitionWeapon : ItemDefinitionBase {
    override protected ItemEntryBase ItemEntry { get; } = Type(ItemEntryWeapon);
}
```

Next you will need to extend an ItemEntryDataResolverBase to include your weapon item entry fields. Then determine how to manage them on save and load from the `ItemEntryWeapon` class we created.

**PLEASE NOTE**: This object you're creating will automatically call Unity's [`JsonUtility.ToJson` and `JsonUtility.FromJson` methods](https://docs.unity3d.com/2021.2/Documentation/Manual/JSONSerialization.html) to save and load your data. So you'll need to make sure that your data is serializable friendly or this will silently fail on save.

```csharp
// This base class we're extending will automatically handle serializing fields like the definition ID, unique ID, quantity, ect.
public class ItemEntryDataResolverWeapon : ItemEntryDataResolverBase<ItemEntryWeapon> {
    public int level;
    public int durability;
    
    protected override void OnSave (ItemEntryWeapon itemEntry) {
        level = itemEntry.Level;
        durability = itemEntry.Durability;
    }
    
    protected override void OnLoad (ItemEntryWeapon itemEntry) {
        itemEntry.Level = level;
        itemEntry.Durability = durability;
    }
}
```

Lastly you need to inform your `ItemEntryWeapon` to use the `ItemEntryDataResolverWeapon` we just created. This is done by overriding the `DataResolver` property and setting it to the `ItemEntryDataResolverWeapon` type.

Add the `DataResolver` override to your existing `ItemEntryWeapon` code you created earlier.

```csharp
public class ItemEntryWeapon : ItemEntryBase {
    protected override ItemEntryDataResolverWeapon DataResolver { get; } = Type(ItemEntryDataResolverWeapon);
    
    ...
}
```

That's it. Your items created from this definition will now automatically save and load as expected. To get the item entry so you can easily edit it. Call the `InventoryInstance` class's generic type methods `Get<T>(definition)`, `Get<T>(id)`, `GetAll<T>()`, `Add<T>(definition)`, ect.

```csharp
var inventory = new InventoryInstance(InventoryDatabase.Current);
inventory.Add(ItemDefinitionWeapon_instance);
var editableItem = inventory.Get<ItemEntryWeapon>("my item ID");
editableItem.Level = 2;
editableItem.Durability = 500;
```
