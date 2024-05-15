using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{
    public class PlayAreaColumn : MonoBehaviour
    {
        public int Number { get => _number; }
        [SerializeField] private int _number;  //1-N left to right

        public List<PlayAreaCell> Cells { get => _cells; }
        [SerializeField] private List<PlayAreaCell> _cells = new List<PlayAreaCell>();


        private PlayArea _playArea;

        private IRowInfoProvider _rowInfoProvider;

        private IDropCellHandler _dropCellHandler;

        private List<DropCell> _droppingCells = new List<DropCell>();

        private List<PlayAreaCell> _cellsToCatchMatches = new List<PlayAreaCell>();

        internal void UpdateStateMachines(out bool anyCellsProcessingRemoval)
        {

            // OR
            // for each statemachien in _cells[i].StateMachines - update     

            anyCellsProcessingRemoval = false;

            for (int i = _cells.Count - 1; i >= 0; i--)
            {
                _cells[i].ItemHandler.UpdateStateMachine();
                _cells[i].ObstacleHandler.UpdateStateMachine();
                _cells[i].BlockHandler.UpdateStateMachine();

                if (_cells[i].ObstacleHandler.GetIsProcessingRemoval() ||
                    _cells[i].BlockHandler.GetIsProcessingRemoval() || 
                    _cells[i].ItemHandler.GetIsProcessingRemoval() )
                {
                    anyCellsProcessingRemoval = true;
                }
            }
        }
        internal void UpdateStagedDrops(out bool anyCellsStaged)
        {
            anyCellsStaged = false;
            for (int i = _cells.Count - 1; i >= 0; i--)
            {

                if (_cells[i].ItemHandler.GetItem() == null  && _cells[i].ObstacleHandler.GetObstacle() == null && _cells[i].IsWaitingForDropCell == false && _cells[i].ItemHandler.GetIsProcessingRemoval() == false)
                {
                    //DropCell dropCell = _dropCellHandler.FindDropCell(_playArea, this, _cells[i], _rowInfoProvider);
                    DropCell dropCell = _dropCellHandler.FindDropCell(_cells[i]);
                    if (dropCell != null)
                    {
                        _cells[i].IsWaitingForDropCell = true;

                        PlayAreaRowInfo dropToInfo = _rowInfoProvider.GetRowInfo(_cells[i].Number);

                        dropCell.StartDropToTarget(dropToInfo, _cells[i]);

                        _droppingCells.Add(dropCell);

                        anyCellsStaged = true;
                    }
                }
            }
        }
        internal void UpdateDroppingCells(out bool anyCellsDropping)
        {
            anyCellsDropping = false;
            for (int i = _droppingCells.Count - 1; i >= 0; i--)
            {
                anyCellsDropping = true;
                bool hasDropArrived;
                _droppingCells[i].UpdateDropPosition(out hasDropArrived);
                if (hasDropArrived)
                {
                    _droppingCells[i].TransferContentsToCell();

                    _cellsToCatchMatches.Add(_droppingCells[i].TargetCell);

                    _droppingCells.RemoveAt(i);
                }
            }
        }
        internal void UpdateMatches(out bool anyMatchesCaught)
        {
            for (int i = _cellsToCatchMatches.Count-1; i >= 0 ; i--)
            {
                (List<PlayAreaCell> matchesCaught, List<PlayAreaCell> obstaclesCaught) = _cellsToCatchMatches[i].MatchDetector.CatchMatchThree(true);

                for (int j = 0; j < matchesCaught.Count; j++)
                {
                    matchesCaught[j].QueueItemForRemoval();
                    anyMatchesCaught = true;
                }

                for (int k = 0; k < obstaclesCaught.Count; k++)
                {
                    obstaclesCaught[k].QueueObstacleForRemoval();
                }

                _cellsToCatchMatches.RemoveAt(i);
            }
            anyMatchesCaught = false;
        }

        private void Awake()
        {
            _playArea = GetComponentInParent<PlayArea>();

            _rowInfoProvider = GetComponent<IRowInfoProvider>();

            _dropCellHandler = GetComponent<IDropCellHandler>();



        }

        // Start is called before the first frame update
        void Start()
        {
            // this MUST be done in start.  PlayAreaCell caches RectTransform in Awake.
            _rowInfoProvider.SetupRowInfo(_cells);

            _dropCellHandler.Setup(_playArea, this, _rowInfoProvider);
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}
