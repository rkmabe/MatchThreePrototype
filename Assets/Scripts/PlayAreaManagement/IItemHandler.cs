using MatchThreePrototype;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaManagment
{
    public interface IItemHandler
    {
        public void SetItem(Item item);

        public void RemoveItemReferenceAndImage();
        public void RemoveItemReference();
        public void RemoveItemImage();
        public bool ContainsItem();

        //public ItemTypes GetItemType();

        public Item GetItem();
        public Image GetImage();
    }
}