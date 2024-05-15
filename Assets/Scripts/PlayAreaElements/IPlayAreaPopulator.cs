using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle;
using System.Collections.Generic;

namespace MatchThreePrototype.PlayAreaElements
{

    public interface IPlayAreaPopulator
    {

        public void Setup(IDrawnItemsHandler drawnItemsHandler, ObstaclePool obstaclePool, BlockPool blockPool);

        public void PlaceItems(List<PlayAreaColumn> columns);

        public void PlaceObstacles(int numCells);

        public void PlaceBlocks(int numCells);

    }
}