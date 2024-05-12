using MatchThreePrototype.PlayAreaElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public interface IRowInfoProvider
    {
        public PlayAreaRowInfo GetRowInfo(int rowNum);
        public void SetupRowInfo(List<PlayAreaCell> cells);
    }
}
