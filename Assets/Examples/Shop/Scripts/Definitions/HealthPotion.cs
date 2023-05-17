using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    [ItemDefinitionDetails("Potion")]
    public class HealthPotion : ItemDefinitionFantasyBase {
        [SerializeField]
        private int _healAmount;

        public override string Category => "Potion";
        public int HealAmount => _healAmount;
    }
}
