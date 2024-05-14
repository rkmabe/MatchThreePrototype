using System.Collections.Generic;
using UnityEngine;
using MatchThreePrototype.PlayAreaElements;

namespace MatchThreePrototype
{

    public class Player : MonoBehaviour
    {

        public PlayArea PlayArea { get => _playArea; set => _playArea = value; }
        private PlayArea _playArea;

        private PlayAreaCell _dragOriginCell = null;

        public int MoveNum { get => _moveNum; }


        private int _moveNum = 0;
        [SerializeField] private TMPro.TextMeshProUGUI _moveNumText;

        // objects for touch interaction with TouchDetector
        private Touch _dragTouch = default(Touch);
        private Vector2 _inputDownPosition = Statics.Vector2Zero();
        private Vector2 _inputUpPosition = Statics.Vector2Zero();



        public void OnTouchInputDown(Vector3 position)
        {
            _inputDownPosition = position;
        }
        public void OnTouchInputDrag(Touch dragTouch)
        {
            _dragTouch = dragTouch;
        }
        public void OnTouchInputUp(Vector3 position)
        {
            _inputUpPosition = position;
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
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            // process no input if play area in flux
            if (_playArea.IsInFlux)
            {
                _inputDownPosition = Statics.Vector2Zero();
                _inputUpPosition = Statics.Vector2Zero();
                _dragTouch = default(Touch);

                return;
            }

            if (_inputDownPosition != Statics.Vector2Zero())
            {
                // look for tile touched
                PlayAreaCell cell = _playArea.GetCellAtPosition(_inputDownPosition);
                if (cell != null)
                {
                    //Debug.Log("Finger down on " + cell.ColumnNumber + "," + cell.Number);

                    if (cell.ItemHandler.GetItem() != null && cell.ItemHandler.GetIsProcessingRemoval()==false && (cell.BlockHandler.GetBlock() == null && cell.ObstacleHandler.GetObstacle() == null))
                    {
                        _dragOriginCell = cell;

                        //_playArea.IndicateDragFromCell(cell);
                        _playArea.CellIndicators.IndicateDragFromCell(cell);

                        _playArea.HeldItemCell.transform.position = cell.transform.position;

                        // TODO: move HeldItemCell to Player .. assign when you assign play area .. 
                        _playArea.HeldItemCell.ItemHandler.SetItem(cell.ItemHandler.GetItem());
                    }
                }

                _inputDownPosition = Statics.Vector2Zero();
            }
            else if (_inputUpPosition != Statics.Vector2Zero())
            {
                if (_dragOriginCell != null)
                {

                    PlayAreaCell dragDestinationCell;
                    bool isDestinationWithinRange = _playArea.IsPositionInSwapRange(_inputUpPosition, _dragOriginCell, out dragDestinationCell);

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

                //_playArea.ClearDragIndicators();
                _playArea.CellIndicators.ClearDragIndicators();

                _playArea.HeldItemCell.ItemHandler.RemoveItem();

                _dragOriginCell = null;

                _inputUpPosition = Statics.Vector2Zero();
            }
            else if (!_dragTouch.Equals(default(Touch)))
            {
                //Debug.Log("Drag to " + _dragTouch.position);

                if (_dragOriginCell != null && _playArea.HeldItemCell.ItemHandler.GetItem() != null)
                {
                    PlayAreaCell dragOverCell;
                    if (_playArea.IsPositionInSwapRange(_dragTouch.position, _dragOriginCell, out dragOverCell))
                    {
                        _playArea.HeldItemCell.transform.position = _dragTouch.position;
                        if (dragOverCell != null)
                        {
                            //_playArea.IndicateDragOverCell(dragOverCell);
                            _playArea.CellIndicators.IndicateDragOverCell(dragOverCell);
                        }
                    }
                }

                _dragTouch = default(Touch);
            }
        }
    }
}
