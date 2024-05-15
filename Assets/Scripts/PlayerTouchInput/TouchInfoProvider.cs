using MatchThreePrototype.PlayAreaElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MatchThreePrototype.Controllers;

namespace MatchThreePrototype.PlayerTouchInput
{

    public class TouchInfoProvider : MonoBehaviour, ITouchInfoProvider
    {



        private PointerEventData _dummyEventData = new PointerEventData(null);

        [SerializeField] private GraphicRaycaster _graphicRaycaster;
        [SerializeField] int _cellSwapRange = 1;

        private SettingsController _settingsController;



        public bool IsPositionInSwapRange(Vector2 touchPoint, PlayAreaCell dragOriginCell, out PlayAreaCell cellTouched)
        {
            // return true if this drag position contains a play area cell within swap range

            //int _cellSwapRange = 1;

            cellTouched = null;
            bool withinPlayArea = false;

            _dummyEventData.position = touchPoint;
            List<RaycastResult> results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(_dummyEventData, results);
            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    // one row of resuts should contain a play area cell.

                    PlayAreaCell cellAtPosition = results[i].gameObject.GetComponentInParent<PlayAreaCell>();
                    if (cellAtPosition != null)
                    {
                        cellTouched = cellAtPosition;
                    }

                    if (results[i].gameObject.tag == Statics.PLAY_AREA_RECT)
                    {
                        withinPlayArea = true;
                    }
                }
            }

            //if (_isSwapRangeLimited)
            if (_settingsController.GetLimitSwapRange())
            {
                if (cellTouched != null)
                {
                    // if the swap range is limited, the cell touched must be within swap range
                    int rowMin = dragOriginCell.Number - _cellSwapRange;
                    int rowMax = dragOriginCell.Number + _cellSwapRange;

                    int colMin = dragOriginCell.ColumnNumber - _cellSwapRange;
                    int colMax = dragOriginCell.ColumnNumber + _cellSwapRange;

                    if ((cellTouched.Number >= rowMin && cellTouched.Number <= rowMax) &&
                        (cellTouched.ColumnNumber >= colMin && cellTouched.ColumnNumber <= colMax))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // no touched cell - we cannot be in range
                    return false;
                }

            }
            else
            {
                // if swap range is not limited this is a valid drag posiition if it is within the play area rect
                return withinPlayArea;
            }

        }

        public PlayAreaCell GetCellAtPosition(Vector2 tapPoint)
        {
            _dummyEventData.position = tapPoint;

            List<RaycastResult> results = new List<RaycastResult>();

            _graphicRaycaster.Raycast(_dummyEventData, results);

            for (int i = 0; i < results.Count; i++)
            {
                PlayAreaCell cellAtPosition = results[i].gameObject.GetComponentInParent<PlayAreaCell>();
                if (cellAtPosition != null)
                {
                    return cellAtPosition;
                }
            }

            return null;
        }

        private void Awake()
        {
            _settingsController = FindAnyObjectByType<SettingsController>();
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
