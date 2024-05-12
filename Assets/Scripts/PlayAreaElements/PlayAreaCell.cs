using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle;
using MatchThreePrototype.PlayAreaCellMatching;
using System;
using UnityEngine;
//using MatchThreePrototype.PlayAreaElements;

namespace MatchThreePrototype.PlayAreaElements
{
    public class PlayAreaCell : MonoBehaviour, IComparable<PlayAreaCell>
    {
        public int Number { get => _number; }
        public int ColumnNumber { get => _parentColumn.Number; }

        private TMPro.TextMeshProUGUI _debugText;

        [SerializeField] private int _number;  //1-N top to bottom

        private PlayArea _playArea;
        private PlayAreaColumn _parentColumn;

        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;

        public IObstacleHandler ObstacleHandler { get => _obstacleHandler; }
        private IObstacleHandler _obstacleHandler;

        public IBlockHandler BlockHandler { get => _blockHandler; }
        private IBlockHandler _blockHandler;

        public IPlayAreaCellMatchDetector MatchDetector { get => _matchDetector; }
        private IPlayAreaCellMatchDetector _matchDetector;

        public IStagedItemHandler StagedItemHandler { get => _stagedItemHandler; }
        private IStagedItemHandler _stagedItemHandler;

        internal bool IsWaitingForDropCell { get => _isWaitingForDropCell; set => _isWaitingForDropCell = value; }
        private bool _isWaitingForDropCell = false;

        public RectTransform RectTransform { get => _rectTransform; }
        private RectTransform _rectTransform;

        internal static float DEFAULT_REMOVAL_DURATION = .5f;

        public override string ToString()
        {
            return ColumnNumber + "," + _number + " Item=" + _itemHandler.GetItem() + ", StagedItem=" + _stagedItemHandler.GetStagedItem();
        }

        // PlayAreaColumn sorts cells DESCENDING by CELL NUMBER (ie row number)
        public int CompareTo(PlayAreaCell compareCell)
        {
            if (compareCell == null)
            {
                return 1;
            }
            else
            {
                //return this.Number.CompareTo(compareCell.Number); // ascending
                return compareCell.Number.CompareTo(this.Number); // descending
            }
        }

        internal void QueueItemForRemoval()
        {
            if (_blockHandler.GetBlock() != null)
            {
                if (_blockHandler.GetIsProcessingRemoval())
                {
                    //Debug.Log("already processing BLOCK removal! -" + _parentColumn.Number + ", " + _number);
                }
                else
                {
                    _blockHandler.StartRemoval();
                }
            }
            else
            {
                if (_itemHandler.GetIsProcessingRemoval())
                {
                    //Debug.Log("already processing ITEM removal! -" + _parentColumn.Number + ", " + _number);
                }
                else
                {
                    _itemHandler.StartRemoval();
                }
            }
        }

        internal void QueueObstacleForRemoval()
        {
            if (_obstacleHandler.GetObstacle() != null)
            {
                if (_obstacleHandler.GetIsProcessingRemoval())
                {
                    //Debug.Log("already processing OBSTACLE removal! -" + _parentColumn.Number + ", " + _number);
                }
                else
                {
                    _obstacleHandler.StartRemoval();
                }
            }
        }

        private void OnDestroy()
        { 

        }

        private void Awake()
        {
            //_rectTransform = GetComponentInChildren<RectTransform>();
            _rectTransform = GetComponent<RectTransform>();

            _playArea = GetComponentInParent<PlayArea>();

            _parentColumn = GetComponentInParent<PlayAreaColumn>();

            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (_debugText != null)
            {
                _debugText.text = _parentColumn.Number + ", " + _number;
            }


            _itemHandler = GetComponent<IItemHandler>();
            _obstacleHandler = GetComponent<IObstacleHandler>();
            _blockHandler = GetComponent<IBlockHandler>();

            _stagedItemHandler = GetComponent<IStagedItemHandler>();

            _matchDetector = GetComponent<IPlayAreaCellMatchDetector>();
            _matchDetector.Setup(_playArea, _parentColumn, this, _itemHandler, _stagedItemHandler);

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
