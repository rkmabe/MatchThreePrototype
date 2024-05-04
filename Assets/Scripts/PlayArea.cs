using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MatchThreePrototype.PlayAreaCellContent;

namespace MatchThreePrototype
{

    public class PlayArea : MonoBehaviour
    {

        [SerializeField] private List<PlayAreaColumn> _columns;

        public float CellAnchorsHeight { get => _cellAnchorsHeight; }
        [Header("Must match in Editor!")]
        [SerializeField] private float _cellAnchorsHeight = 0.11111f;

        public bool IsSwapRangeLimited { get => _isSwapRangeLimited; }
        //[SerializeField] private bool _isSwapRangeLimited = false;
        private bool _isSwapRangeLimited = false;

        public int CellSwapRange { get => _cellSwapRange; }
        [SerializeField] private int _cellSwapRange = 0;

        [Header("Must be UNIQUE!")]
        //[SerializeField] private List<ItemTypes> _allowedItemTypes;
        private List<ItemTypes> _allowedItemTypes;

        [SerializeField] private List<BlockTypes> _allowedBlockTypes;

        [SerializeField] private List<ObstacleTypes> _allowedObstacleTypes;

        public MoveItemCell CellMoveToOrigin { get => _cellMoveToOrigin; }
        [SerializeField] private MoveItemCell _cellMoveToOrigin;

        public MoveItemCell CellMoveToDestination { get => _cellMoveToDestination; }
        [SerializeField] private MoveItemCell _cellMoveToDestination;

        [SerializeField] private GameObject _destinationCellIndicator;
        private Image _destinationCellIndicatorImage;

        [SerializeField] private GameObject _originCellIndicator;
        private Image _originCellIndicatorImage;

        public HeldItemCell HeldItemCell { get => _heldItemCell; }
        [SerializeField] private HeldItemCell _heldItemCell;

        [SerializeField] private RectTransform _playAreaRect;

        private float _percentCellsToBlock;
        private float _percentCellsToObstruct;

        private GraphicRaycaster _graphicRaycaster;

        private ItemPool _itemPool;
        private BlockPool _blockPool;
        private ObstaclePool _obstaclePool;

        private int _numCells;

        private List<PlayAreaItem> _drawnItems = new List<PlayAreaItem>();

        public bool IsPopulated { get => _isPopulated; }
        private bool _isPopulated = false;

        private int _numCellsChecked = 0;
        private int _numMatch3s = 0;

        public bool IsInFlux { get => _isInFlux; }
        private bool _isInFlux = false;

        private const string PLAY_AREA_RECT = "PLAY_AREA_RECT";

        public delegate void OnCellCheckMatchRequest();
        public static OnCellCheckMatchRequest OnCellCheckMatchRequestDelegate;

        private SettingsController _settingsController;

        private void CheckAllCellMatches()
        {

            // can be used to cause all cells to check mathes
            // usually this is not necessary -
            // only cells ajacent to modified cells must be checked most of the time.

            _numCellsChecked = 0;
            PlayAreaCell.OnCellCheckMatchCompleteDelegate += OnCellCheckMatchComplete;

            // advertise that you want cells to check
            OnCellCheckMatchRequestDelegate();

            //_isCheckingMatches = true;
        }
        private void OnCellCheckMatchComplete(bool isMatch3)
        {
            _numCellsChecked++;
            if (isMatch3)
            {
                _numMatch3s++;
            }
            if (_numCellsChecked == _numCells)
            {
                Debug.Log("All cells MATHCED - QUE REMOVAL");

                PlayAreaCell.OnCellCheckMatchCompleteDelegate -= OnCellCheckMatchComplete;

                if (_numMatch3s > 0)
                {

                }

            }
        }

        internal void IndicateDragOverCell(PlayAreaCell dragOverCell)
        {
            _destinationCellIndicator.transform.position = dragOverCell.transform.position;
            _destinationCellIndicatorImage.color = new Color(_destinationCellIndicatorImage.color.r, _destinationCellIndicatorImage.color.g, _destinationCellIndicatorImage.color.b, 1);
        }

        internal void IndicateDragFromCell(PlayAreaCell dragFromCell)
        {
            _originCellIndicator.transform.position = dragFromCell.transform.position;
            _originCellIndicatorImage.color = new Color(_originCellIndicatorImage.color.r, _originCellIndicatorImage.color.g, _originCellIndicatorImage.color.b, 1);
        }

        internal void ClearDragIndicators()
        {
            _originCellIndicatorImage.color = new Color(_originCellIndicatorImage.color.r, _originCellIndicatorImage.color.g, _originCellIndicatorImage.color.b, 0);
            _destinationCellIndicatorImage.color = new Color(_destinationCellIndicatorImage.color.r, _destinationCellIndicatorImage.color.g, _destinationCellIndicatorImage.color.b, 0);
        }

        private int GetDrawnItemsIndex(List<ItemTypes> excludedItemTypes)
        {
            if (excludedItemTypes.Count == 0)
            {
                return UnityEngine.Random.Range(0, _drawnItems.Count);
            }
            else
            {
                for (int i = 0; i < _drawnItems.Count; i++)
                {
                    bool isExcluded = false;
                    for (int j = 0; j < excludedItemTypes.Count; j++)
                    {
                        if (_drawnItems[i].ItemType == excludedItemTypes[j])
                        {
                            isExcluded = true;
                            break;
                        }
                    }
                    if (!isExcluded)
                    {
                        return i;
                    }
                }
            }

            // you are never going to get here. .. unless you have too few allowed item types!
            //Debug.LogError("oh yeah?");
            return UnityEngine.Random.Range(0, _drawnItems.Count);
        }

        private void Populate()
        {
            List<CellInPlay> cellsIToPick = new List<CellInPlay>();
            int k = 0;

            // draw a pool of items
            DrawItems();

            // shuffle
            ShuffleItems();

            // populate grid, ensuring that no match 3 is initially populated
            for (int i = 0; i < _columns.Count; i++)
            {
                for (int j = 0; j < _columns[i].Cells.Count; j++)
                {
                    List<ItemTypes> excludedItemTypes = new List<ItemTypes>();
                    PlayAreaItem validItem = null;
                    int o = 0;
                    while (validItem == null)
                    {

                        //int index = UnityEngine.Random.Range(0, _drawnItems.Count);
                        int drawnItemsIndex = GetDrawnItemsIndex(excludedItemTypes);
                        ItemTypes candidateItemType = _drawnItems[drawnItemsIndex].ItemType;
                        if (IsPopulationPlacementValid(candidateItemType, _columns[i].Number, _columns[i].Cells[j].Number))
                        {
                            validItem = _drawnItems[drawnItemsIndex];

                            //_columns[i].Cells[j].SetItem(validItem);
                            _columns[i].Cells[j].ItemHandler.SetItem(validItem);

                            _drawnItems.RemoveAt(drawnItemsIndex);

                            k++;
                            CellInPlay item = new CellInPlay();
                            item.index = k;
                            item.column = _columns[i].Number;
                            item.row = _columns[i].Cells[j].Number;
                            cellsIToPick.Add(item);

                        }
                        else
                        {
                            excludedItemTypes.Add(candidateItemType);
                            o++;
                            //Debug.Log("Match3 prevented at " + _columns[i].Number + ", " + _columns[i].Cells[j].Number);
                        }
                        if (o > 5)
                        {
                            Debug.Log("could not find valid item - SHOULD NOT HAPPEN!!");
                            validItem = _drawnItems[0];
                        }
                    }
                }
            }

            //"obstruct" a certain number of cells in play
            int numCellsToObstruct = Mathf.RoundToInt(_numCells * _percentCellsToObstruct);
            if (numCellsToObstruct > 0 && _allowedObstacleTypes.Count > 0)
            {
                for (int i = 0; i < numCellsToObstruct; i++)
                {
                    if (cellsIToPick.Count > 0)
                    {
                        int rand = UnityEngine.Random.Range(0, cellsIToPick.Count);
                        {
                            // find cellToBlock in the play area and apply 
                            CellInPlay cellToObstruct = cellsIToPick[rand];

                            // select an Allowed Block and apply it to the randomly slected cell in play
                            PlayAreaObstacle obstacle = GetAllowedObstacle();

                            PlayAreaColumn col = GetPlayAreaColumn(cellToObstruct.column);
                            PlayAreaCell cell = GetPlayAreaCell(col, cellToObstruct.row);

                            if (cell.ItemHandler.GetItem() != null)
                            {
                                cell.ItemHandler.RemoveItemReferenceAndImage();
                            }

                            //cell.SetObstacle(obstacle);
                            cell.ObstacleHandler.SetObstacle(obstacle);

                            cellsIToPick.RemoveAt(rand);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("OUT of cells to pick to obstruct!");
                    }
                }
            }
            
            //"block out" a percentage of cells in itemsInPlay
            int numCellsToBlock = Mathf.RoundToInt(_numCells * _percentCellsToBlock);
            if (numCellsToBlock > 0 && _allowedBlockTypes.Count > 0)
            {
                for (int i = 0; i < numCellsToBlock; i++)
                {
                    if (cellsIToPick.Count > 0)
                    {
                        int rand = UnityEngine.Random.Range(0, cellsIToPick.Count);
                        {
                            // find cellToBlock in the play area and apply the block
                            CellInPlay cellToBlock = cellsIToPick[rand];

                            // select an Allowed Block and apply it to the randomly slected cell in play
                            Block block = GetAllowedBlock();

                            PlayAreaColumn col = GetPlayAreaColumn(cellToBlock.column);
                            PlayAreaCell cell = GetPlayAreaCell(col, cellToBlock.row);

                            cell.BlockHandler.SetBlock(block);

                            cellsIToPick.RemoveAt(rand);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("OUT of cells to pick to block!");
                    }
                }
            }

            _isPopulated = true;
        }

        internal PlayAreaObstacle GetAllowedObstacle()
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

        internal Block GetAllowedBlock()
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

        internal PlayAreaItem GetFromDrawnItems()
        {
            if (_drawnItems.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, _drawnItems.Count);
                PlayAreaItem item = _drawnItems[index];
                _drawnItems.RemoveAt(index);

                return item;
            }

            Debug.LogError("No more DRAWN items!");
            return null;
        }

        internal void ReturnToDrawnItems(PlayAreaItem item)
        {
            _drawnItems.Add(item);
        }

        private void DrawItems()
        {
            // draw enough items to fully populate play area twice.
            // try to draw an equal amount of each type.
            // if not an even divide, draw a random type for each remainder.

            int drawCount = _numCells * 2;

            int divTypesPerDrawCount = drawCount / _allowedItemTypes.Count;
            for (int i = 0; i < _allowedItemTypes.Count; i++)
            {
                for (int j = 0; j < divTypesPerDrawCount; j++)
                {
                    ItemTypes itemType = _allowedItemTypes[i];

                    PlayAreaItem item = _itemPool.GetNextAvailable(itemType);

                    _drawnItems.Add(item);
                }
            }

            int modTypesPerDrawCount = drawCount % _allowedItemTypes.Count;
            for (int i = 0; i < modTypesPerDrawCount; i++)
            {
                PlayAreaItem item = _itemPool.GetNextAvailable();
                _drawnItems.Add(item);
            }

        }

        private void ShuffleItems()
        {
            for (int i = _drawnItems.Count - 1; i > 0; i--)
            {
                int k = UnityEngine.Random.Range(0, i + 1);
                PlayAreaItem itemToSwap = _drawnItems[k];
                _drawnItems[k] = _drawnItems[i];
                _drawnItems[i] = itemToSwap;
            }
        }

        private bool IsPopulationPlacementValid(ItemTypes itemTypeToPlace, int columnNum, int cellNum)
        {
            // check to see if a match 3 will occur as a result of this POPULATION placement.
            // if it will, the placement is invalid.

            // items are populated from columns left to right and cells UP to DOWN
            // .. but could be DOWN to UP, depending on cell sort

            bool isMatchOneAbove = IsItemAtPositionMatch1(itemTypeToPlace, columnNum, cellNum - 1);
            if (isMatchOneAbove)
            {
                bool isMatchTwoAbove = IsItemAtPositionMatch1(itemTypeToPlace, columnNum, cellNum - 2);
                if (isMatchTwoAbove)
                {
                    return false;
                }
            }

            bool isMatchOneBelow = IsItemAtPositionMatch1(itemTypeToPlace, columnNum, cellNum + 1);
            if (isMatchOneBelow)
            {
                bool isMatchTwoBelow = IsItemAtPositionMatch1(itemTypeToPlace, columnNum, cellNum + 2);
                if (isMatchTwoBelow)
                {
                    return false;
                }
            }

            bool isMatchOneLeft = IsItemAtPositionMatch1(itemTypeToPlace, columnNum - 1, cellNum);
            if (isMatchOneLeft)
            {
                bool isMatchTwoLeft = IsItemAtPositionMatch1(itemTypeToPlace, columnNum - 2, cellNum);
                if (isMatchTwoLeft)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsItemAtPositionMatch1(ItemTypes itemTypeToMatch, int columnNum, int cellNum)
        {
            bool isMatch = false;

            // get item at position columnNum, cellnum 
            PlayAreaItem itemAtPosition = GetPlayAreaItemAt(columnNum, cellNum);
            if (itemAtPosition != null)
            {
                isMatch = itemTypeToMatch == itemAtPosition.ItemType ? true : false;
            }

            return isMatch;
        }

        private PlayAreaItem GetPlayAreaItemAt(int columnNum, int cellNum)
        {
            PlayAreaColumn column = GetPlayAreaColumn(columnNum);

            if (column != null)
            {
                PlayAreaCell cell = GetPlayAreaCell(column, cellNum);
                if (cell != null)
                {
                    //return cell.Item;
                    return cell.ItemHandler.GetItem();
                }
            }

            return null;
        }

        internal PlayAreaCell GetPlayAreaCell(PlayAreaColumn column, int cellNum)
        {
            for (int i = 0; i < column.Cells.Count; i++)
            {
                if (column.Cells[i].Number == cellNum)
                {
                    return column.Cells[i];
                }
            }

            return null;
        }

        internal PlayAreaColumn GetPlayAreaColumn(int columnNum)
        {
            for (int i = 0; i < _columns.Count; i++)
            {
                if (_columns[i].Number == columnNum)
                {
                    return _columns[i];
                }
            }

            return null;
        }

        internal bool IsPositionInSwapRange(Vector2 touchPoint, PlayAreaCell dragOriginCell, out PlayAreaCell cellTouched)
        {
            // return true if this drag position contains a play area cell within swap range

            cellTouched = null;
            bool withinPlayArea = false;

            _dummyEventData.position = touchPoint;
            List<RaycastResult> results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(_dummyEventData, results);
            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    // one row of resuts should contain a play area cell.

                    PlayAreaCell cellAtPosition = results[i].gameObject.GetComponentInParent<PlayAreaCell>();
                    if (cellAtPosition != null)
                    {
                        cellTouched = cellAtPosition;
                    }

                    if (results[i].gameObject.tag == PLAY_AREA_RECT)
                    {
                        withinPlayArea = true;
                    }
                }
            }

            if (_isSwapRangeLimited)
            {
                if (cellTouched != null)
                {
                    // if the swap range is limited, the cell touched must be within swap range
                    int rowMin = dragOriginCell.Number - _cellSwapRange;
                    int rowMax = dragOriginCell.Number + _cellSwapRange;

                    int colMin = dragOriginCell.ColumnNumber - _cellSwapRange;
                    int colMax = dragOriginCell.ColumnNumber + _cellSwapRange;

                    if ((cellTouched.Number >= rowMin && cellTouched.Number <= rowMax) &&
                        (cellTouched.ColumnNumber >= colMin && cellTouched.ColumnNumber <= colMax))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // no touched cell - we cannot be in range
                    return false;
                }

            }
            else
            {
                // if swap range is not limited this is a valid drag posiition if it is within the play area rect
                return withinPlayArea;
            }

        }

        internal PlayAreaCell GetCellAtPosition(Vector2 tapPoint)
        {
            _dummyEventData.position = tapPoint;

            List<RaycastResult> results = new List<RaycastResult>();

            _graphicRaycaster.Raycast(_dummyEventData, results);

            for (int i = 0; i < results.Count; i++)
            {
                PlayAreaCell cellAtPosition = results[i].gameObject.GetComponentInParent<PlayAreaCell>();
                if (cellAtPosition != null)
                {
                    return cellAtPosition;
                }
            }

            return null;
        }
        private PointerEventData _dummyEventData = new PointerEventData(null);

        private int GetCellCount()
        {
            int cellCount = 0;
            for (int i = 0; i < _columns.Count; i++)
            {
                cellCount += _columns[i].Cells.Count;
            }

            return cellCount;
        }

        private List<ItemTypes> PrototypeBuildItemTypes()
        {
            List<ItemTypes> itemTypes = new List<ItemTypes>();

            // we must have at least 3 types (at least with 9x9 play area!)
            itemTypes.Add(ItemTypes.BluePin);
            itemTypes.Add(ItemTypes.WhitePin);
            itemTypes.Add(ItemTypes.RedPin);

            int numItemTypes = _settingsController.GetNumItemTypes();

            if (numItemTypes > 3)
            {
                itemTypes.Add(ItemTypes.BlackBall);
            }
            if (numItemTypes > 4) 
            {
                itemTypes.Add(ItemTypes.GreenPin);
            }
            if (numItemTypes > 5)
            {
                itemTypes.Add(ItemTypes.PurplePin);
            }
            if (numItemTypes > 6)
            {
                itemTypes.Add(ItemTypes.PinkPin);
            }

            return itemTypes;
        }

        private void Awake()
        {
            _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();

            _itemPool = FindAnyObjectByType<ItemPool>();
            _blockPool = FindObjectOfType<BlockPool>();
            _obstaclePool = FindObjectOfType<ObstaclePool>();

            _settingsController = FindAnyObjectByType<SettingsController>();

            _numCells = GetCellCount();

            _originCellIndicatorImage = _originCellIndicator.GetComponentInChildren<Image>();
            _destinationCellIndicatorImage = _destinationCellIndicator.GetComponentInChildren<Image>();

        }
        private void OnDestroy()
        {

        }


        // Start is called before the first frame update
        void Start()
        {


        }

        //bool DEBUG_CHECK = false;

        // Update is called once per frame
        void Update()
        {

            //if (DEBUG_CHECK)
            //{
            //    CheckAllCellMatches();
            //    DEBUG_CHECK = false;
            //}

            // do not process anything until populated
            if (!_isPopulated)
            {
                if (_itemPool.IsInitialized && _blockPool.IsInitialized && _obstaclePool.IsInitialized)
                {

                    _percentCellsToBlock = _settingsController.GetPctBlock();
                    _percentCellsToObstruct = _settingsController.GetPctObstacle();

                    _allowedItemTypes = PrototypeBuildItemTypes();
                    _isSwapRangeLimited = _settingsController.GetLimitSwapRange();


                    Populate();

                    //DEBUG_CHECK = true;

                }
                return;
            }

            // PROCESS SWAPS-------------------------------------------------------
            // process any active SwapItemCells
            bool anyCellsSwapping = false;

            if (_cellMoveToOrigin.ItemHandler.GetItem() != null)
            {
                anyCellsSwapping = true;
                bool hasSwapArrived;
                _cellMoveToOrigin.UpdatePosition(out hasSwapArrived);
                if (hasSwapArrived)
                {
                    _cellMoveToOrigin.ProcessOnArrival();
                }
            }

            if (_cellMoveToDestination.ItemHandler.GetItem() != null)
            {
                anyCellsSwapping = true;
                bool hasSwapArrived;
                _cellMoveToDestination.UpdatePosition(out hasSwapArrived);
                if (hasSwapArrived)
                {
                    _cellMoveToDestination.ProcessOnArrival();
                }
            }
            if (anyCellsSwapping)
            {
                _isInFlux = true;
                return;
            }

            // PROCESS DROPS-------------------------------------------------------
            bool anyColumnDropping = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                bool thisColumnDropping;
                _columns[i].UpdateDroppingCells(out thisColumnDropping);
                if (thisColumnDropping)
                {
                    anyColumnDropping = true;
                }
            }
            if (anyColumnDropping)
            {
                _isInFlux = true;
                return;
            }

            // PROCESS MATCHES------------------------------------------------------
            bool anyMatchRemoved = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                bool thisColumnMatchesCaught;
                _columns[i].UpdateMatches(out thisColumnMatchesCaught);
                if (thisColumnMatchesCaught)
                {
                    anyMatchRemoved = true;
                }
            }
            if (anyMatchRemoved)
            {
                _isInFlux = true;
                return;
            }

            // PROCESS REMOVALS-----------------------------------------------------
            bool anyColumnRemoving = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                bool thisColumnRemoving;
                _columns[i].UpdateRemovals(out thisColumnRemoving);
                if (thisColumnRemoving)
                {
                    anyColumnRemoving = true;
                }
            }
            if (anyColumnRemoving)
            {
                _isInFlux = true;
                return;
            }

            // STAGE NEW DROPS-------------------------------------------------------
            bool anyColumnsStaged = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                bool thisColumnStaged;
                _columns[i].UpdateStagedDrops(out thisColumnStaged);
                if (thisColumnStaged)
                {
                    anyColumnsStaged = true;
                }
            }
            if (anyColumnsStaged)
            {
                _isInFlux = true;
                return;
            }


            // if we make it here, the play area is NOT in flux
            _isInFlux = false;
        }

        struct CellInPlay
        {
            public int index;
            public int column;
            public int row;
        }

    }

    public struct PlayAreaCellMatches
    {
        public ItemTypes ItemType;

        public bool IsMatchUp;
        public PlayAreaCell CellMatchUp;

        public bool IsMatchDown;
        public PlayAreaCell CellMatchDown;

        public bool IsMatchLeft;
        public PlayAreaCell CellMatchLeft;

        public bool IsMatchRight;
        public PlayAreaCell CellMatchRight;

        public bool IsMiddleMatchVert;
        public bool IsMiddleMatchHorz;

        public bool IsObstacleUp;
        public PlayAreaCell CellObstacleUp;

        public bool IsObstacleDown;
        public PlayAreaCell CellObstacleDown;

        public bool IsObstacleLeft;
        public PlayAreaCell CellObstacleLeft;

        public bool IsObstacleRight;
        public PlayAreaCell CellObstacleRight;

    }


}



