using MatchThreePrototype.PlayAreaCellContent.Item;

namespace MatchThreePrototype.PlayAreaCellMatching
{

    public interface IStagedItemHandler
    {
        public void SetStagedItem(PlayAreaItem item);

        public void RemoveStagedItem();

        public PlayAreaItem GetStagedItem();

        public bool GetMatchWithStagedItem();
    }
}
