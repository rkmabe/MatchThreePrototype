using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;

namespace MatchThreePrototype.MatchDetection
{
    public interface IStagedItemHandler
    {
        public void SetStagedItem(Item item);

        public void RemoveStagedItem();

        public Item GetStagedItem();

        public bool GetMatchWithStagedItem();
    }
}
