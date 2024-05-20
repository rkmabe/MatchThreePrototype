using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.UI;
using UnityEngine;

namespace MatchThreePrototype.MatchReaction.MatchTypes
{
    public abstract class Match
    {
        //protected ScoreInfoBlock _scoreInfo;

        public ItemTypes ItemType;
        public int PlayerMoveNum;
        public int NumMatches;
        public bool IsBonusCatch;

        public abstract void GamePlayEvent();

        public abstract void ScoreMatch(ScoreInfoBlock scoreInfo);

    }
}