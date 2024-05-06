using MatchThreePrototype.PlayAreaCellContent;
using MatchThreePrototype.PlayAreaCellMatching;
using System;
using UnityEngine;

namespace MatchThreePrototype
{
    public class PlayAreaCell : MonoBehaviour, IComparable<PlayAreaCell>
    {
        public int Number { get => _number; }
        public int ColumnNumber { get => _parentColumn.Number; }

        private TMPro.TextMeshProUGUI _debugText;

        [SerializeField] private int _number;  //1-N top to bottom

        private PlayArea _playArea;
        private PlayAreaColumn _parentColumn;

        public IPlayAreaItemHandler ItemHandler { get => _itemHandler; }
        private IPlayAreaItemHandler _itemHandler;

        public IPlayAreaObstacleHandler ObstacleHandler { get => _obstacleHandler; }
        private IPlayAreaObstacleHandler _obstacleHandler;

        public IPlayAreaBlockHandler BlockHandler { get => _blockHandler; }
        private IPlayAreaBlockHandler _blockHandler;

        public IPlayAreaCellMatchDetector MatchDetector { get => _matchDetector; }
        private IPlayAreaCellMatchDetector _matchDetector;

        public IStagedItemHandler StagedItemHandler { get => _stagedItemHandler; }
        private IStagedItemHandler _stagedItemHandler;

        internal DropCell StagedDropCell { get => _stagedDropCell; }
        private DropCell _stagedDropCell;

        public RectTransform RectTransform { get => _rectTransform; }
        private RectTransform _rectTransform;

        private BlockPool _blockPool;

        public bool IsProcessingBlockRemoval { get => _isProcessingBlockRemoval; }
        internal bool _isProcessingBlockRemoval;
        private float _secsBlockRemovalProcessing = 0;

        public bool IsProcessingItemRemoval { get => _isProcessingItemRemoval; }
        internal bool _isProcessingItemRemoval;
        private float _secsItemRemovalProcessing = 0;

        public bool IsProcessingObstacleRemoval { get => _isProcessingObstacleRemoval; }
        internal bool _isProcessingObstacleRemoval;
        private float _secsObstacleRemovalProcessing = 0;

        internal static float DEFAULT_REMOVAL_DURATION = .5f;
        private float _removalDuration = DEFAULT_REMOVAL_DURATION;

        public override string ToString()
        {
            return ColumnNumber + "," + _number + " Item=" + _itemHandler.GetItem() + ", StagedItem=" + _stagedItemHandler.GetStagedItem();
        }

        internal void SetStagedDropCell(DropCell dropCell)
        {
            _stagedDropCell = dropCell;
        }
        internal void RemoveStagedDropCell()
        {
            _stagedDropCell = null;
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


        internal void UpdateObstacleRemovalAnimation(out bool isComplete)
        {
            //float alphaLerp;
            //if (_secsObstacleRemovalProcessing < _removalDuration)
            //{
            //    alphaLerp = Mathf.Lerp(Statics.ALPHA_ON, Statics.ALPHA_OFF, _secsObstacleRemovalProcessing / _removalDuration);
            //    _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, alphaLerp);

            //    _secsObstacleRemovalProcessing += Time.deltaTime;
            //}
            //else
            //{
            //    _obstacleImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, 0);
            //    _obstacleImage.sprite = null;

            //    _isProcessingObstacleRemoval = false;
            //}
            //isComplete = !_isProcessingObstacleRemoval;


            float alphaLerp;
            if (_secsObstacleRemovalProcessing < _removalDuration)
            {
                alphaLerp = Mathf.Lerp(Statics.ALPHA_ON, Statics.ALPHA_OFF, _secsObstacleRemovalProcessing / _removalDuration);
                _obstacleHandler.GetImage().color = new Color(_obstacleHandler.GetImage().color.r, _obstacleHandler.GetImage().color.g, _obstacleHandler.GetImage().color.b, alphaLerp);

                _secsObstacleRemovalProcessing += Time.deltaTime;
            }
            else
            {
                //_obstacleHandler.GetImage().color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, 0);
                _obstacleHandler.GetImage().color = new Color(_blockHandler.GetImage().color.r, _blockHandler.GetImage().color.g, _blockHandler.GetImage().color.b, 0);
                _obstacleHandler.GetImage().sprite = null;

                _isProcessingObstacleRemoval = false;
            }
            isComplete = !_isProcessingObstacleRemoval;

        }

        internal void UpdateBlockRemovalAnimation(out bool isComplete)
        {
            float alphaLerp;
            if (_secsBlockRemovalProcessing < _removalDuration)
            {
                alphaLerp = Mathf.Lerp(Statics.BLOCK_ALPHA_ON, Statics.ALPHA_OFF, _secsBlockRemovalProcessing / _removalDuration);
                _blockHandler.GetImage().color = new Color(_blockHandler.GetImage().color.r, _blockHandler.GetImage().color.g, _blockHandler.GetImage().color.b, alphaLerp);

                _secsBlockRemovalProcessing += Time.deltaTime;
            }
            else
            {
                _blockHandler.GetImage().color = new Color(_blockHandler.GetImage().color.r, _blockHandler.GetImage().color.g, _blockHandler.GetImage().color.b, 0);
                _blockHandler.GetImage().sprite = null;

                _isProcessingBlockRemoval = false;
            }

            isComplete = !_isProcessingBlockRemoval;
        }

        internal void UpdateItemRemovalAnimation()
        {
            //float alphaLerp;
            //if (_secsItemRemovalProcessing < _removalDuration)
            //{
            //    _itemHandler.geti

            //    alphaLerp = Mathf.Lerp(ALPHA_ON, ALPHA_OFF, _secsItemRemovalProcessing / _removalDuration);
            //    _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, alphaLerp);

            //    _secsItemRemovalProcessing += Time.deltaTime;
            //}
            //else
            //{
            //    _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, 0);
            //    _itemImage.sprite = null;

            //    _isProcessingItemRemoval = false;
            //}

            float alphaLerp;
            if (_secsItemRemovalProcessing < _removalDuration)
            {                
                alphaLerp = Mathf.Lerp(Statics.ALPHA_ON, Statics.ALPHA_OFF, _secsItemRemovalProcessing / _removalDuration);
                _itemHandler.GetImage().color = new Color(_itemHandler.GetImage().color.r, _itemHandler.GetImage().color.g, _itemHandler.GetImage().color.b, alphaLerp);

                _secsItemRemovalProcessing += Time.deltaTime;
            }
            else
            {
                _itemHandler.GetImage().color = new Color(_itemHandler.GetImage().color.r, _itemHandler.GetImage().color.g, _itemHandler.GetImage().color.b, 0);
                _itemHandler.GetImage().sprite = null;

                _isProcessingItemRemoval = false;
            }


        }


        internal void QueueItemForRemoval()
        {
            if (_isProcessingItemRemoval)
            {
                //Debug.Log("already processing removal! -" + _parentColumn.Number + ", " + _number);
                return;
            }

            // TODO: remove once you feel safe from this..
            //if (_item == null)
            //{
            //    Debug.LogError("item already null! - " + _parentColumn.Number + ", " + _number);
            //}
            //if (_itemImage.sprite == null)
            //{
            //    Debug.LogError("sprite is already null! - " + _parentColumn.Number + ", " + _number);
            //}

            //if (_block == null)
            if (_blockHandler.GetBlock() == null)
            {
                _isProcessingItemRemoval = true;
                _secsItemRemovalProcessing = 0;
            }
            else
            {
                _isProcessingBlockRemoval = true;
                _secsBlockRemovalProcessing = 0;
            }
        }

        internal void QueueObstacleForRemoval()
        {
            if (_isProcessingObstacleRemoval)
            {
                //Debug.Log("already processing OBSTACLE removal! -" + _parentColumn.Number + ", " + _number);
                return;
            }
            //if (_obstacle == null)
            //{
            //    Debug.LogError("OBSTACLE already null! - " + _parentColumn.Number + ", " + _number);
            //}

            //if (_obstacle != null)
            if (_obstacleHandler.GetObstacle() != null)
            {
                _isProcessingObstacleRemoval = true;
                _secsObstacleRemovalProcessing = 0;
            }
        }


        internal void OnNewRemoveDuration(float duration)
        {
            _removalDuration = duration;
        }

        private void OnDestroy()
        { 
            SettingsController.OnNewRemoveDurationDelegate -= OnNewRemoveDuration;
        }

        private void Awake()
        {

            SettingsController.OnNewRemoveDurationDelegate += OnNewRemoveDuration;

            _rectTransform = GetComponentInChildren<RectTransform>();

            _playArea = GetComponentInParent<PlayArea>();

            _parentColumn = GetComponentInParent<PlayAreaColumn>();

            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (_debugText != null)
            {
                _debugText.text = _parentColumn.Number + ", " + _number;
            }

            _blockPool = FindAnyObjectByType<BlockPool>();


            _itemHandler = GetComponent<IPlayAreaItemHandler>();
            _obstacleHandler = GetComponent<IPlayAreaObstacleHandler>();
            _blockHandler = GetComponent<IPlayAreaBlockHandler>();

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
