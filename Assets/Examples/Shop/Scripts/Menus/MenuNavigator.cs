using System.Collections.Generic;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    public class MenuNavigator : MonoBehaviour {
        [SerializeField]
        List<GameObject> _menus;

        public void ShowMenu (GameObject menu) {
            Debug.Log("Show menu");

            foreach (var m in _menus) {
                m.SetActive(false);
            }

            menu.SetActive(true);
        }
    }
}
