using MatchThreePrototype;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent
{
    public interface IPlayAreaItemHandler
    {
        public void SetItem(PlayAreaItem item);

        public void RemoveItemReferenceAndImage();
        public void RemoveItemReference();
        public void RemoveItemImage();
        public bool ContainsItem();

        //public ItemTypes GetItemType();

        public PlayAreaItem GetItem();
        public Image GetImage();
    }
}