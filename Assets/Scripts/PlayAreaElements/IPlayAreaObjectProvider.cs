using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public interface IPlayAreaObjectProvider 
    {
        public PlayAreaColumn GetPlayAreaColumn(List<PlayAreaColumn> columns, int columnNum);

        public PlayAreaCell GetPlayAreaCell(PlayAreaColumn column, int cellNum);
    }
}
