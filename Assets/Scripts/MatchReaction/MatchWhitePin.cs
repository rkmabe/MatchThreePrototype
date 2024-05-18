using UnityEngine;
using MatchThreePrototype.UI;

namespace MatchThreePrototype.MatchReaction
{

    public class MatchWhitePin : Match
    {
        public MatchWhitePin(ScoreInfoBlock scoreInfo) 
        {
            _scoreInfo = scoreInfo;
        }


        public override void GamePlayEvent()
        {
            Debug.Log("WHITE pin gameplay event");

            //_scoreController.UpdateNumWhitePins(1);

        }

        public override void ScoreMatch()
        {
            _scoreInfo.UpdateNumWhitePins(NumMatches);
        }

    }
}