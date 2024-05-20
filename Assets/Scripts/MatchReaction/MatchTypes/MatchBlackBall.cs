using MatchThreePrototype.UI;

namespace MatchThreePrototype.MatchReaction.MatchTypes
{

    public class MatchBlackBall : Match
    {
        public override void GamePlayEvent()
        {
            //Debug.Log("BLACK ball gameplay event");
        }

        public override void ScoreMatch(ScoreInfoBlock scoreInfo)
        {
            scoreInfo.UpdateNumBlackBalls(NumMatches);
        }
    }
}