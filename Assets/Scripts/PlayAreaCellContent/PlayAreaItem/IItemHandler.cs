using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{
    public interface IItemHandler
    {
        public Item GetItem();
        public void SetItem(Item item);

        public void RemoveItem();

        public Image GetImage();


        public void StartRemoval();
        public bool GetIsProcessingRemoval();
        public void FinishRemoval();


        public void UpdateStateMachine();

    }
}