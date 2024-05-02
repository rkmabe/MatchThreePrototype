using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent
{

    public interface IImageFader
    {
        // Start is called before the first frame update
        public void UpdateItemRemovalAnimation(Image image);
    }
}