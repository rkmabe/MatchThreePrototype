using MatchThreePrototype.MatchDetection;
using MatchThreePrototype.PlayerTouchInput;
using MatchThreePrototype.UI;
using UnityEngine;

namespace MatchThreePrototype.MatchReaction
{
    public class MatchProcessor : MonoBehaviour
    {

        private Player _player;
        private ScoreInfoBlock _scoreInfo;

        //private List<MoveRecord> _moveRecords = new List<MoveRecord>();

        //private void OnMatchCaught(MatchRecord match)
        private void OnMatchCaught(Match match)
        {

            // add move record
            MoveRecord rec;
            rec.PlayerMoveNum = _player.MoveNum;
            rec.match = match;
            //_moveRecords.Add(rec);
            _scoreInfo.UpdateMoveText(rec);


            //match.MatchBase.GamePlayEvent();
            //match.MatchBase.ScoreMatch();
            match.ScoreMatch();
        }

        private void OnDestroy()
        {
            PlayAreaCellMatchDetector.OnMatchCaughtDelegate -= OnMatchCaught;
        }

        private void Awake()
        {
            PlayAreaCellMatchDetector.OnMatchCaughtDelegate += OnMatchCaught;

            _player = FindFirstObjectByType<Player>();
            _scoreInfo = FindFirstObjectByType<ScoreInfoBlock>();

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

    struct MoveRecord
    {
        public int PlayerMoveNum;
        public Match match;
    }

}
