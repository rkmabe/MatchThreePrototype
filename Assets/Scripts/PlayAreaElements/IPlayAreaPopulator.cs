using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle;
using MatchThreePrototype.PlayAreaElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public interface IPlayAreaPopulator
    {
        public void PlaceItems(List<PlayAreaColumn> columns, IDrawnItemsHandler drawnItemsHandler);

        public void PlaceObstacles(int numCells, ObstaclePool obstaclePool);

        public void PlaceBlocks(int numCells, BlockPool blockPool);

    }
}