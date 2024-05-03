using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent
{

    public interface IPlayAreaObstacleHandler
    {
        public void SetObstacle(PlayAreaObstacle obstacle);

        public void RemoveObstacle();

        public PlayAreaObstacle GetObstacle();

        public Image GetImage();

        public bool CanDrop();

    }
}
