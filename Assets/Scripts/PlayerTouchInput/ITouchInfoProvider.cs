using MatchThreePrototype.PlayAreaElements;
using UnityEngine;

namespace MatchThreePrototype.PlayerTouchInput
{

    public interface ITouchInfoProvider
    {
        public PlayAreaCell GetCellAtPosition(Vector2 tapPoint);

        public bool IsPositionInSwapRange(Vector2 touchPoint, PlayAreaCell dragOriginCell, out PlayAreaCell cellTouched);

    }
}