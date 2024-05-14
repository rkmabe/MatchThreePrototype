using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public interface ICellIndicators
    {

        public void IndicateDragOverCell(PlayAreaCell dragOverCell);

        public void IndicateDragFromCell(PlayAreaCell dragFromCell);

        public void ClearDragIndicators();

    }
}
