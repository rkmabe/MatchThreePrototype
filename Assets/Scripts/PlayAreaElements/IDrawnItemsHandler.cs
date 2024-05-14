using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{
    public interface IDrawnItemsHandler
    {
        public void DrawItems(int numCells, List<ItemTypes> allowedItemTypes, ItemPool itemPool);

        public void ShuffleItems();

        public List<Item> GetDrawnItems();

        public Item GetRandomItem();

        public int GetDrawnItemsIndex(List<ItemTypes> excludedItemTypes);

        public void OnDrawnItemReturn(Item item);

    }
}