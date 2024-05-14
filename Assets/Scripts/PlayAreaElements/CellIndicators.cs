using MatchThreePrototype.PlayAreaElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaElements
{

    public class CellIndicators : MonoBehaviour, ICellIndicators
    {

        [SerializeField] private GameObject _destinationCellIndicator;
        private Image _destinationCellIndicatorImage;

        [SerializeField] private GameObject _originCellIndicator;
        private Image _originCellIndicatorImage;

        public void IndicateDragOverCell(PlayAreaCell dragOverCell)
        {
            _destinationCellIndicator.transform.position = dragOverCell.transform.position;
            _destinationCellIndicatorImage.color = new Color(_destinationCellIndicatorImage.color.r, _destinationCellIndicatorImage.color.g, _destinationCellIndicatorImage.color.b, 1);
        }

        public void IndicateDragFromCell(PlayAreaCell dragFromCell)
        {
            _originCellIndicator.transform.position = dragFromCell.transform.position;
            _originCellIndicatorImage.color = new Color(_originCellIndicatorImage.color.r, _originCellIndicatorImage.color.g, _originCellIndicatorImage.color.b, 1);
        }

        public void ClearDragIndicators()
        {
            _originCellIndicatorImage.color = new Color(_originCellIndicatorImage.color.r, _originCellIndicatorImage.color.g, _originCellIndicatorImage.color.b, 0);
            _destinationCellIndicatorImage.color = new Color(_destinationCellIndicatorImage.color.r, _destinationCellIndicatorImage.color.g, _destinationCellIndicatorImage.color.b, 0);
        }

        private void Awake()
        {
            _originCellIndicatorImage = _originCellIndicator.GetComponentInChildren<Image>();
            _destinationCellIndicatorImage = _destinationCellIndicator.GetComponentInChildren<Image>();
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
