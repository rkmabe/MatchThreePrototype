using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaManagment
{

    public interface IImageFader
    {
        // Start is called before the first frame update
        public void UpdateItemRemovalAnimation(Image image);
    }
}