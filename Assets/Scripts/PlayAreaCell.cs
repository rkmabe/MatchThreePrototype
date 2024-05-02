using MatchThreePrototype.PlayAreaManagment;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace MatchThreePrototype
{
    public class PlayAreaCell : MonoBehaviour, IComparable<PlayAreaCell> // IItemHandler,
    {

        private DebugBorderMatcher _borderMatcher;

        public int Number { get => _number; }
        public int ColumnNumber { get => _parentColumn.Number; }

        private TMPro.TextMeshProUGUI _debugText;

        [SerializeField] private int _number;  //1-N top to bottom

        private PlayArea _playArea;
        private PlayAreaColumn _parentColumn;

        public Obstacle Obstacle { get => _obstacle; }
        private Obstacle _obstacle;
        [SerializeField] private Image _obstacleImage;

        //public Content Obstacle { get => _obstacle; }
        //private Content _obstacle;

        public Block Block { get => _block; }
        private Block _block;
        [SerializeField] private Image _blockImage;

        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;


        internal Item StagedItem { get => _stagedItem; }
        private Item _stagedItem;

        internal bool MatchWithStagedItem { get => _matchWithStagedItem; }
        private bool _matchWithStagedItem = false;

        internal DropCell StagedDropCell { get => _stagedDropCell; }
        private DropCell _stagedDropCell;

        public delegate void OnCellCheckMatchComplete(bool isMatch3);
        public static OnCellCheckMatchComplete OnCellCheckMatchCompleteDelegate;

        public RectTransform RectTransform { get => _rectTransform; }
        private RectTransform _rectTransform;

        public delegate void OnMatchCaught(MatchRecord match);
        public static OnMatchCaught OnMatchCaughtDelegate;

        private BlockPool _blockPool;

        public bool IsProcessingBlockRemoval { get => _isProcessingBlockRemoval; }
        internal bool _isProcessingBlockRemoval;
        private float _secsBlockRemovalProcessing = 0;
        //private static float BLOCK_REMOVAL_DURATION = .5f; //2.25f; // .25f;
        private static float BLOCK_ALPHA_ON = .65f;

        public bool IsProcessingItemRemoval { get => _isProcessingItemRemoval; }
        internal bool _isProcessingItemRemoval;
        private float _secsItemRemovalProcessing = 0;

        public bool IsProcessingObstacleRemoval { get => _isProcessingObstacleRemoval; }
        internal bool _isProcessingObstacleRemoval;
        private float _secsObstacleRemovalProcessing = 0;

        internal static float DEFAULT_REMOVAL_DURATION = .5f;
        private float _removalDuration = DEFAULT_REMOVAL_DURATION;

        private static float ALPHA_ON = 1;
        private static float ALPHA_OFF = 0;


        public override string ToString()
        {
            return ColumnNumber + "," + _number + " Item=" + _itemHandler.GetItem() + ", StagedItem=" + _stagedItem;

        }


        internal void AddAdjacentObstacles(PlayAreaCellMatches m, List<PlayAreaCell> adjacentObstacleCells)
        {
            if (m.IsObstacleUp)
            {
                adjacentObstacleCells.Add(m.CellObstacleUp);
            }
            if (m.IsObstacleDown)
            {
                adjacentObstacleCells.Add(m.CellObstacleDown);
            }
            if (m.IsObstacleLeft)
            {
                adjacentObstacleCells.Add(m.CellObstacleLeft);
            }
            if (m.IsObstacleRight)
            {
                adjacentObstacleCells.Add(m.CellObstacleRight);
            }
        }

        internal (List<PlayAreaCell> ItemMatchesCaught, List<PlayAreaCell> ObstaclesCaught) CatchMatchThree(bool useBorderMatcher, bool isDrop)
        {
            // Look for matches in each cardinal direction.
            // if you have a match, "crawl" that direction finding additional matches until chain is broken

            List<PlayAreaCell> matchesCaught = new List<PlayAreaCell>();
            List<PlayAreaCell> obstaclesCaught = new List<PlayAreaCell>();

            PlayAreaCellMatches m1 = CheckAdjacentMatches(useBorderMatcher);


            // VERTICAL matches-------------------------------------------------------------------
            List<PlayAreaCell> vertCellMatches = new List<PlayAreaCell>();

            List<PlayAreaCell> vertObstaclesAdjacent = new List<PlayAreaCell>();
            AddAdjacentObstacles(m1, vertObstaclesAdjacent);

            if (m1.IsMatchUp)
            {
                vertCellMatches.Add(this);
                vertCellMatches.Add(m1.CellMatchUp);

                bool matchUpChainBroken = false;
                int cellNumUp = _number;
                while (!matchUpChainBroken)
                {
                    cellNumUp--;

                    PlayAreaCell cellUpChain = _playArea.GetPlayAreaCell(_parentColumn, cellNumUp);
                    PlayAreaCellMatches mUP = cellUpChain.CheckAdjacentMatches(useBorderMatcher);

                    if (mUP.IsMatchUp)
                    {
                        vertCellMatches.Add(mUP.CellMatchUp);
                    }
                    else
                    {
                        matchUpChainBroken = true;
                    }

                    AddAdjacentObstacles(mUP, vertObstaclesAdjacent);

                }
            }

            if (m1.IsMatchDown)
            {
                // this could have already been added if it matches the item ABOVE (if this is the middle of a match)
                // so check before adding it here again for a match BELOW
                if (!vertCellMatches.Contains(this))
                {
                    vertCellMatches.Add(this);
                }
                vertCellMatches.Add(m1.CellMatchDown);


                bool matchDownChainBroken = false;
                int cellNumDown = _number;
                while (!matchDownChainBroken)
                {
                    cellNumDown++;
                    PlayAreaCell cellDownChain = _playArea.GetPlayAreaCell(_parentColumn, cellNumDown);
                    PlayAreaCellMatches mDOWN = cellDownChain.CheckAdjacentMatches(useBorderMatcher);

                    if (mDOWN.IsMatchDown)
                    {
                        vertCellMatches.Add(mDOWN.CellMatchDown);
                    }
                    else
                    {
                        matchDownChainBroken = true;
                    }

                    AddAdjacentObstacles(mDOWN, vertObstaclesAdjacent);

                }
            }
            if (vertCellMatches.Count >= 3)
            {
                // vertical match 3 detected!
                matchesCaught.AddRange(vertCellMatches);

                MatchRecord match = new MatchRecord();
                match.ItemType = m1.ItemType;
                match.NumMatches = vertCellMatches.Count;
                match.IsBonusCatch = isDrop;

                OnMatchCaughtDelegate(match);

                //Debug.Log("VERT MATCH - " + match.ItemType);

                obstaclesCaught.AddRange(vertObstaclesAdjacent);

            }

            // HORIZTONTAL matches--------------------------------------------------------------------
            List<PlayAreaCell> horzCellMatches = new List<PlayAreaCell>();

            List<PlayAreaCell> horzObstaclesAdjacent = new List<PlayAreaCell>();
            AddAdjacentObstacles(m1, horzObstaclesAdjacent);

            if (m1.IsMatchLeft)
            {
                horzCellMatches.Add(this);
                horzCellMatches.Add(m1.CellMatchLeft);

                bool matchLeftChainBroken = false;
                int colNumLeft = _parentColumn.Number;
                while (!matchLeftChainBroken)
                {
                    colNumLeft--;
                    PlayAreaColumn columnLeft = _playArea.GetPlayAreaColumn(colNumLeft);
                    if (columnLeft != null)
                    {
                        PlayAreaCell cellLeftChain = _playArea.GetPlayAreaCell(columnLeft, _number);
                        if (cellLeftChain != null)
                        {
                            PlayAreaCellMatches mLEFT = cellLeftChain.CheckAdjacentMatches(useBorderMatcher);

                            if (mLEFT.IsMatchLeft)
                            {
                                horzCellMatches.Add(mLEFT.CellMatchLeft);
                            }
                            else
                            {
                                matchLeftChainBroken = true;
                            }

                            AddAdjacentObstacles(mLEFT, horzObstaclesAdjacent);

                        }
                    }
                }
            }
            if (m1.IsMatchRight)
            {
                // could have already been placed if THIS is in middle horiztonally
                if (!horzCellMatches.Contains(this))
                {
                    horzCellMatches.Add(this);
                }
                horzCellMatches.Add(m1.CellMatchRight);

                bool matchRightChainBroken = false;
                int colNumRight = _parentColumn.Number;
                while (!matchRightChainBroken)
                {
                    colNumRight++;
                    PlayAreaColumn columnRight = _playArea.GetPlayAreaColumn(colNumRight);
                    if (columnRight != null)
                    {
                        PlayAreaCell cellRightChain = _playArea.GetPlayAreaCell(columnRight, _number);
                        if (cellRightChain != null)
                        {
                            PlayAreaCellMatches mRIGHT = cellRightChain.CheckAdjacentMatches(useBorderMatcher);

                            if (mRIGHT.IsMatchRight)
                            {
                                horzCellMatches.Add(mRIGHT.CellMatchRight);
                            }
                            else
                            {
                                matchRightChainBroken = true;
                            }

                            AddAdjacentObstacles(mRIGHT, horzObstaclesAdjacent);

                        }
                    }
                }
            }
            if (horzCellMatches.Count >= 3)
            {
                // match could already be here if the cell is part of a vertical AND horiztontal match
                for (int i = 0; i < horzCellMatches.Count; i++)
                {
                    if (!matchesCaught.Contains(horzCellMatches[i]))
                    {
                        matchesCaught.Add(horzCellMatches[i]);
                    }
                }

                MatchRecord match = new MatchRecord();
                match.ItemType = m1.ItemType;
                match.NumMatches = horzCellMatches.Count;
                match.IsBonusCatch = isDrop;

                OnMatchCaughtDelegate(match);

                obstaclesCaught.AddRange(horzObstaclesAdjacent);

                //Debug.Log("HORZ MATCH - " + match.ItemType);

            }

            //return matchesCaught;
            return (matchesCaught, obstaclesCaught);
        }

        internal PlayAreaCellMatches CheckAdjacentMatches(bool useBorderMatcher)
        {
            PlayAreaCellMatches m = new PlayAreaCellMatches();
            m.IsMatchUp = false;
            m.CellMatchUp = null;

            m.IsMatchDown = false;
            m.CellMatchDown = null;

            m.IsMatchLeft = false;
            m.CellMatchLeft = null;

            m.IsMatchRight = false;
            m.CellMatchRight = null;

            m.IsMiddleMatchVert = false;
            m.IsMiddleMatchHorz = false;

            m.IsObstacleUp = false;
            m.CellObstacleUp = null;

            m.IsObstacleDown = false;
            m.CellObstacleDown = null;

            m.IsObstacleLeft = false;
            m.CellObstacleLeft = null;

            m.IsObstacleRight = false;
            m.CellObstacleRight = null;


            //Item thisCellItem = (_matchWithStagedItem) ? _stagedItem : _item;
            Item thisCellItem = (_matchWithStagedItem) ? _stagedItem : _itemHandler.GetItem();
            

            // if there is NO item in the cell, there are NO matches.
            if (thisCellItem == null)
            {
                return m;
            }

            m.ItemType = thisCellItem.ItemType;

            // MATCH UP?
            PlayAreaCell cellUp = _playArea.GetPlayAreaCell(_parentColumn, _number - 1);
            if (cellUp != null)
            {
                //Item itemUP = (cellUp.MatchWithStagedItem) ? cellUp.StagedItem : cellUp.Item;
                Item itemUP = (cellUp.MatchWithStagedItem) ? cellUp.StagedItem : cellUp.ItemHandler.GetItem();
                if (itemUP != null && !cellUp.IsProcessingItemRemoval)
                {
                    m.IsMatchUp = (itemUP.ItemType == m.ItemType) ? true : false;
                    if (m.IsMatchUp)
                    {
                        m.CellMatchUp = cellUp;
                    }
                }
                else if (cellUp.Obstacle != null && !cellUp.IsProcessingObstacleRemoval)
                {
                    m.IsObstacleUp = true;
                    m.CellObstacleUp = cellUp;
                }
            }

            // MATCH DOWN ?
            PlayAreaCell cellDOWN = _playArea.GetPlayAreaCell(_parentColumn, _number + 1);
            if (cellDOWN != null)
            {
                //Item itemDOWN = (cellDOWN.MatchWithStagedItem) ? cellDOWN.StagedItem : cellDOWN.Item;
                Item itemDOWN = (cellDOWN.MatchWithStagedItem) ? cellDOWN.StagedItem : cellDOWN.ItemHandler.GetItem();
                if (itemDOWN != null && !cellDOWN.IsProcessingItemRemoval)
                {
                    m.IsMatchDown = (itemDOWN.ItemType == m.ItemType) ? true : false;
                    if (m.IsMatchDown)
                    {
                        m.CellMatchDown = cellDOWN;
                    }
                }
                else if (cellDOWN.Obstacle != null && !cellDOWN.IsProcessingObstacleRemoval)
                {
                    m.IsObstacleDown = true;
                    m.CellObstacleDown = cellDOWN;
                }
            }

            // MATCH LEFT ?
            PlayAreaColumn colulmnLeft = _playArea.GetPlayAreaColumn(_parentColumn.Number - 1);
            if (colulmnLeft != null)
            {
                PlayAreaCell cellLEFT = _playArea.GetPlayAreaCell(colulmnLeft, _number);
                if (cellLEFT != null)
                {
                    //Item itemLEFT = (cellLEFT.MatchWithStagedItem) ? cellLEFT.StagedItem : cellLEFT.Item;
                    Item itemLEFT = (cellLEFT.MatchWithStagedItem) ? cellLEFT.StagedItem : cellLEFT.ItemHandler.GetItem();
                    if (itemLEFT != null && !cellLEFT.IsProcessingItemRemoval)
                    {
                        m.IsMatchLeft = (itemLEFT.ItemType == m.ItemType) ? true : false;
                        if (m.IsMatchLeft)
                        {
                            m.CellMatchLeft = cellLEFT;
                        }
                    }
                    else if (cellLEFT.Obstacle != null && !cellLEFT.IsProcessingObstacleRemoval)
                    {
                        m.IsObstacleLeft = true;
                        m.CellObstacleLeft = cellLEFT;
                    }
                }
            }

            // MATCH RIGHT ?
            PlayAreaColumn columnRight = _playArea.GetPlayAreaColumn(_parentColumn.Number + 1);
            if (columnRight != null)
            {
                PlayAreaCell cellRIGHT = _playArea.GetPlayAreaCell(columnRight, _number);
                if (cellRIGHT != null)
                {
                    //Item itemRIGHT = (cellRIGHT.MatchWithStagedItem) ? cellRIGHT.StagedItem : cellRIGHT.Item;
                    Item itemRIGHT = (cellRIGHT.MatchWithStagedItem) ? cellRIGHT.StagedItem : cellRIGHT.ItemHandler.GetItem();
                    if (itemRIGHT != null && !cellRIGHT.IsProcessingItemRemoval)
                    {
                        m.IsMatchRight = (itemRIGHT.ItemType == m.ItemType) ? true : false;
                        if (m.IsMatchRight)
                        {
                            m.CellMatchRight = cellRIGHT;
                        }
                    }
                    else if (cellRIGHT.Obstacle != null && !cellRIGHT.IsProcessingObstacleRemoval)
                    {
                        m.IsObstacleRight = true;
                        m.CellObstacleRight = cellRIGHT;
                    }
                }
            }

            // MIDDLE match?
            m.IsMiddleMatchVert = (m.IsMatchUp && m.IsMatchDown) ? true : false;
            m.IsMiddleMatchHorz = (m.IsMatchLeft && m.IsMatchRight) ? true : false;

            // highlight borders
            if (useBorderMatcher && _borderMatcher != null)
            {
                _borderMatcher.IsMatchUp = m.IsMatchUp;
                _borderMatcher.IsMatchDown = m.IsMatchDown;
                _borderMatcher.IsMatchLeft = m.IsMatchLeft;
                _borderMatcher.IsMatchRight = m.IsMatchRight;
                _borderMatcher.IsMiddleMatchVert = m.IsMiddleMatchVert;
                _borderMatcher.IsMiddleMatchHorz = m.IsMiddleMatchHorz;
            }

            return m;

        }

        internal void RemoveObstacle()
        {
            _obstacle = null;
            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, ALPHA_OFF);
            _obstacleImage.sprite = null;
        }
        internal void SetObstacle(Obstacle obstacle)
        {
            _obstacle = obstacle;
            _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, ALPHA_ON);
            _obstacleImage.sprite = obstacle.Sprite;
        }

        internal void SetBlock(Block block)
        {
            _block= block;
            _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, BLOCK_ALPHA_ON);
            _blockImage.sprite = block.CurrentSprite;
        }

        internal void RemoveBlockLevel()
        {

            bool allLevelsRemoved;
            _block.RemoveLevel(out allLevelsRemoved);
            if (allLevelsRemoved)
            {
                // TODO: restore this to prefab state before returning to pool .. 
                // any missing levels must be restored .. just cache them .. 
                _blockPool.Return(_block);
                _block = null;
            }
            else
            {
                _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, BLOCK_ALPHA_ON);
                _blockImage.sprite = _block.CurrentSprite;
            }
        }

        internal void SetStagedDropCell(DropCell dropCell)
        {
            _stagedDropCell = dropCell;
        }
        internal void RemoveStagedDropCell()
        {
            _stagedDropCell = null;
        }

        internal void SetStagedItem(Item item)
        {
            _matchWithStagedItem = true;
            _stagedItem = item;
        }
        internal void RemoveStagedItem()
        {
            _matchWithStagedItem = false;
            _stagedItem = null;
        }

        private void OnCellCheckMatchRequest()
        {
            PlayAreaCellMatches m = CheckAdjacentMatches(true);

            bool isMatch3 = (m.IsMiddleMatchHorz || m.IsMiddleMatchVert) ? true : false;

            OnCellCheckMatchCompleteDelegate(isMatch3);
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
            float alphaLerp;
            if (_secsObstacleRemovalProcessing < _removalDuration)
            {
                alphaLerp = Mathf.Lerp(ALPHA_ON, ALPHA_OFF, _secsObstacleRemovalProcessing / _removalDuration);
                _obstacleImage.color = new Color(_obstacleImage.color.r, _obstacleImage.color.g, _obstacleImage.color.b, alphaLerp);

                _secsObstacleRemovalProcessing += Time.deltaTime;
            }
            else
            {
                _obstacleImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, 0);
                _obstacleImage.sprite = null;

                _isProcessingObstacleRemoval = false;
            }
            isComplete = !_isProcessingObstacleRemoval;
        }

        internal void UpdateBlockRemovalAnimation(out bool isComplete)
        {
            float alphaLerp;
            if (_secsBlockRemovalProcessing < _removalDuration)
            {
                alphaLerp = Mathf.Lerp(BLOCK_ALPHA_ON, ALPHA_OFF, _secsBlockRemovalProcessing / _removalDuration);
                _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, alphaLerp);

                _secsBlockRemovalProcessing += Time.deltaTime;
            }
            else
            {
                _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, 0);
                _blockImage.sprite = null;

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
                alphaLerp = Mathf.Lerp(ALPHA_ON, ALPHA_OFF, _secsItemRemovalProcessing / _removalDuration);
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

            if (_block == null)
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

            if (_obstacle != null)
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
            PlayArea.OnCellCheckMatchRequestDelegate -= OnCellCheckMatchRequest;

            SettingsController.OnNewRemoveDurationDelegate -= OnNewRemoveDuration;

        }

        private void Awake()
        {

            PlayArea.OnCellCheckMatchRequestDelegate += OnCellCheckMatchRequest;

            SettingsController.OnNewRemoveDurationDelegate += OnNewRemoveDuration;

            _rectTransform = GetComponentInChildren<RectTransform>();

            _playArea = GetComponentInParent<PlayArea>();

            _parentColumn = GetComponentInParent<PlayAreaColumn>();

            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (_debugText != null)
            {
                _debugText.text = _parentColumn.Number + ", " + _number;
            }

            _borderMatcher = GetComponentInChildren<DebugBorderMatcher>();

            _blockPool = FindAnyObjectByType<BlockPool>();


            _itemHandler = GetComponent<ItemHandler>();

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
