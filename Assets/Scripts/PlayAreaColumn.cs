using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype
{
    public class PlayAreaColumn : MonoBehaviour
    {
        public int Number { get => _number; }
        [SerializeField] private int _number;  //1-N left to right

        public List<PlayAreaCell> Cells { get => _cells; }
        [SerializeField] private List<PlayAreaCell> _cells = new List<PlayAreaCell>();

        [SerializeField] private List<DropCell> _dropCells;

        private List<DropCell> _droppingCells = new List<DropCell>();

        private PlayArea _playArea;

        private List<PlayAreaRowInfo> _rowInfo = new List<PlayAreaRowInfo>();

        internal DropCell GetEmptyDropCell()
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

        private DropCell FindDropItem(PlayAreaCell cell)
        {
            DropCell dropCell = null;

            int cellNumUp = cell.Number - 1;

            bool areDropsPrevented = false;

            while (dropCell == null && areDropsPrevented == false)
            {
                if (cellNumUp > 0)
                {
                    // this is NOT the top cell
                    PlayAreaCell cellUp = _playArea.GetPlayAreaCell(this, cellNumUp);

                    // once a blocked cell is encoutned, stop trying to find drop items
                    if (cellUp.BlockHandler.GetBlock() != null || (cellUp.ObstacleHandler.GetObstacle() != null && !cellUp.ObstacleHandler.CanDrop()))
                    {
                        //hitBlock = true;
                        // if we hit a block, we stop.
                        areDropsPrevented = true;
                        break;
                    }

                    //if (cellUp.ItemHandler.GetItem() != null && !cellUp.IsProcessingItemRemoval)
                    if (cellUp.ItemHandler.GetItem() != null && !cellUp.ItemHandler.GetIsProcessingRemoval())
                    {
                        dropCell = GetEmptyDropCell();
                        dropCell.SetDropFromPosition(GetRowInfo(cellUp.Number));
                        dropCell.ItemHandler.SetItem(cellUp.ItemHandler.GetItem());

                        cellUp.ItemHandler.RemoveItem();
                    }
                    else if (cellUp.ObstacleHandler.GetObstacle() != null && cellUp.ObstacleHandler.CanDrop())
                    {
                        dropCell = GetEmptyDropCell();
                        dropCell.SetDropFromPosition(GetRowInfo(cellUp.Number));
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
                        float newMinY = topmostMinY + _playArea.CellAnchorsHeight; // .11111f; 
                        float newMaxY = topmostMaxY + _playArea.CellAnchorsHeight; // .11111f;
                        dropCell.SetDropFromPosition(newMinY, newMaxY);
                    }


                    //// test "4 blue pin horz" case
                    //ItemPool itemPool = FindAnyObjectByType<ItemPool>();
                    //Item bluepin = itemPool.GetNextAvailable(ItemTypes.BluePin);
                    ////Debug.Log("FORCE 4 BLUE PINS");
                    //dropCell.SetItem(bluepin);

                    //dropCell.SetItem(_playArea.GetFromDrawnItems());
                    dropCell.ItemHandler.SetItem(_playArea.GetFromDrawnItems());
                }
            }

            return dropCell;

        }

        private PlayAreaRowInfo GetRowInfo(int rowNum)
        {
            for (int i = 0; i < _rowInfo.Count; i++)
            {
                if (_rowInfo[i].RowNum == rowNum)
                {
                    return _rowInfo[i];
                }
            }

            //Debug.LogError("RowInfo not found for " + rowNum);
            return default(PlayAreaRowInfo);
        }


        internal void UpdateStateMachines(out bool anyCellsProcessingRemoval)
        {

            // OR
            // for each statemachien in _cells[i].StateMachines - update     

            anyCellsProcessingRemoval = false;

            for (int i = _cells.Count - 1; i >= 0; i--)
            {
                _cells[i].ItemHandler.UpdateStateMachine();
                _cells[i].ObstacleHandler.UpdateStateMachine();
                _cells[i].BlockHandler.UpdateStateMachine();

                if (_cells[i].ObstacleHandler.GetIsProcessingRemoval() ||
                    _cells[i].BlockHandler.GetIsProcessingRemoval() || 
                    _cells[i].ItemHandler.GetIsProcessingRemoval() )
                {
                    anyCellsProcessingRemoval = true;
                }
            }
        }

        internal void UpdateStagedDrops(out bool anyCellsStaged)
        {
            anyCellsStaged = false;
            for (int i = _cells.Count - 1; i >= 0; i--)
            {

                if (_cells[i].ItemHandler.GetItem() == null  && _cells[i].ObstacleHandler.GetObstacle() == null && _cells[i].IsWaitingForDropCell == false && _cells[i].ItemHandler.GetIsProcessingRemoval() == false)
                {
                    DropCell dropCell = FindDropItem(_cells[i]);
                    if (dropCell != null)
                    {
                        //_cells[i].SetStagedDropCell(dropCell);
                        _cells[i].IsWaitingForDropCell = true;

                        PlayAreaRowInfo dropToInfo = GetRowInfo(_cells[i].Number);

                        dropCell.StartDropToTarget(dropToInfo, _cells[i]);

                        _droppingCells.Add(dropCell);

                        anyCellsStaged = true;
                    }
                }
            }
        }
        internal void UpdateDroppingCells(out bool anyCellsDropping)
        {
            anyCellsDropping = false;
            for (int i = _droppingCells.Count - 1; i >= 0; i--)
            {
                anyCellsDropping = true;
                bool hasDropArrived;
                _droppingCells[i].UpdateDropPosition(out hasDropArrived);
                if (hasDropArrived)
                {
                    _droppingCells[i].TransferContentsToCell();

                    _cellsToCatchMatches.Add(_droppingCells[i].TargetCell);

                    _droppingCells.RemoveAt(i);
                }
            }
        }

        private List<PlayAreaCell> _cellsToCatchMatches = new List<PlayAreaCell>();
        internal void UpdateMatches(out bool anyMatchesCaught)
        {
            for (int i = _cellsToCatchMatches.Count-1; i >= 0 ; i--)
            {
                (List<PlayAreaCell> matchesCaught, List<PlayAreaCell> obstaclesCaught) = _cellsToCatchMatches[i].MatchDetector.CatchMatchThree(true);

                for (int j = 0; j < matchesCaught.Count; j++)
                {
                    matchesCaught[j].QueueItemForRemoval();
                    anyMatchesCaught = true;
                }

                for (int k = 0; k < obstaclesCaught.Count; k++)
                {
                    obstaclesCaught[k].QueueObstacleForRemoval();
                }

                _cellsToCatchMatches.RemoveAt(i);
            }
            anyMatchesCaught = false;
        }

        private void Awake()
        {
            _playArea = GetComponentInParent<PlayArea>();
            if (_playArea == null)
            {
                Debug.LogError("COLUMN with NO play area!");
            }

            if (_cells.Count > 0)
            {
                for (int i = 0; i < _cells.Count; i++)
                {
                    RectTransform rect = _cells[i].gameObject.GetComponent<RectTransform>();
                    PlayAreaRowInfo row = new PlayAreaRowInfo();
                    row.RowNum = _cells[i].Number;
                    row.MinY = rect.anchorMin.y;
                    row.MaxY = rect.anchorMax.y;
                    _rowInfo.Add(row);
                }
            }
            else
            {
                Debug.LogError("COLUMN with NO cells!");
            }

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        internal struct PlayAreaRowInfo
        {
            public int RowNum;
            public float MinY;
            public float MaxY;
        }

    }
}
