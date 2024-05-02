using MatchThreePrototype;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaManagment
{
    public interface IItemHandler
    {
        //public void Set(Item item, Image image);
        public void SetItem(Item item);

        public void RemoveItemReferenceAndImage();
        public void RemoveItemReference();
        public void RemoveItemImage();


        //public void RemoveReference();
        //public void RemoveItemSprite();


        public bool ContainsItem();

        //public ItemTypes GetItemType();
        public Item GetItem();

        public Image GetImage();

        //public void RemovalAnimationUpdate();

        //public void UpdateRemovalAnimation();

        //public Image GetImage();
    }
}