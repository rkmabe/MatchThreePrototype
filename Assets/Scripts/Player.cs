using System.Collections.Generic;
using UnityEngine;

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

                    //_dragOriginCell = cell;

                    if (cell.Item != null && (cell.Block == null && cell.Obstacle == null))
                    {
                        _dragOriginCell = cell;

                        _playArea.IndicateDragFromCell(cell);

                        _playArea.HeldItemCell.transform.position = cell.transform.position;
                        _playArea.HeldItemCell.SetItem(cell.Item);
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

                    if (dragDestinationCell != null && dragDestinationCell.Block == null && dragDestinationCell.Obstacle == null && isDestinationWithinRange)
                    {
                        //Debug.Log("Finger UP on " + cell.ColumnNumber + "," + cell.Number);

                        _moveNum++;
                        _moveNumText.text = "Move: " + _moveNum.ToString();

                        // there is ALWAYS an item in the ORIGIN cell, and there is ALWAYS a match at the destination cell.
                        // there is not necessarily an item in the DESTINATION.  There is NOT necessarily a MATCH at the ORGIN.                       
                        dragDestinationCell.SetStagedItem(_dragOriginCell.Item);
                        _dragOriginCell.SetStagedItem(dragDestinationCell.Item);

                        (List<PlayAreaCell> matchesCaughtAtDestination, List<PlayAreaCell> obstaclesCaughtAtDestination) = dragDestinationCell.CatchMatchThree(false, false);

                        //Debug.Log("Dest ObsNum=" + obstaclesCaughtAtDestination.Count);

                        if (matchesCaughtAtDestination.Count > 0)
                        {
                            // start at origin and move to destination ("drag from" position to "drag to" position)
                            _playArea.CellMoveToDestination.transform.position = _dragOriginCell.transform.position;
                            _playArea.CellMoveToDestination.SetTargetCell(dragDestinationCell);
                            _playArea.CellMoveToDestination.SetItem(_dragOriginCell.Item);       // _dragOriginCell.Item should NEVER be null
                            _playArea.CellMoveToDestination.SetCellMatchesCaught(matchesCaughtAtDestination);

                            _playArea.CellMoveToDestination.SetObstaclesCaught(obstaclesCaughtAtDestination);

                            _dragOriginCell.RemoveItem();

                            // start at destination and move to origin ("drag to" position to "drag from" position)
                            if (dragDestinationCell.Item == null)
                            {
                                _playArea.CellMoveToOrigin.RemoveTarget();   // used in PlayArea update - target MUST be cleared here!
                            }
                            else
                            {
                                (List<PlayAreaCell> matchesCaughtAtOrigin, List<PlayAreaCell> obstaclesCaughtAtOrigin) = _dragOriginCell.CatchMatchThree(false, false);

                                _playArea.CellMoveToOrigin.transform.position = dragDestinationCell.transform.position;
                                _playArea.CellMoveToOrigin.SetTargetCell(_dragOriginCell);
                                _playArea.CellMoveToOrigin.SetItem(dragDestinationCell.Item);
                                _playArea.CellMoveToOrigin.SetCellMatchesCaught(matchesCaughtAtOrigin);

                                _playArea.CellMoveToOrigin.SetObstaclesCaught(obstaclesCaughtAtOrigin);

                                dragDestinationCell.RemoveItem();
                            }
                        }
                        else
                        {
                            _moveNum--;
                            _moveNumText.text = "Moves: " + _moveNum.ToString();
                        }

                        dragDestinationCell.RemoveStagedItem();
                        _dragOriginCell.RemoveStagedItem();

                    }

                }

                _playArea.ClearDragIndicators();

                _playArea.HeldItemCell.RemoveItem();

                _dragOriginCell = null;


                _inputUpPosition = Statics.Vector2Zero();
            }
            else if (!_dragTouch.Equals(default(Touch)))
            {
                //Debug.Log("Drag to " + _dragTouch.position);

                if (_dragOriginCell != null && _playArea.HeldItemCell.Item != null)
                {
                    PlayAreaCell dragOverCell;
                    if (_playArea.IsPositionInSwapRange(_dragTouch.position, _dragOriginCell, out dragOverCell))
                    {
                        _playArea.HeldItemCell.transform.position = _dragTouch.position;
                        if (dragOverCell != null)
                        {
                            _playArea.IndicateDragOverCell(dragOverCell);
                        }
                    }
                }

                _dragTouch = default(Touch);
            }
        }
    }
}
