using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent
{

    public interface IPlayAreaBlockHandler
    {
        public void SetBlock(Block block);

        public void RemoveBlockLevel();

        public Block GetBlock();

        public Image GetImage();

    }

}
