using UnityEngine;
using MatchThreePrototype.UI;

namespace MatchThreePrototype.MatchReaction
{
    public class MatchRedPin : Match
    {

        public MatchRedPin(ScoreInfoBlock scoreInfo)
        {
            _scoreInfo = scoreInfo;
        }

        public override void GamePlayEvent()
        {
            Debug.Log("RED pin gameplay event");
        }

        public override void ScoreMatch()
        {
            _scoreInfo.UpdateNumRedPins(NumMatches);
        }

    }
}
