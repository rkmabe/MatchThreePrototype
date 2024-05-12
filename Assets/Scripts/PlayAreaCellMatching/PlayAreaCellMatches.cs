using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using MatchThreePrototype.PlayAreaElements;

namespace MatchThreePrototype.PlayAreaCellMatching
{

    public struct PlayAreaCellMatches
    {
        public ItemTypes ItemType;

        public bool IsMatchUp;
        public PlayAreaCell CellMatchUp;

        public bool IsMatchDown;
        public PlayAreaCell CellMatchDown;

        public bool IsMatchLeft;
        public PlayAreaCell CellMatchLeft;

        public bool IsMatchRight;
        public PlayAreaCell CellMatchRight;

        public bool IsMiddleMatchVert;
        public bool IsMiddleMatchHorz;

        public bool IsObstacleUp;
        public PlayAreaCell CellObstacleUp;

        public bool IsObstacleDown;
        public PlayAreaCell CellObstacleDown;

        public bool IsObstacleLeft;
        public PlayAreaCell CellObstacleLeft;

        public bool IsObstacleRight;
        public PlayAreaCell CellObstacleRight;

    }
}
