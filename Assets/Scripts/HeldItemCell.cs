using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class HeldItemCell : MonoBehaviour   //, IItemHandler
    {

        public Item Item { get => _item; }
        private Item _item;  // ITEM currently in cell


        private Image _image;
        private TMPro.TextMeshProUGUI _debugText;


        private void Awake()
        {
            _image = GetComponentInChildren<Image>();
            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }

        void Start()
        {

        }

        void Update()
        {

        }


        internal void SetItem(Item item)
        {
            _item = item;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, .5f);
            _image.sprite = item.Sprite;
        }
        internal void RemoveItem()
        {
            _item = null;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
            _image.sprite = null;
        }

    }
}
