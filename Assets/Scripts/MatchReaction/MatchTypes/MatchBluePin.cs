using MatchThreePrototype.UI;

namespace MatchThreePrototype.MatchReaction.MatchTypes
{

    public class MatchBluePin : Match
    {
        public override void GamePlayEvent()
        {
            //Debug.Log("BLUE pin gameplay event");
        }


        public override void ScoreMatch(ScoreInfoBlock scoreInfo)
        {
            scoreInfo.UpdateNumBluePins(NumMatches);
        }
    }
}