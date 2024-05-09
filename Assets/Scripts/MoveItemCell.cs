using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class MoveItemCell : MonoBehaviour
    {

        public bool IsMoving { get => _isMoving; }
        private bool _isMoving = false;

        public int TargetColumn { get => _targetColNum; }
        private int _targetColNum;

        //public PlayAreaCell TargetCell { get => _targetCell; }
        private PlayAreaCell _targetCell;

        private List<PlayAreaCell> _cellMatchesCaught = new List<PlayAreaCell>();

        private List<PlayAreaCell> _obstaclesCaught = new List<PlayAreaCell>();


        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;


        private TMPro.TextMeshProUGUI _debugText;

        private RectTransform _rectTransform;

        //private RectTransform _targetRectTransform;

        //private static float SWAP_SPEED = 3000;//300;


        internal static float DEFAULT_MOVE_SPEED = 3000;
        private float _moveSpeed = DEFAULT_MOVE_SPEED;

        //internal void SetCellsToRemove(List<PlayAreaCell> _cells)
        internal void SetCellMatchesCaught(List<PlayAreaCell> cellMatchesCaught)
        {
            _cellMatchesCaught = cellMatchesCaught;
        }
        internal void SetObstaclesCaught(List<PlayAreaCell> obstaclesCaught)
        {
            _obstaclesCaught = obstaclesCaught;
        }

        internal void SetTargetCell(PlayAreaCell cell)
        {
            _targetCell = cell;
            _targetColNum = cell.ColumnNumber;

            //_targetRectTransform = cell.RectTransform;

            _isMoving = true;
        }

        internal void RemoveTarget()
        {
            _targetCell = null;
            _targetColNum = 0;
        }

        internal void ProcessOnArrival()
        {

            //if (_item == null)
            if (_itemHandler.GetItem() == null)
            {
                Debug.LogError("ITEM should NOT be null on arrival!");
                //Debug.Break();
            }
            else
            {
                _targetCell.ItemHandler.SetItem(_itemHandler.GetItem());
            }

            for (int i = _cellMatchesCaught.Count-1; i >= 0; i--)
            {
                _cellMatchesCaught[i].QueueItemForRemoval();
                _cellMatchesCaught.RemoveAt(i);
            }

            for (int j = _obstaclesCaught.Count - 1; j >= 0; j--)
            {
                _obstaclesCaught[j].QueueObstacleForRemoval();
                _obstaclesCaught.RemoveAt(j);
            }

            //RemoveItem();
            _itemHandler.RemoveItemReferenceAndImage();
                RemoveTarget();
        }


        internal void UpdatePosition(out bool hasArrived)
        {
            if (Statics.IsCloseEnough(_rectTransform.position, _targetCell.RectTransform.position))
            {
                _isMoving = false;
            }
            else
            {
                _rectTransform.position = Vector2.MoveTowards(_rectTransform.position, _targetCell.RectTransform.position, _moveSpeed * Time.deltaTime);
            }

            hasArrived = !(_isMoving);
        }


        internal void OnNewMoveSpeed(float speed)
        {
            _moveSpeed = speed;
        }

        private void OnDestroy()
        {
            SettingsController.OnNewMoveSpeedDelegate -= OnNewMoveSpeed;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();

            SettingsController.OnNewMoveSpeedDelegate  += OnNewMoveSpeed;

            _itemHandler = GetComponent<IItemHandler>();
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
