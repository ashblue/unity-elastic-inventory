using System.Collections.Generic;
using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    [ItemDefinitionDetails("Recipe")]
    public class Recipes : ItemDefinitionFantasyBase {
        [SerializeField]
        List<ItemEntryData> _ingredients;

        [SerializeField]
        ItemDefinitionFantasyBase _result;

        public override string Category => "Recipe";
        public List<ItemEntryData> Ingredients => _ingredients;
        public ItemDefinitionFantasyBase Result => _result;

        // Call this to verify ingredients are present in the inventory before running
        public bool HasIngredients (IInventoryInstance inventory) {
            foreach (var ingredient in _ingredients) {
                if (!inventory.Has(ingredient.Definition, ingredient.Quantity)) {
                    return false;
                }
            }

            return true;
        }

        public void ConvertIngredientsToItem (IInventoryInstance inventory) {
            foreach (var ingredient in _ingredients) {
                inventory.Remove(ingredient.Definition, ingredient.Quantity);
            }

            inventory.Add(_result);
        }
    }
}
