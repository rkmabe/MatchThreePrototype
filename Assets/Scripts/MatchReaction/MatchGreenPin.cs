using MatchThreePrototype.MatchReaction;
using UnityEngine;
using MatchThreePrototype.UI;

public class MatchGreenPin : Match
{
    public MatchGreenPin(ScoreInfoBlock scoreInfo)
    {
        _scoreInfo = scoreInfo;
    }

    public override void GamePlayEvent()
    {
        Debug.Log("GREEN pin gameplay event");
    }

    public override void ScoreMatch()
    {
        _scoreInfo.UpdateNumGreenPins(NumMatches);
    }
}
