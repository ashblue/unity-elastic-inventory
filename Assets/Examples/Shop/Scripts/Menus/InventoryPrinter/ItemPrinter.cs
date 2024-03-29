﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.Fluid.Examples {
    public class ItemPrinter : MonoBehaviour {
        [SerializeField]
        Image _image;

        [SerializeField]
        TMPro.TextMeshProUGUI _quantity;

        [SerializeField]
        Button _button;

        public void Setup (ItemDefinitionFantasyBase definition, int quantity) {
            _image.sprite = definition.Image;
            SetQuantity(quantity);
        }

        public void AddClick (Action action) {
            _button.onClick.AddListener(() => action());
        }

        public void SetQuantity (int quantity) {
            _quantity.text = quantity == 1 ? "" : quantity.ToString();
        }
    }
}
