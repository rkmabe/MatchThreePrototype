using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using System.Collections.Generic;

namespace MatchThreePrototype.PlayAreaElements
{
    public interface IDrawnItemsHandler
    {
        public void Setup(List<ItemTypes> allowedItemTypes, ItemPool itemPool);

        public void DrawItems(int numCells);

        public void ShuffleItems();

        public List<Item> GetDrawnItems();

        public Item GetRandomItem();

        public int GetDrawnItemsIndex(List<ItemTypes> excludedItemTypes);

        public void OnDrawnItemReturn(Item item);

    }
}