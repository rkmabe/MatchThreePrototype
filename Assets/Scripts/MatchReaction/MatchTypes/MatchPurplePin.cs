using MatchThreePrototype.UI;

namespace MatchThreePrototype.MatchReaction.MatchTypes
{
    public class MatchPurplePin : Match
    {

        public override void GamePlayEvent()
        {
            //Debug.Log("PURPLE pin gameplay event");
        }

        public override void ScoreMatch(ScoreInfoBlock scoreInfo)
        {
            scoreInfo.UpdateNumPurplePins(NumMatches);
        }
    }
}