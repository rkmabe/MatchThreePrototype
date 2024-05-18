using MatchThreePrototype.MatchReaction;
using UnityEngine;
using MatchThreePrototype.UI;

public class MatchPinkPin : Match
{
    public MatchPinkPin(ScoreInfoBlock scoreInfo)
    {        
        _scoreInfo = scoreInfo;
    }

    public override void GamePlayEvent()
    {
        Debug.Log("PINK pin gameplay event");
    }

    public override void ScoreMatch()
    {
        _scoreInfo.UpdateNumPinkPins(NumMatches);
    }
}
