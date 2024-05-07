using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.Block
{

    public interface IPlayAreaBlockHandler
    {
        public void SetBlock(PlayAreaBlock block);

        public void RemoveBlockLevel();

        public PlayAreaBlock GetBlock();

        public Image GetImage();

    }
}
