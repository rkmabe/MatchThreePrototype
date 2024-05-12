using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public interface IDropCellHandler
    {
        public DropCell FindDropCell(PlayArea playArea, PlayAreaColumn column, PlayAreaCell cell, IRowInfoProvider rowInfoProvider);
    }

}
