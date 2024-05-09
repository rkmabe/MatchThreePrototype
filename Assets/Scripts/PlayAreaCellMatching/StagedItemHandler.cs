using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellMatching
{

    public class StagedItemHandler : MonoBehaviour, IStagedItemHandler
    {

        internal Item StagedItem { get => _stagedItem; }
        private Item _stagedItem;

        internal bool MatchWithStagedItem { get => _matchWithStagedItem; }
        private bool _matchWithStagedItem = false;

        public void SetStagedItem(Item item)
        {
            _matchWithStagedItem = true;
            _stagedItem = item;
        }
        public void RemoveStagedItem()
        {
            _matchWithStagedItem = false;
            _stagedItem = null;
        }

        public Item GetStagedItem()
        {
            return _stagedItem;
        }

        public bool GetMatchWithStagedItem()
        {
            return _matchWithStagedItem;
        }
    }
}