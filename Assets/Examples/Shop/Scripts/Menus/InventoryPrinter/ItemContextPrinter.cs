using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    public class ItemContextPrinter : MonoBehaviour {
        [SerializeField]
        private TMPro.TextMeshProUGUI _title;

        [SerializeField]
        private TMPro.TextMeshProUGUI _description;

        void Start () {
            _title.text = "";
            _description.text = "";
        }

        public void Set (ItemDefinitionFantasyBase definition) {
            _title.text = definition.DisplayName;

            var description = definition.Description;

            if (definition is ItemDefinitionWeapon weapon) {
                description += $"\n\n- Damage: {weapon.Damage}\n- Energy Cost: {weapon.EnergyCost}";
            } else if (definition is HealthPotion potion) {
                description += $"\n\nHeal Amount: {potion.HealAmount}";
            }

            _description.text = description;
        }
    }
}
