using MatchThreePrototype.Controllers;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle;
using MatchThreePrototype.MatchDetection;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class PlayArea : MonoBehaviour
    {
        [SerializeField] private RectTransform _playAreaRect;


        [SerializeField] private List<PlayAreaColumn> _columns;

        public float CellAnchorsHeight { get => _cellAnchorsHeight; }
        [Header("Must match in Editor!")]
        [SerializeField] private float _cellAnchorsHeight = 0.11111f;

        //[Header("Must be UNIQUE!")]
        //[SerializeField] private List<ItemTypes> _allowedItemTypes;
        private List<ItemTypes> _allowedItemTypes;

        public MoveItemCell CellMoveToOrigin { get => _cellMoveToOrigin; }
        [SerializeField] private MoveItemCell _cellMoveToOrigin;

        public MoveItemCell CellMoveToDestination { get => _cellMoveToDestination; }
        [SerializeField] private MoveItemCell _cellMoveToDestination;

        public HeldItemCell HeldItemCell { get => _heldItemCell; }
        [SerializeField] private HeldItemCell _heldItemCell;

        [SerializeField] bool DebugCheckMatches = false;

        public bool EnableDebugBorderMatcher { get => _enableDebugBorderMatcher; }
        [SerializeField] bool _enableDebugBorderMatcher = false;


        private float _percentCellsToBlock;
        private float _percentCellsToObstruct;

        private ItemPool _itemPool;
        private BlockPool _blockPool;
        private ObstaclePool _obstaclePool;

        private int _numCells;

        public IDrawnItemsHandler DrawnItemsHandler { get => _drawnItemsHandler; }
        private IDrawnItemsHandler _drawnItemsHandler;

        private IPlayAreaPopulator _playAreaPopulator;

        public ICellIndicators CellIndicators { get => _cellIndicators; }
        private ICellIndicators _cellIndicators;

        private bool _isPopulated = false;

        private int _numCellsChecked = 0;
        private int _numMatch3s = 0;

        public bool IsInFlux { get => _isInFlux; }
        private bool _isInFlux = false;

        public delegate void OnCellCheckMatchRequest();
        public static OnCellCheckMatchRequest OnCellCheckMatchRequestDelegate;

        private SettingsController _settingsController;

        private void CheckAllCellMatches()
        {
            // currently only used in editor tests
            // can be used to cause all cells to check mathes, but
            // usually only cells in new positions must be checked

            _numCellsChecked = 0;
            PlayAreaCellMatchDetector.OnCellCheckMatchCompleteDelegate += OnCellCheckMatchComplete;

            // advertise that you want cells to check
            OnCellCheckMatchRequestDelegate();

        }
        private void OnCellCheckMatchComplete(bool isMatch3)
        {
            // currently only used in editor tests

            _numCellsChecked++;
            if (isMatch3)
            {
                _numMatch3s++;
            }
            if (_numCellsChecked == _numCells)
            {
                Debug.Log("All cells MATHCED");

                PlayAreaCellMatchDetector.OnCellCheckMatchCompleteDelegate -= OnCellCheckMatchComplete;
            }
        }

        private void Populate()
        {

            _drawnItemsHandler.Setup(_allowedItemTypes, _itemPool);

            _drawnItemsHandler.DrawItems(_numCells);

            _drawnItemsHandler.ShuffleItems();

            _playAreaPopulator.Setup(_drawnItemsHandler, _obstaclePool, _blockPool);

            _playAreaPopulator.PlaceItems(_columns);

            int numCellsToObstruct = Mathf.RoundToInt(_numCells * _percentCellsToObstruct);
            _playAreaPopulator.PlaceObstacles(numCellsToObstruct);
            
            int numCellsToBlock = Mathf.RoundToInt(_numCells * _percentCellsToBlock);
            _playAreaPopulator.PlaceBlocks(numCellsToBlock);

            _isPopulated = true;
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
            _itemPool = FindAnyObjectByType<ItemPool>();
            _blockPool = FindObjectOfType<BlockPool>();
            _obstaclePool = FindObjectOfType<ObstaclePool>();
            _settingsController = FindAnyObjectByType<SettingsController>();

            _numCells = GetCellCount();


            _drawnItemsHandler = GetComponent<IDrawnItemsHandler>();
            _playAreaPopulator = GetComponent<IPlayAreaPopulator>();

            _cellIndicators = GetComponent<ICellIndicators>();

        }
        private void OnDestroy()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

            if (DebugCheckMatches)
            {
                CheckAllCellMatches();
                DebugCheckMatches = false;
            }

            // do not process anything until populated
            // PROTOTYPE of play area population process.
            // In Production, this will be triggered when a level is loaded.
            if (!_isPopulated)
            {
                if (_itemPool.IsInitialized && _blockPool.IsInitialized && _obstaclePool.IsInitialized)
                {
                    _percentCellsToBlock = _settingsController.GetPctBlock();
                    _percentCellsToObstruct = _settingsController.GetPctObstacle();

                    _allowedItemTypes = PrototypeBuildItemTypes();
                    //_isSwapRangeLimited = _settingsController.GetLimitSwapRange();

                    Populate();
                }
                return;
            }

            //return;

            // PROCESS SWAPS-------------------------------------------------------
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
            bool anyMatchCaught = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                bool thisColumnMatchesCaught;
                _columns[i].UpdateMatches(out thisColumnMatchesCaught);
                if (thisColumnMatchesCaught)
                {
                    anyMatchCaught = true;
                }
            }
            if (anyMatchCaught)
            {
                _isInFlux = true;
                return;
            }

            // UPDATE STATE MACHINES----------CHECK FOR IN PROCESS REMOVALS--------
            bool anyColumnRemoving = false;
            for (int i = 0; i < _columns.Count; i++)
            {
                bool thisColumnRemoving;
                _columns[i].UpdateStateMachines(out thisColumnRemoving);
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
}