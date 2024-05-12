using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatchThreePrototype.PlayAreaElements;

namespace MatchThreePrototype.PlayAreaCellMatching
{
    public interface IPlayAreaCellMatchDetector
    {

        public void Setup(PlayArea playArea, PlayAreaColumn column, PlayAreaCell cell, IItemHandler itemHandler, IStagedItemHandler stagedItemHandler);

        public PlayAreaCellMatches CheckAdjacentMatches();

        public (List<PlayAreaCell> ItemMatchesCaught, List<PlayAreaCell> ObstaclesCaught) CatchMatchThree(bool isDrop);

    }
}
