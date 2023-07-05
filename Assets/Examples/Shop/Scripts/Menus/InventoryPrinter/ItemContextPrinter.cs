using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CleverCrow.Fluid.Examples {
    public class ItemContextPrinter : MonoBehaviour {
        [SerializeField]
        private TMPro.TextMeshProUGUI _title;

        [SerializeField]
        private TMPro.TextMeshProUGUI _description;

        [SerializeField]
        Button _purchaseButton;

        public UnityEvent<IItemEntryReadOnly> OnPurchase { get; } = new();

        void Start () {
            Clear();
        }

        public void Clear () {
            _title.text = "";
            _description.text = "";
            if (_purchaseButton != null) _purchaseButton.gameObject.SetActive(false);
        }

        public void Set (IItemEntryReadOnly entry) {
            var definition = entry.Definition as ItemDefinitionFantasyBase;

            _title.text = definition.DisplayName;
            var description = definition.Description;
            description += $"\n\n- Cost: {definition.Cost}";

            if (definition is ItemDefinitionWeapon weapon) {
                description += $"\n- Damage: {weapon.Damage}\n- Energy Cost: {weapon.EnergyCost}";
            } else if (definition is HealthPotion potion) {
                description += $"\n- Heal Amount: {potion.HealAmount}";
            }

            _description.text = description;

            if (_purchaseButton != null) {
                _purchaseButton.gameObject.SetActive(true);
                _purchaseButton.onClick.RemoveAllListeners();
                _purchaseButton.onClick.AddListener(() => OnPurchase.Invoke(entry));
            }
        }
    }
}
