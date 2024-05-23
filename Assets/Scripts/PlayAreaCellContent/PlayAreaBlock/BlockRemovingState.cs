using UnityEngine;
using UnityEngine.UI;
using MatchThreePrototype.Controllers;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock
{

    public class BlockRemovingState : IContentState
    {

        private float _secsRemovalProcessing = 0;

        internal static float DEFAULT_REMOVAL_DURATION = .5f;
        private float _removalDuration = DEFAULT_REMOVAL_DURATION;

        private BlockHandler _blockHandler;

        public void Enter()
        {
            _secsRemovalProcessing = 0;
        }

        public void Exit()
        {
            _blockHandler.FinishRemoval();
        }

        public void Update()
        {
            // TODO: change to transition to next block level, if any.
            // although only one level is currently in use, this is set up for mulitple levels.

            float alphaLerp;
            if (_secsRemovalProcessing < _removalDuration)
            {

                alphaLerp = Mathf.Lerp(Statics.BLOCK_ALPHA_ON, Statics.ALPHA_OFF, _secsRemovalProcessing / _removalDuration);

                Image image = _blockHandler.GetImage();
                image.color = new Color(image.color.r, image.color.g, image.color.b, alphaLerp);

                _secsRemovalProcessing += Time.deltaTime;
            }
            else
            {
                Image image = _blockHandler.GetImage();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

                _blockHandler.StateMachine.TransitionTo(_blockHandler.StateMachine.IdleState);

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

        public BlockRemovingState(BlockHandler blockHandler)
        {
            _blockHandler = blockHandler;
            SettingsController.OnNewRemoveDurationDelegate += OnNewRemoveDuration;
        }


    }
}