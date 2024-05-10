using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle
{

    public interface IObstacleHandler
    {
        public void SetObstacle(Obstacle obstacle);

        public void RemoveObstacle();

        public Obstacle GetObstacle();

        public Image GetImage();

        public bool CanDrop();


        public void StartRemoval();
        public bool GetIsProcessingRemoval();
        public void FinishRemoval();

        public void UpdateStateMachine();

    }
}
