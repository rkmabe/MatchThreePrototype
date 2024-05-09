using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{
    public interface IItemHandler
    {
        public Item GetItem();
        public void SetItem(Item item);

        public Image GetImage();

        public void RemoveItemReferenceAndImage();
        public void RemoveItemReference();
        public void RemoveItemImage();




        // TODO: refactor as IContentRemover ?

        public void StartRemoval();
        public bool GetIsProcessingRemoval();
        public void StopRemoval();


        public void UpdateStateMachine();

    }
}