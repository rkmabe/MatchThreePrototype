using MatchThreePrototype.PlayAreaCellContent;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle
{

    public class ObstacleIdleState : IContentState
    {
        private ObstacleHandler _obstacleHandler;

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {

            if (_obstacleHandler.GetObstacle() == null)
            {
                return;
            }

            // .. periodically blink, sparkle, etc

            if (_obstacleHandler.IsProcessingRemoval)
            {
                _obstacleHandler.StateMachine.TransitionTo(_obstacleHandler.StateMachine.RemovingState);
            }
        }

        public ObstacleIdleState(ObstacleHandler obstacleHandler)
        {
            _obstacleHandler = obstacleHandler;
        }
    }
}