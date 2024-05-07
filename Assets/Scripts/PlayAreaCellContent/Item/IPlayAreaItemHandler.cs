using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.Item
{
    public interface IPlayAreaItemHandler
    {
        public void SetItem(PlayAreaItem item);
        public void RemoveItemReferenceAndImage();
        public void RemoveItemReference();
        public void RemoveItemImage();

        //public ItemTypes GetItemType();

        public PlayAreaItem GetItem();
        public Image GetImage();
    }
}