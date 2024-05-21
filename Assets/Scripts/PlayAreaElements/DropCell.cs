using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle;
using MatchThreePrototype.Controllers;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{
    public class DropCell : MonoBehaviour 
    {
        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;

        public IObstacleHandler ObstacleHandler { get => _obstacleHandler; }
        private IObstacleHandler _obstacleHandler;

        private TMPro.TextMeshProUGUI _debugText;

        public float RectMinY { get => _rectTransform.anchorMin.y; }
        public float RectMaxY { get => _rectTransform.anchorMax.y; }
        private RectTransform _rectTransform;

        public float EditorRectMinY { get => _editorRectMinY; }// { get => _rectTransformEditor.anchorMin.y; }
        public float EditorRectMaxY { get => _editorRectMaxY; } //{ get => _rectTransformEditor.anchorMax.y; }

        private float _editorRectMinY;
        private float _editorRectMaxY;


        internal static float DEFAULT_DROP_SPEED = 2500;//250;
        private float _dropSpeed = DEFAULT_DROP_SPEED;

        public bool IsDropping { get => _isDropping; }
        private bool _isDropping = false;

        public PlayAreaCell TargetCell { get => _targetCell; }
        private PlayAreaCell _targetCell;

        internal void StartDropToTarget(PlayAreaRowInfo rowInfo, PlayAreaCell targetCell)
        {
            _targetCell = targetCell;

            _isDropping = true;
        }

        internal void SetDropFromPosition(float minY, float maxY)
        {
            _rectTransform.anchorMin = new Vector2(_rectTransform.anchorMin.x, minY);
            _rectTransform.anchorMax = new Vector2(_rectTransform.anchorMax.x, maxY);
        }
        internal void SetDropFromPosition(PlayAreaRowInfo rowInfo)
        {
            _rectTransform.anchorMin = new Vector2(_rectTransform.anchorMin.x, rowInfo.MinY);
            _rectTransform.anchorMax = new Vector2(_rectTransform.anchorMax.x, rowInfo.MaxY);
        }

        public override string ToString()
        {
            //return base.ToString() + "Item=" + _item + " Obsatcle=" + _obstacle;
            return base.ToString() + "Item=" + _itemHandler.GetItem() + " Obsatcle=" + _obstacleHandler.GetObstacle();
        }

        internal void UpdateDropPosition(out bool hasArrived)
        {
            if (Statics.IsCloseEnough(_rectTransform.position, _targetCell.RectTransform.position))
            {
                _isDropping = false;
            }
            else
            {
                _rectTransform.position = Vector2.MoveTowards(_rectTransform.position, _targetCell.RectTransform.position, _dropSpeed * Time.deltaTime);
            }

            hasArrived = !(_isDropping);
        }

        internal void TransferContentsToCell()
        {
            _targetCell.IsWaitingForDropCell = false;


            if (_itemHandler.GetItem() != null)
            {
                _targetCell.ItemHandler.SetItem(_itemHandler.GetItem());
                _itemHandler.RemoveItem();
            }
            else if (_obstacleHandler.GetObstacle() != null)
            {
                _targetCell.ObstacleHandler.SetObstacle(_obstacleHandler.GetObstacle());
                _obstacleHandler.RemoveObstacle();
            }

            // return drop item to home position
            _rectTransform.anchorMin = new Vector2(_rectTransform.anchorMin.x, _editorRectMinY);
            _rectTransform.anchorMax = new Vector2(_rectTransform.anchorMax.x, _editorRectMaxY);
            _rectTransform.anchoredPosition = Statics.Vector2Zero();
        }


        internal void OnNewDropSpeed(float speed)
        {
            _dropSpeed = speed;
            //Debug.Log(speed);
        }

        private void OnDestroy()
        {
            SettingsController.OnNewDropSpeedDelegate -= OnNewDropSpeed;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _editorRectMinY = _rectTransform.anchorMin.y;
            _editorRectMaxY = _rectTransform.anchorMax.y;

            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();

            SettingsController.OnNewDropSpeedDelegate += OnNewDropSpeed;

            _itemHandler = GetComponent<IItemHandler>();

            _obstacleHandler = GetComponent<IObstacleHandler>();

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
