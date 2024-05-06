using MatchThreePrototype.PlayAreaCellContent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellMatching
{

    public class StagedItemHandler : MonoBehaviour, IStagedItemHandler
    {

        internal PlayAreaItem StagedItem { get => _stagedItem; }
        private PlayAreaItem _stagedItem;

        internal bool MatchWithStagedItem { get => _matchWithStagedItem; }
        private bool _matchWithStagedItem = false;

        public void SetStagedItem(PlayAreaItem item)
        {
            _matchWithStagedItem = true;
            _stagedItem = item;
        }
        public void RemoveStagedItem()
        {
            _matchWithStagedItem = false;
            _stagedItem = null;
        }

        public PlayAreaItem GetStagedItem()
        {
            return _stagedItem;
        }

        public bool GetMatchWithStagedItem()
        {
            return _matchWithStagedItem;
        }
    }
}