using MatchThreePrototype.PlayAreaCellContent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellMatching
{

    public interface IStagedItemHandler
    {
        public void SetStagedItem(PlayAreaItem item);

        public void RemoveStagedItem();

        public PlayAreaItem GetStagedItem();

        public bool GetMatchWithStagedItem();
    }
}
