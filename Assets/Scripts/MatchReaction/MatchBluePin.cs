using MatchThreePrototype.MatchReaction;
using MatchThreePrototype.UI;
using UnityEngine;

public class MatchBluePin : Match
{
    public MatchBluePin(ScoreInfoBlock scoreInfo)
    {
        _scoreInfo = scoreInfo;
    }

    public override void GamePlayEvent()
    {
        Debug.Log("BLUE pin gameplay event");
    }

    public override void ScoreMatch()
    {
        _scoreInfo.UpdateNumBluePins(NumMatches);
    }
}
