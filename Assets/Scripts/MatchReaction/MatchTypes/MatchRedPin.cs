using MatchThreePrototype.UI;

namespace MatchThreePrototype.MatchReaction.MatchTypes
{
    public class MatchRedPin : Match
    {
        public override void GamePlayEvent()
        {
            //Debug.Log("RED pin gameplay event");
        }

        public override void ScoreMatch(ScoreInfoBlock scoreInfo)
        {
            scoreInfo.UpdateNumRedPins(NumMatches);
        }

    }
}
