using UnityEngine;
using MatchThreePrototype.UI;

namespace MatchThreePrototype.MatchReaction.MatchTypes
{

    public class MatchWhitePin : Match
    {

        public override void GamePlayEvent()
        {
            //Debug.Log("WHITE pin gameplay event");

        }

        public override void ScoreMatch(ScoreInfoBlock scoreInfo)
        {
            scoreInfo.UpdateNumWhitePins(NumMatches);
        }

    }
}