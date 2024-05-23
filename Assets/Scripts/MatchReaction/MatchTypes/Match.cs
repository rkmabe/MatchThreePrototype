using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.UI;

namespace MatchThreePrototype.MatchReaction.MatchTypes
{
    public abstract class Match
    {

        public ItemTypes ItemType;
        public int PlayerMoveNum;
        public int NumMatches;
        public bool IsBonusCatch;

        public abstract void GamePlayEvent();

        public abstract void ScoreMatch(ScoreInfoBlock scoreInfo);

    }
}