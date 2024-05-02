using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent
{

    public class PlayAreaItemHandler : MonoBehaviour, IPlayAreaItemHandler
    {
        [SerializeField] private Image _itemImage;

        private PlayAreaItem _item;


        public void SetItem(PlayAreaItem item)
        {
            _item = item;
            _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, 1);
            _itemImage.sprite = item.Sprite;
        }

        public void RemoveItemReferenceAndImage()
        {
            RemoveItemReference();
            RemoveItemImage();
        }

        public void RemoveItemReference()
        {
            _item = null;
        }
        public void RemoveItemImage()
        {
            _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, 0);
            _itemImage.sprite = null;
        }



        public bool ContainsItem()
        {
            return _item != null;
        }

        //public ItemTypes GetItemType()
        //{
        //    return _item.ItemType;
        //}
        public PlayAreaItem GetItem()
        {
            return _item;
        }

        public Image GetImage()
        {
            return _itemImage;
        }

    }
}