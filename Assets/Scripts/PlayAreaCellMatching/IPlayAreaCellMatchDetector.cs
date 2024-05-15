using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaElements;
using System.Collections.Generic;

namespace MatchThreePrototype.PlayAreaCellMatching
{
    public interface IPlayAreaCellMatchDetector
    {

        public void Setup(PlayArea playArea, PlayAreaColumn column, PlayAreaCell cell, IItemHandler itemHandler, IStagedItemHandler stagedItemHandler);

        public PlayAreaCellMatches CheckAdjacentMatches();

        public (List<PlayAreaCell> ItemMatchesCaught, List<PlayAreaCell> ObstaclesCaught) CatchMatchThree(bool isDrop);

    }
}
