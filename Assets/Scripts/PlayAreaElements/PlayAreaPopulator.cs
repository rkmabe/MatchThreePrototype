using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{
    public class PlayAreaPopulator : MonoBehaviour, IPlayAreaPopulator
    {

        [SerializeField] private List<ObstacleTypes> _allowedObstacleTypes;
        [SerializeField] private List<BlockTypes> _allowedBlockTypes;

        List<ItemPlacedInfo> _cellsContainingOnlyItems = new List<ItemPlacedInfo>();

        IDrawnItemsHandler _drawnItemsHandler;
        ObstaclePool _obstaclePool;
        BlockPool _blockPool;

        public void Setup(IDrawnItemsHandler drawnItemsHandler, ObstaclePool obstaclePool, BlockPool blockPool)
        {
            _drawnItemsHandler = drawnItemsHandler;
            _obstaclePool = obstaclePool;
            _blockPool = blockPool;
        }

        public void PlaceItems(List<PlayAreaColumn> columns)
        {
            // populate grid, ensuring that no match 3 is initially populated
            for (int i = 0; i < columns.Count; i++)
            {
                int itemPlacedCounter = 0;

                for (int j = 0; j < columns[i].Cells.Count; j++)
                {
                    List<ItemTypes> excludedItemTypes = new List<ItemTypes>();
                    Item validItem = null;
                    int o = 0;
                    while (validItem == null)
                    {
                        List<Item> drawnItems = _drawnItemsHandler.GetDrawnItems();

                        int drawnItemsIndex = _drawnItemsHandler.GetDrawnItemsIndex(excludedItemTypes);
                        ItemTypes candidateItemType = drawnItems[drawnItemsIndex].ItemType;

                        if (IsPopulationPlacementValid(candidateItemType, columns[i].Number, columns[i].Cells[j].Number))
                        {
                            //validItem = _drawnItems[drawnItemsIndex];
                            validItem = drawnItems[drawnItemsIndex];


                            columns[i].Cells[j].ItemHandler.SetItem(validItem);

                            //_drawnItems.RemoveAt(drawnItemsIndex);
                            drawnItems.RemoveAt(drawnItemsIndex);

                            itemPlacedCounter++;
                            ItemPlacedInfo cellPopulated = new ItemPlacedInfo();
                            cellPopulated.index = itemPlacedCounter;
                            cellPopulated.column = columns[i].Number;
                            cellPopulated.row = columns[i].Cells[j].Number;

                            cellPopulated.item = validItem;
                            cellPopulated.cell = columns[i].Cells[j];

                            _cellsContainingOnlyItems.Add(cellPopulated);

                        }
                        else
                        {
                            excludedItemTypes.Add(candidateItemType);
                            o++;
                            //Debug.Log("Match3 prevented at " + columns[i].Number + ", " + columns[i].Cells[j].Number);
                        }
                        if (o > 5)
                        {
                            Debug.LogWarning("could not find valid item - SHOULD NOT HAPPEN!!");
                            validItem = drawnItems[0];
                        }
                    }
                }
            }
        }

        public void PlaceObstacles(int numCellsToObstruct)
        {

            if (numCellsToObstruct > 0 && _allowedObstacleTypes.Count > 0)
            {
                for (int i = 0; i < numCellsToObstruct; i++)
                {
                    if (_cellsContainingOnlyItems.Count > 0)
                    {
                        int rand = UnityEngine.Random.Range(0, _cellsContainingOnlyItems.Count);
                        {
                            // find cellToBlock in the play area and apply 
                            ItemPlacedInfo cellToObstruct = _cellsContainingOnlyItems[rand];

                            // select an Allowed Block and apply it to the randomly slected cell in play
                            Obstacle obstacle = GetAllowedObstacle();

                            if (cellToObstruct.cell.ItemHandler.GetItem() != null)
                            {
                                cellToObstruct.cell.ItemHandler.RemoveItem();
                            }

                            cellToObstruct.cell.ObstacleHandler.SetObstacle(obstacle);

                            _cellsContainingOnlyItems.RemoveAt(rand);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("OUT of cells to pick to obstruct!");
                    }
                }
            }
        }
        public void PlaceBlocks(int numCellsToBlock)
        {
            if (numCellsToBlock > 0 && _allowedBlockTypes.Count > 0)
            {
                for (int i = 0; i < numCellsToBlock; i++)
                {
                    if (_cellsContainingOnlyItems.Count > 0)
                    {
                        int rand = UnityEngine.Random.Range(0, _cellsContainingOnlyItems.Count);
                        {
                            // find cellToBlock in the play area and apply the block
                            ItemPlacedInfo cellToBlock = _cellsContainingOnlyItems[rand];

                            // select an Allowed Block and apply it to the randomly slected cell in play
                            Block block = GetAllowedBlock();

                            cellToBlock.cell.BlockHandler.SetBlock(block);

                            _cellsContainingOnlyItems.RemoveAt(rand);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("OUT of cells to pick to block!");
                    }
                }
            }

        }

        private Block GetAllowedBlock()
        {
            if (_allowedBlockTypes.Count == 0)
            {
                return null;
            }
            if (_allowedBlockTypes.Count == 1)
            {
                return _blockPool.GetNextAvailable(_allowedBlockTypes[0]);
            }
            else
            {
                int rand = UnityEngine.Random.Range(0, _allowedBlockTypes.Count);
                return _blockPool.GetNextAvailable(_allowedBlockTypes[rand]);
            }
        }

        private Obstacle GetAllowedObstacle()
        {
            if (_allowedObstacleTypes.Count == 0)
            {
                return null;
            }
            if (_allowedObstacleTypes.Count == 1)
            {
                return _obstaclePool.GetNextAvailable(_allowedObstacleTypes[0]);
            }
            else
            {
                int rand = UnityEngine.Random.Range(0, _allowedObstacleTypes.Count);
                return _obstaclePool.GetNextAvailable(_allowedObstacleTypes[rand]);
            }
        }


        private bool IsPopulationPlacementValid(ItemTypes itemTypeToPlace, int columnNum, int cellNum)
        {
            // check to see if a match 3 will occur as a result of this POPULATION placement.
            // if it will, the placement is invalid.

            // items are populated from columns left to right and cells UP to DOWN
            // .. but could be DOWN to UP, depending on cell sort

            bool isMatchOneAbove = IsMatch(itemTypeToPlace, columnNum, cellNum - 1);
            if (isMatchOneAbove)
            {
                bool isMatchTwoAbove = IsMatch(itemTypeToPlace, columnNum, cellNum - 2);
                if (isMatchTwoAbove)
                {
                    return false;
                }
            }

            bool isMatchOneBelow = IsMatch(itemTypeToPlace, columnNum, cellNum + 1);
            if (isMatchOneBelow)
            {
                bool isMatchTwoBelow = IsMatch(itemTypeToPlace, columnNum, cellNum + 2);
                if (isMatchTwoBelow)
                {
                    return false;
                }
            }

            bool isMatchOneLeft = IsMatch(itemTypeToPlace, columnNum - 1, cellNum);
            if (isMatchOneLeft)
            {
                bool isMatchTwoLeft = IsMatch(itemTypeToPlace, columnNum - 2, cellNum);
                if (isMatchTwoLeft)
                {
                    return false;
                }
            }

            return true;
        }
        private bool IsMatch(ItemTypes itemTypeToMatch, int columnNum, int cellNum)
        {
            bool isMatch = false;

            // just look in _cellsWithItems instead of getting this from Play ARea!!

            // get item at position columnNum, cellnum 
            Item itemAtPosition = GetItemPopulatedAt(columnNum, cellNum);
            if (itemAtPosition != null)
            {
                isMatch = itemTypeToMatch == itemAtPosition.ItemType ? true : false;
            }

            return isMatch;
        }
        private Item GetItemPopulatedAt(int columnNum, int cellNum)
        {
            for (int i = 0; i < _cellsContainingOnlyItems.Count ; i++)
            {
                if (_cellsContainingOnlyItems[i].column == columnNum &&
                    _cellsContainingOnlyItems[i].row  == cellNum)
                {
                    return _cellsContainingOnlyItems[i].item;
                }
            }
            return null;
        }

    }

    struct ItemPlacedInfo
    {
        public int index;
        public int column;
        public int row;

        public Item item;
        public PlayAreaCell cell;
    }
}