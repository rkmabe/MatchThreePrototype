using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{

    public class ItemIdleState : IContentState
    {
        private ItemHandler _itemHandler;

        public void Enter()
        {
            //throw new System.NotImplementedException();
            //Image image = _itemHandler.GetImage();
            //_itemHandler.GetImage().color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_ON);
            //image.color = new Color(image.color.r, image.color.g, image.color.b, Statics.ALPHA_ON);

        }

        public void Exit()
        {
            //throw new System.NotImplementedException();
        }

        public void Update()
        {

            if (_itemHandler.GetItem() == null)
            {
                return;
            }

            // .. periodically blink, sparkle, etc

            if (_itemHandler.IsProcessingRemoval)
            {
                _itemHandler.StateMachine.TransitionTo(_itemHandler.StateMachine.RemovingState);
            }
        }

        public ItemIdleState(ItemHandler itemHandler)
        {
            _itemHandler = itemHandler;
        }
    }
}
