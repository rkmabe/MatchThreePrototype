using UnityEngine;
using UnityEngine.UI;
using MatchThreePrototype.Controllers;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle
{

    public class ObstacleRemovingState : IContentState
    {

        private float _secsRemovalProcessing = 0;

        internal static float DEFAULT_REMOVAL_DURATION = .5f;
        private float _removalDuration = DEFAULT_REMOVAL_DURATION;

        private ObstacleHandler _obstacleHandler;

        public void Enter()
        {
            _secsRemovalProcessing = 0;
        }

        public void Exit()
        {
            _obstacleHandler.FinishRemoval();
        }

        public void Update()
        {

            float alphaLerp;
            if (_secsRemovalProcessing < _removalDuration)
            {
                alphaLerp = Mathf.Lerp(Statics.ALPHA_ON, Statics.ALPHA_OFF, _secsRemovalProcessing / _removalDuration);

                Image image = _obstacleHandler.GetImage();
                image.color = new Color(image.color.r, image.color.g, image.color.b, alphaLerp);

                _secsRemovalProcessing += Time.deltaTime;
            }
            else
            {
                Image image = _obstacleHandler.GetImage();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

                _obstacleHandler.StateMachine.TransitionTo(_obstacleHandler.StateMachine.IdleState);
            }

        }

        internal void OnNewRemoveDuration(float duration)
        {
            _removalDuration = duration;
        }

        internal void CleanUpOnDestroy()
        {
            SettingsController.OnNewRemoveDurationDelegate -= OnNewRemoveDuration;
        }

        public ObstacleRemovingState(ObstacleHandler obstacleHandler)
        {
            _obstacleHandler = obstacleHandler;
            SettingsController.OnNewRemoveDurationDelegate += OnNewRemoveDuration;
        }


    }
}