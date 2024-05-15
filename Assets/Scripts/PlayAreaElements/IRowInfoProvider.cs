using System.Collections.Generic;

namespace MatchThreePrototype.PlayAreaElements
{

    public interface IRowInfoProvider
    {
        public PlayAreaRowInfo GetRowInfo(int rowNum);
        public void SetupRowInfo(List<PlayAreaCell> cells);
    }
}
