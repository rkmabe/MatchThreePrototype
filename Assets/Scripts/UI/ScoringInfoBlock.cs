using MatchThreePrototype.MatchReaction;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.UI
{

    public class ScoreInfoBlock : MonoBehaviour
    {


        [SerializeField] private TMPro.TextMeshProUGUI _infoText;
        [SerializeField] private Button _infoUpButton;
        [SerializeField] private Button _infoDownButton;
        private int _currInfoTextPageNum = 1;

        [SerializeField] private TMPro.TextMeshProUGUI _numWhitePinsText;
        private int _numWhitePins;

        [SerializeField] private TMPro.TextMeshProUGUI _numGreenPinsText;
        private int _numGreenPins;

        [SerializeField] private TMPro.TextMeshProUGUI _numBluePinsText;
        private int _numBluePins;

        [SerializeField] private TMPro.TextMeshProUGUI _numRedPinsText;
        private int _numRedPins;

        [SerializeField] private TMPro.TextMeshProUGUI _numPurplePinsText;
        private int _numPurplePins;

        [SerializeField] private TMPro.TextMeshProUGUI _numPinkPinsText;
        private int _numPinkPins;

        [SerializeField] private TMPro.TextMeshProUGUI _numBlackBallsText;
        private int _numBlackBalls;

        int _lastMoveNum = 0;

        internal void UpdateNumWhitePins(int numPins)
        {
            _numWhitePins += numPins;
            _numWhitePinsText.text = "W: " + _numWhitePins.ToString();
        }
        internal void UpdateNumGreenPins(int numPins)
        {
            _numGreenPins += numPins;
            _numGreenPinsText.text = "G: " + _numGreenPins.ToString();
        }
        internal void UpdateNumBluePins(int numPins)
        {
            _numBluePins += numPins;
            _numBluePinsText.text = "Bl: " + _numBluePins.ToString();
        }
        internal void UpdateNumRedPins(int numPins)
        {
            _numRedPins += numPins;
            _numRedPinsText.text = "R: " + _numRedPins.ToString();
        }
        internal void UpdateNumPurplePins(int numPins)
        {
            _numPurplePins += numPins;
            _numPurplePinsText.text = "Pu: " + _numPurplePins.ToString();
        }
        internal void UpdateNumPinkPins(int numPins)
        {
            _numPinkPins += numPins;
            _numPinkPinsText.text = "Pi: " + _numPinkPins.ToString();
        }
        internal void UpdateNumBlackBalls(int numBalls)
        {
            _numBlackBalls += numBalls;
            _numBlackBallsText.text = "Bb: " + _numBlackBalls.ToString();
        }

        internal void UpdateMoveText(MoveRecord rec)
        {
            if (rec.PlayerMoveNum != _lastMoveNum)
            {
                _infoText.text = string.Empty;
                _currInfoTextPageNum = 1;
            }

            string text = "Move=" + rec.PlayerMoveNum + ", Type=" + rec.match.ItemType + ", Num=" + rec.match.NumMatches + ", Bonus=" + rec.match.IsBonusCatch;


            if (_infoText.text == string.Empty)
            {
                _infoText.pageToDisplay = 1;
                _infoText.text = text;
            }
            else
            {
                _infoText.text += Environment.NewLine + text;
            }

            SetTextNavInteractable();

            _lastMoveNum = rec.PlayerMoveNum;
        }

        private void SetTextNavInteractable()
        {
            if (_infoText.textInfo.pageCount <= 1)
            {
                _infoDownButton.interactable = false;
                _infoUpButton.interactable = false;
            }
            else
            {
                if (_currInfoTextPageNum < _infoText.textInfo.pageCount &&
                    _currInfoTextPageNum > 1)
                {
                    _infoDownButton.interactable = true;
                    _infoUpButton.interactable = true;
                }
                else if (_currInfoTextPageNum < _infoText.textInfo.pageCount)
                {
                    _infoDownButton.interactable = true;
                    _infoUpButton.interactable = false;
                }
                else if (_currInfoTextPageNum > 1)
                {
                    _infoDownButton.interactable = false;
                    _infoUpButton.interactable = true;
                }
            }
        }

        public void OnInfoUpClick()
        {
            if (_currInfoTextPageNum > 1)
            {
                _currInfoTextPageNum--;
                _infoText.pageToDisplay = _currInfoTextPageNum;
                SetTextNavInteractable();
            }

        }
        public void OnInfoDownClick() 
        {
            if (_infoText.textInfo.pageCount > _currInfoTextPageNum)
            {
                _currInfoTextPageNum++;
                _infoText.pageToDisplay = _currInfoTextPageNum;
                SetTextNavInteractable();
            }
        }



        private void OnDestroy()
        {
        }

        private void Awake()
        {
            _infoUpButton.onClick.AddListener(OnInfoUpClick);
            _infoDownButton.onClick.AddListener(OnInfoDownClick);
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
