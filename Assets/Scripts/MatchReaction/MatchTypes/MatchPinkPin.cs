using MatchThreePrototype.UI;

namespace MatchThreePrototype.MatchReaction.MatchTypes
{
    public class MatchPinkPin : Match
    {
        public override void GamePlayEvent()
        {
            //Debug.Log("PINK pin gameplay event");
        }

        public override void ScoreMatch(ScoreInfoBlock scoreInfo)
        {
            scoreInfo.UpdateNumPinkPins(NumMatches);
        }
    }
}