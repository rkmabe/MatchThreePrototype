using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{


    public class DebugBorderMatcher : MonoBehaviour
    {

        [SerializeField] private Image _matchUp;
        [SerializeField] private Image _matchLeft;
        [SerializeField] private Image _matchRight;
        [SerializeField] private Image _matchDown;

        private static Color MatchColor = Color.yellow;
        private static Color MiddleMatchColor = Color.cyan;

        internal bool IsMatchUp
        {
            //get { return _isMatchUp; }
            set
            {
                //_isMatchUp = value;
                if (value)
                {
                    _matchUp.color = MatchColor;
                    _matchUp.gameObject.SetActive(true);
                }
                else
                {
                    _matchUp.gameObject.SetActive(false);
                }
            }
        }
        //private bool _isMatchUp;

        internal bool IsMatchLeft
        {
            //get { return _isMatchLeft; }
            set
            {
                //_isMatchLeft = value;
                if (value)
                {
                    _matchLeft.color = MatchColor;
                    _matchLeft.gameObject.SetActive(true);
                }
                else
                {
                    _matchLeft.gameObject.SetActive(false);
                }
            }
        }
        //private bool _isMatchLeft;

        internal bool IsMatchRight
        {
            //get { return _isMatchRight; }
            set
            {
                //_isMatchRight = value;
                if (value)
                {
                    _matchRight.color = MatchColor;
                    _matchRight.gameObject.SetActive(true);
                }
                else
                {
                    _matchRight.gameObject.SetActive(false);
                }
            }
        }
        //private bool _isMatchRight;

        internal bool IsMatchDown
        {
            //get { return _isMatchDown; }
            set
            {
                //_isMatchDown = value;
                //if (_isMatchDown)
                if (value)
                {
                    _matchDown.color = MatchColor;
                    _matchDown.gameObject.SetActive(true);
                }
                else
                {
                    _matchDown.gameObject.SetActive(false);
                }
            }
        }
        //private bool _isMatchDown;


        internal bool IsMiddleMatchVert
        {
            //get { return _isMiddleMatchVert; }
            set
            {
                //_isMiddleMatchVert = value;
                if (value)
                {
                    _matchUp.color = MiddleMatchColor;
                    _matchDown.color = MiddleMatchColor;
                }
                else
                {
                    //_matchDown.gameObject.SetActive(false);
                }
            }
        }
        //private bool _isMiddleMatchVert;

        internal bool IsMiddleMatchHorz
        {
            //get { return _isMiddleMatchHorz; }
            set
            {
                //_isMiddleMatchHorz = value;
                if (value)
                {
                    _matchLeft.color = MiddleMatchColor;
                    _matchRight.color = MiddleMatchColor;
                }
                else
                {
                    //_matchDown.gameObject.SetActive(false);
                }
            }
        }
        //private bool _isMiddleMatchHorz;

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
