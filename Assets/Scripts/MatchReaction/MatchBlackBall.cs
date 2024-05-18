using MatchThreePrototype.MatchReaction;
using MatchThreePrototype.UI;
using UnityEngine;

public class MatchBlackBall : Match
{
    public MatchBlackBall(ScoreInfoBlock scoreInfo)
    {
        _scoreInfo = scoreInfo;
    }

    public override void GamePlayEvent()
    {
        Debug.Log("BLACK ball gameplay event");
    }

    public override void ScoreMatch()
    {
        _scoreInfo.UpdateNumBlackBalls(NumMatches);
    }
}
