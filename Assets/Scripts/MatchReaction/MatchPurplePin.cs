using MatchThreePrototype.MatchReaction;
using MatchThreePrototype.UI;
using UnityEngine;

public class MatchPurplePin : Match
{

    public MatchPurplePin(ScoreInfoBlock scoreInfo)
    {
        _scoreInfo = scoreInfo;
    }

    public override void GamePlayEvent()
    {
        Debug.Log("PURPLE pin gameplay event");
    }

    public override void ScoreMatch()
    {
        _scoreInfo.UpdateNumPurplePins(NumMatches);
    }
}
