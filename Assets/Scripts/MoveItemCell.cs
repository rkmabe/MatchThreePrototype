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

        public Item Item { get => _item; }
        private Item _item;  // ITEM currently in cell

        private Image _image;
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

        internal void SetItem(Item item)
        {
            _item = item;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);
            _image.sprite = item.Sprite;
        }
        private void RemoveItem()
        {
            _item = null;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
            _image.sprite = null;

            RemoveTarget();
        }

        internal void RemoveTarget()
        {
            _targetCell = null;
            _targetColNum = 0;
        }

        internal void ProcessOnArrival()
        {

            if (_item == null)
            {
                //Debug.LogError("ITEM should NOT be null on arrival!");
                //Debug.Break();
            }
            else
            {
                _targetCell.SetItem(_item);
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

            RemoveItem();
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
            _image = GetComponentInChildren<Image>();
            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();


            SettingsController.OnNewMoveSpeedDelegate  += OnNewMoveSpeed;
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
