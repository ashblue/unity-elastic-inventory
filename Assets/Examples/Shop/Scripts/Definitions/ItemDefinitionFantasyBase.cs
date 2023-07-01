using CleverCrow.Fluid.ElasticInventory;
using UnityEngine;

namespace CleverCrow.Fluid.Examples {
    // We strongly recommend declaring your own custom item definition base class. This way you can add/remove your own custom fields
    // to any item definition on the fly
    public abstract class ItemDefinitionFantasyBase : ItemDefinitionBase {
        [SerializeField]
        Sprite _image;

        [SerializeField]
        int _cost;

        [TextArea(3, 10)]
        [SerializeField]
        string _description;

        public Sprite Image => _image;
        public int Cost => _cost;
        public string Description => _description;
    }
}
