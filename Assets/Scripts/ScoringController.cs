using MatchThreePrototype.PlayAreaCellMatching;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MatchThreePrototype.PlayAreaCellContent;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;

namespace MatchThreePrototype
{

    public class ScoringController : MonoBehaviour
    {


        [SerializeField] private TMPro.TextMeshProUGUI _infoText;
        [SerializeField] private Button _infoUpButton;
        [SerializeField] private Button _infoDownButton;
        private int _currInfoTextPageNum = 1;
        //[SerializeField] private TMPro.TextMeshProUGUI _infoTextPageCount;

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

        private Player _player;

        private List<MatchRecord> _matchRecords = new List<MatchRecord>();
        private List<MoveRecord> _moveRecords = new List<MoveRecord>();

        int _lastMoveNum = 0;

        private void OnMatchCaught(MatchRecord match)
        {

            //Debug.Log("_moveAttemptID=" + _moveAttemptID);


            if (_lastMoveNum != _player.MoveNum)
            {
                // new move - clear LastMove detail text
                _infoText.text = string.Empty;

                //_infoText.textInfo.Clear();

                _currInfoTextPageNum = 1;
            }


            if (match.ItemType == ItemTypes.WhitePin)
            {
                _numWhitePins += match.NumMatches;
                _numWhitePinsText.text = "W: " + _numWhitePins.ToString();
            }
            else if (match.ItemType == ItemTypes.GreenPin)
            {
                _numGreenPins += match.NumMatches;
                _numGreenPinsText.text = "G: " + _numGreenPins.ToString();
            }
            else if (match.ItemType == ItemTypes.BluePin)
            {
                _numBluePins += match.NumMatches;
                _numBluePinsText.text = "Bl: " + _numBluePins.ToString();
            }
            else if (match.ItemType == ItemTypes.RedPin)
            {
                _numRedPins += match.NumMatches;
                _numRedPinsText.text = "R: " + _numRedPins.ToString();
            }
            else if (match.ItemType == ItemTypes.PurplePin)
            {
                _numPurplePins += match.NumMatches;
                _numPurplePinsText.text = "Pu: " + _numPurplePins.ToString();
            }
            else if (match.ItemType == ItemTypes.PinkPin)
            {
                _numPinkPins += match.NumMatches;
                _numPinkPinsText.text = "Pi: " + _numPinkPins.ToString();
            }
            else if (match.ItemType == ItemTypes.BlackBall)
            {
                _numBlackBalls += match.NumMatches;
                _numBlackBallsText.text = "Bb: " + _numBlackBalls.ToString();
            }

            match.PlayerMoveNum = _player.MoveNum;
            _matchRecords.Add(match);



            if (_infoText.text == string.Empty)
            {
                _infoText.text = match.ToString();
            }
            else
            {
                _infoText.text += Environment.NewLine + match.ToString();
            }
            SetTextNavInteractable();

            _lastMoveNum = _player.MoveNum;
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
            PlayAreaCellMatchDetector.OnMatchCaughtDelegate -= OnMatchCaught;
        }

        private void Awake()
        {
            PlayAreaCellMatchDetector.OnMatchCaughtDelegate += OnMatchCaught;

            _player = FindAnyObjectByType<Player>();

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





    public struct MoveRecord
    {
        public int MoveNumber;
        public int MoveAttemptID;

        public int TotalItemsRemoved;
        public int MatchesRemoved;

    }

    public class MatchRecord
    {
        public int PlayerMoveNum;

        public ItemTypes ItemType;
        public int NumMatches;
        public bool IsBonusCatch;

        public override string ToString()
        {
            return "Move=" + PlayerMoveNum +  ", Type=" + ItemType + ", Num=" + NumMatches + ", Bonus=" + IsBonusCatch;
        }
    }

}
