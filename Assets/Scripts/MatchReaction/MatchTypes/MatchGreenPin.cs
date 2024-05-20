using MatchThreePrototype.UI;

namespace MatchThreePrototype.MatchReaction.MatchTypes
{

    public class MatchGreenPin : Match
    {
        public override void GamePlayEvent()
        {
            //Debug.Log("GREEN pin gameplay event");
        }

        public override void ScoreMatch(ScoreInfoBlock scoreInfo)
        {
            scoreInfo.UpdateNumGreenPins(NumMatches);
        }
    }
}
