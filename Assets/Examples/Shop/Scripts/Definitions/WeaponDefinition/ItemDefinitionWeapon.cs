﻿using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    [ItemDefinitionDetails("Weapon")]
    public class ItemDefinitionWeapon : ItemDefinitionFantasyBase {
        [SerializeField]
        private int _damage;

        [SerializeField]
        private int _energyCost;

        public override bool Unique => true;
        public override IItemEntryDataResolver DataResolver => new ItemEntryDataResolverWeapon();

        // Please note this DOES NOT automatically add the category to the dropdown of available categories
        // To do that you must add the category to the ItemDatabase
        // You don't need to do this if you're not planning to make this category available to the category dropdown
        public override string Category => "Weapon";
        public int Damage => _damage;
        public int EnergyCost => _energyCost;

        public override IItemEntry CreateItemEntry (int quantity = 1, string id = null, System.DateTime? createdAt = null, System.DateTime? updatedAt = null) {
            var entry = new ItemEntryWeapon();
            entry.Setup(this, quantity, id, createdAt, updatedAt);

            return entry;
        }
    }
}
