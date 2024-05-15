using MatchThreePrototype.PlayAreaCellContent;
using UnityEngine;
using UnityEngine.UI;
using MatchThreePrototype.Controllers;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{

    public class ItemRemovingState : IContentState
    {

        private float _secsRemovalProcessing = 0;

        internal static float DEFAULT_REMOVAL_DURATION = .5f;
        private float _removalDuration = DEFAULT_REMOVAL_DURATION;

        private ItemHandler _itemHandler;

        public void Enter()
        {
            _secsRemovalProcessing = 0;
        }

        public void Exit()
        {
            _itemHandler.FinishRemoval();
        }

        public void Update()
        {

            float alphaLerp;
            if (_secsRemovalProcessing < _removalDuration)
            {

                alphaLerp = Mathf.Lerp(Statics.ALPHA_ON, Statics.ALPHA_OFF, _secsRemovalProcessing / _removalDuration);


                Image image = _itemHandler.GetImage();
                image.color = new Color(image.color.r, image.color.g, image.color.b, alphaLerp);

                _secsRemovalProcessing += Time.deltaTime;
            }
            else
            {

                Image image = _itemHandler.GetImage();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

                _itemHandler.StateMachine.TransitionTo(_itemHandler.StateMachine.IdleState);

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

        public ItemRemovingState(ItemHandler itemHandler)
        {
            _itemHandler = itemHandler;
            SettingsController.OnNewRemoveDurationDelegate += OnNewRemoveDuration;
        }


    }
}