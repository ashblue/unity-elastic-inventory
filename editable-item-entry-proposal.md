# Editable Item Entry Proposal

To create an editable an item that saves and loads you'll need to extend the ItemEntryBase class to include your editable fields. We will also need to create our custom definition and connect it.

```csharp
// This is the data layer that is safe to edit
public class CustomItemEntry : ItemEntryBase {
    public override bool Unique { get; } = true;

    public int Level { get; set; } = 1;
    public int Durability { get; set; } = 1000;
}

// Our definition should be readonly, don't edit it at runtime
public class CustomItemDefinition : ItemDefinitionBase {
    override ItemEntryBase ItemEntry { get; } = Type(CustomItemEntry);
}
```

Next you will need to extend an ItemEntryDataResolverBase to include your custom item entry fields. Then determine how to manage them on save and load from the `CustomItemEntry` class we created.

**PLEASE NOTE**: This object you're creating will automatically call Unity's [`JsonUtility.ToJson` and `JsonUtility.FromJson` methods](https://docs.unity3d.com/2021.2/Documentation/Manual/JSONSerialization.html) to save and load your data. So you'll need to make sure that your data is serializable friendly or this will silently fail on save.

```csharp
// This base class we're extending will automatically handle serializing fields like the definition ID, unique ID, quantity, ect.
public class CustomItemEntryDataResolver : ItemEntryDataResolverBase<CustomItemEntry> {
    public int level;
    public int durability;
    
    public override void OnSave (CustomItemEntry itemEntry) {
        level = itemEntry.Level;
        durability = itemEntry.Durability;
    }
    
    public override void OnLoad (CustomItemEntry itemEntry) {
        itemEntry.Level = level;
        itemEntry.Durability = durability;
    }
}
```

Lastly you need to inform your `CustomItemEntry` to use the `CustomItemEntryDataResolver` we just created. This is done by overriding the `DataResolver` property and setting it to the `CustomItemEntryDataResolver` type.

```csharp
public class CustomItemEntry : ItemEntryBase {
    ...
    override CustomItemEntryDataResolver DataResolver { get; } = Type(CustomItemEntryDataResolver);
}
```

That's it. Your items created from this definition will now automatically save and load as expected. To get the item entry so you can easily edit it. You can call the `InventoryInstance` class's generic type methods `Get<T>(definition)`, `Get<T>(id)`, `GetAll<T>()`, `Add<T>(definition)`, ect.

```csharp
var inventory = // Creation code goes here for new InventoryInstance
var editableItem = inventory.Get<CustomItemEntry>("my item ID");
editableItem.Level = 2;
editableItem.Durability = 500;
```
