using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock
{

    public interface IBlockHandler
    {
        public void SetBlock(Block block);

        public void RemoveBlockLevel();

        public Block GetBlock();

        public Image GetImage();

        public void StartRemoval();
        public bool GetIsProcessingRemoval();
        public void FinishRemoval();

        public void UpdateStateMachine();

    }
}
