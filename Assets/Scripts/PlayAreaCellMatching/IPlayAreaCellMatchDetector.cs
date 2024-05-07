using MatchThreePrototype.PlayAreaCellContent.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellMatching
{
    public interface IPlayAreaCellMatchDetector
    {

        public void Setup(PlayArea playArea, PlayAreaColumn column, PlayAreaCell cell, IPlayAreaItemHandler itemHandler, IStagedItemHandler stagedItemHandler);

        public PlayAreaCellMatches CheckAdjacentMatches();

        public (List<PlayAreaCell> ItemMatchesCaught, List<PlayAreaCell> ObstaclesCaught) CatchMatchThree(bool isDrop);

    }
}
