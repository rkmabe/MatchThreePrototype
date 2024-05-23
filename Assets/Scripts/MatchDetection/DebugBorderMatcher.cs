using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.MatchDetection
{
    public class DebugBorderMatcher : MonoBehaviour
    {

        [SerializeField] private Image _matchUp;
        [SerializeField] private Image _matchLeft;
        [SerializeField] private Image _matchRight;
        [SerializeField] private Image _matchDown;

        private static Color MatchColor = Color.yellow;
        private static Color MiddleMatchColor = Color.cyan;

        internal void HighlightMatches(PlayAreaCellMatches m)
        {

            ClearHighlights();

            IsMatchUp = m.IsMatchUp;
            IsMatchDown = m.IsMatchDown;
            IsMatchLeft = m.IsMatchLeft;
            IsMatchRight = m.IsMatchRight;
            IsMiddleMatchVert = m.IsMiddleMatchVert;
            IsMiddleMatchHorz = m.IsMiddleMatchHorz;
        }

        private void ClearHighlights()
        {
            _matchUp.color = Color.clear;
            _matchUp.gameObject.SetActive(false);

            _matchDown.color = Color.clear;
            _matchDown.gameObject.SetActive(false);

            _matchLeft.color = Color.clear;
            _matchLeft.gameObject.SetActive(false);

            _matchRight.color = Color.clear;
            _matchRight.gameObject.SetActive(false);
        }

        private bool IsMatchUp
        {
            set
            {
                if (value)
                {
                    _matchUp.color = MatchColor;
                    _matchUp.gameObject.SetActive(true);
                }
            }
        }

        private bool IsMatchLeft
        {
            set
            {
                if (value)
                {
                    _matchLeft.color = MatchColor;
                    _matchLeft.gameObject.SetActive(true);
                }
            }
        }

        private bool IsMatchRight
        {
            set
            {
                if (value)
                {
                    _matchRight.color = MatchColor;
                    _matchRight.gameObject.SetActive(true);
                }
            }
        }

        private bool IsMatchDown
        {
            set
            {
                if (value)
                {
                    _matchDown.color = MatchColor;
                    _matchDown.gameObject.SetActive(true);
                }
            }
        }

        private bool IsMiddleMatchVert
        {
            set
            {
                if (value)
                {
                    _matchUp.color = MiddleMatchColor;
                    _matchDown.color = MiddleMatchColor;
                }
            }
        }

        private bool IsMiddleMatchHorz
        {
            set
            {
                if (value)
                {
                    _matchLeft.color = MiddleMatchColor;
                    _matchRight.color = MiddleMatchColor;
                }
            }
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
