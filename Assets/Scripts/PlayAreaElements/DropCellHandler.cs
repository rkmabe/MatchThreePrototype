using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class DropCellHandler : MonoBehaviour, IDropCellHandler
    {
        [SerializeField] private List<DropCell> _dropCells;


        private DropCell GetEmptyDropCell()
        {
            // Drop Items are like an elevator to the column and the item is the passenger.
            // There should be one drop cell per PlayAreaCell setup in the edtior.  So we should never run out.

            for (int i = 0; i < _dropCells.Count; i++)
            {
                if (_dropCells[i].ItemHandler.GetItem() == null && _dropCells[i].ObstacleHandler.GetObstacle() == null)
                {
                    return _dropCells[i];
                }
            }

            Debug.LogError("No DROP CELL availalbe!");
            return null;
        }

        public DropCell FindDropCell(PlayArea playArea, PlayAreaColumn column, PlayAreaCell cell, IRowInfoProvider rowInfoProvider)
        {
            DropCell dropCell = null;

            int cellNumUp = cell.Number - 1;

            bool areDropsPrevented = false;

            while (dropCell == null && areDropsPrevented == false)
            {
                if (cellNumUp > 0)
                {
                    // this is NOT the top cell
                    PlayAreaCell cellUp = playArea.GetPlayAreaCell(column, cellNumUp);

                    // once a blocked cell is encoutned, stop trying to find drop items
                    if (cellUp.BlockHandler.GetBlock() != null || (cellUp.ObstacleHandler.GetObstacle() != null && !cellUp.ObstacleHandler.CanDrop()))
                    {
                        //hitBlock = true;
                        // if we hit a block, we stop.
                        areDropsPrevented = true;
                        break;
                    }

                    if (cellUp.ItemHandler.GetItem() != null && !cellUp.ItemHandler.GetIsProcessingRemoval())
                    {
                        dropCell = GetEmptyDropCell();
                        dropCell.SetDropFromPosition(rowInfoProvider.GetRowInfo(cellUp.Number));
                        dropCell.ItemHandler.SetItem(cellUp.ItemHandler.GetItem());

                        cellUp.ItemHandler.RemoveItem();
                    }
                    else if (cellUp.ObstacleHandler.GetObstacle() != null && cellUp.ObstacleHandler.CanDrop())
                    {
                        dropCell = GetEmptyDropCell();
                        dropCell.SetDropFromPosition(rowInfoProvider.GetRowInfo(cellUp.Number));
                        dropCell.ObstacleHandler.SetObstacle(cellUp.ObstacleHandler.GetObstacle());

                        cellUp.ObstacleHandler.RemoveObstacle();
                    }
                    else
                    {
                        cellNumUp--;
                    }
                }
                else
                {
                    // we have reached the top of the column and found no items or blocks
                    // generate a new item above the column
                    dropCell = GetEmptyDropCell();

                    // put the new item on top of any drop items already there
                    float topmostMinY = 0;
                    float topmostMaxY = 0;
                    for (int i = 0; i < _dropCells.Count; i++)
                    {
                        if (_dropCells[i].ItemHandler.GetItem() != null || _dropCells[i].ObstacleHandler.GetObstacle() != null)
                        {
                            if (_dropCells[i].RectMinY > topmostMinY)
                            {
                                topmostMinY = _dropCells[i].RectMinY;
                                topmostMaxY = _dropCells[i].RectMaxY;
                            }
                        }
                    }
                    if (topmostMinY == 0)
                    {
                        dropCell.SetDropFromPosition(dropCell.EditorRectMinY, dropCell.EditorRectMaxY);
                    }
                    else
                    {
                        float newMinY = topmostMinY + playArea.CellAnchorsHeight; // .11111f; 
                        float newMaxY = topmostMaxY + playArea.CellAnchorsHeight; // .11111f;
                        dropCell.SetDropFromPosition(newMinY, newMaxY);
                    }


                    //// test "4 blue pin horz" case
                    //ItemPool itemPool = FindAnyObjectByType<ItemPool>();
                    //Item bluepin = itemPool.GetNextAvailable(ItemTypes.BluePin);
                    ////Debug.Log("FORCE 4 BLUE PINS");
                    //dropCell.SetItem(bluepin);

                    //dropCell.ItemHandler.SetItem(playArea.GetFromDrawnItems());
                    dropCell.ItemHandler.SetItem(playArea.DrawnItemsHandler.GetRandomItem());
                }
            }

            return dropCell;

        }



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}