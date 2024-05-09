using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent
{

    public interface IContentState
    {
        public void Enter();
        public void Update();
        public void Exit();

    }
}
