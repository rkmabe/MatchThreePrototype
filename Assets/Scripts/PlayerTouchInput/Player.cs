using MatchThreePrototype.PlayAreaElements;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayerTouchInput
{

    public class Player : MonoBehaviour
    {

        public PlayArea PlayArea { get => _playArea; set => _playArea = value; }
        private PlayArea _playArea;        

        private PlayAreaCell _dragOriginCell = null;

        public int MoveNum { get => _moveNum; }
        private int _moveNum = 0;
        [SerializeField] private TMPro.TextMeshProUGUI _moveNumText;

        ITouchInfoProvider _touchInfoProvider;

        public void OnTouchInputDown(Vector3 position)
        {
            if (!_playArea.IsInFlux)
            {
                ProcessFingerDown(position);
            }
        }

        public void OnTouchInputDrag(Touch dragTouch)
        {
            if (!_playArea.IsInFlux)
            {
                ProcessFingerDrag(dragTouch);
            }
        }
        public void OnTouchInputUp(Vector3 position)
        {
            if (!_playArea.IsInFlux)
            {
                ProcessFingerUp(position);
            }
        }

        private void ProcessFingerUp(Vector3 position)
        {
            if (_dragOriginCell != null)
            {

                PlayAreaCell dragDestinationCell;
                bool isDestinationWithinRange = _touchInfoProvider.IsPositionInSwapRange(position, _dragOriginCell, out dragDestinationCell);

                if (dragDestinationCell != null && dragDestinationCell.BlockHandler.GetBlock() == null && dragDestinationCell.ObstacleHandler.GetObstacle() == null && isDestinationWithinRange)
                {
                    //Debug.Log("Finger UP on " + cell.ColumnNumber + "," + cell.Number);

                    _moveNum++;
                    _moveNumText.text = "Move: " + _moveNum.ToString();

                    // there is ALWAYS an item in the ORIGIN cell, and there is ALWAYS a match at the destination cell.
                    // there is not necessarily an item in the DESTINATION.  There is NOT necessarily a MATCH at the ORGIN.                       

                    dragDestinationCell.StagedItemHandler.SetStagedItem(_dragOriginCell.ItemHandler.GetItem());
                    _dragOriginCell.StagedItemHandler.SetStagedItem(dragDestinationCell.ItemHandler.GetItem());

                    (List<PlayAreaCell> matchesCaughtAtDestination, List<PlayAreaCell> obstaclesCaughtAtDestination) = dragDestinationCell.MatchDetector.CatchMatchThree(false);

                    if (matchesCaughtAtDestination.Count > 0)
                    {

                        // TODO: this will change once out of prototype
                        //_moveNum++;
                        //_moveNumText.text = "Move: " + _moveNum.ToString();

                        // start at origin and move to destination ("drag from" position to "drag to" position)
                        _playArea.CellMoveToDestination.transform.position = _dragOriginCell.transform.position;
                        _playArea.CellMoveToDestination.SetTargetCell(dragDestinationCell);
                        _playArea.CellMoveToDestination.ItemHandler.SetItem(_dragOriginCell.ItemHandler.GetItem());       // _dragOriginCell.Item should NEVER be null
                        _playArea.CellMoveToDestination.SetCellMatchesCaught(matchesCaughtAtDestination);
                        _playArea.CellMoveToDestination.SetObstaclesCaught(obstaclesCaughtAtDestination);

                        _dragOriginCell.ItemHandler.RemoveItem();

                        // start at destination and move to origin ("drag to" position to "drag from" position)
                        if (dragDestinationCell.ItemHandler.GetItem() == null || dragDestinationCell.ItemHandler.GetIsProcessingRemoval())
                        {
                            _playArea.CellMoveToOrigin.RemoveTarget();   // used in PlayArea update - target MUST be cleared here!
                        }
                        else
                        {
                            (List<PlayAreaCell> matchesCaughtAtOrigin, List<PlayAreaCell> obstaclesCaughtAtOrigin) = _dragOriginCell.MatchDetector.CatchMatchThree(false);

                            _playArea.CellMoveToOrigin.transform.position = dragDestinationCell.transform.position;
                            _playArea.CellMoveToOrigin.SetTargetCell(_dragOriginCell);
                            _playArea.CellMoveToOrigin.ItemHandler.SetItem(dragDestinationCell.ItemHandler.GetItem());
                            _playArea.CellMoveToOrigin.SetCellMatchesCaught(matchesCaughtAtOrigin);
                            _playArea.CellMoveToOrigin.SetObstaclesCaught(obstaclesCaughtAtOrigin);

                            dragDestinationCell.ItemHandler.RemoveItem();
                        }

                    }
                    else
                    {
                        _moveNum--;
                        _moveNumText.text = "Moves: " + _moveNum.ToString();
                    }

                    dragDestinationCell.StagedItemHandler.RemoveStagedItem();
                    _dragOriginCell.StagedItemHandler.RemoveStagedItem();

                }

            }

            _playArea.CellIndicators.ClearDragIndicators();

            _playArea.HeldItemCell.ItemHandler.RemoveItem();

            _dragOriginCell = null;
        }

        private void ProcessFingerDown(Vector3 position)
        {
            PlayAreaCell cell = _touchInfoProvider.GetCellAtPosition(position);

            if (cell != null)
            {
                //Debug.Log("Finger down on " + cell.ColumnNumber + "," + cell.Number);

                if (cell.ItemHandler.GetItem() != null && cell.ItemHandler.GetIsProcessingRemoval() == false && (cell.BlockHandler.GetBlock() == null && cell.ObstacleHandler.GetObstacle() == null))
                {
                    _dragOriginCell = cell;

                    _playArea.CellIndicators.IndicateDragFromCell(cell);

                    _playArea.HeldItemCell.transform.position = cell.transform.position;

                    _playArea.HeldItemCell.ItemHandler.SetItem(cell.ItemHandler.GetItem());
                }
            }
        }

        private void ProcessFingerDrag(Touch dragTouch)
        {

            if (_dragOriginCell != null && _playArea.HeldItemCell.ItemHandler.GetItem() != null)
            {
                PlayAreaCell dragOverCell;
                if (_touchInfoProvider.IsPositionInSwapRange(dragTouch.position, _dragOriginCell, out dragOverCell))
                {
                    _playArea.HeldItemCell.transform.position = dragTouch.position;
                    if (dragOverCell != null)
                    {
                        _playArea.CellIndicators.IndicateDragOverCell(dragOverCell);
                    }
                }
                //else
                //{
                //    Debug.Log("not in range");
                //}
            }
        }


        private void OnDestroy()
        {
            TouchDetector.OnTouchInputDownDelegate -= OnTouchInputDown;
            TouchDetector.OnTouchInputUpDelegate -= OnTouchInputUp;
            TouchDetector.OnTouchInputDragDelegate -= OnTouchInputDrag;
        }

        private void Awake()
        {

            TouchDetector.OnTouchInputDownDelegate += OnTouchInputDown;
            TouchDetector.OnTouchInputUpDelegate += OnTouchInputUp;
            TouchDetector.OnTouchInputDragDelegate += OnTouchInputDrag;

            _touchInfoProvider = GetComponent<TouchInfoProvider>();

        }

        // Start is called before the first frame update
        void Start()
        {

        }

    }
}
