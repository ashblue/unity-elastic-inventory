using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.Fluid.Examples {
    public class ItemCategoryButton : MonoBehaviour {
        const float _ACTIVE_OPACITY = 1f;
        const float _INACTIVE_OPACITY = 0.5f;

        [SerializeField]
        string _category;

        [SerializeField]
        Button _button;

        [SerializeField]
        Image _image;

        public void Setup (InventoryPrinter inventoryPrinter) {
            _button.onClick.AddListener(() => {
                inventoryPrinter.SetCategory(_category);
                inventoryPrinter.ClearCategoryActive();
                SetActive(true);
            });
        }

        public void SetActive (bool active) {
            _image.color = new Color(1, 1, 1, active ? _ACTIVE_OPACITY : _INACTIVE_OPACITY);
        }
    }
}
