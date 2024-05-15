namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{

    public class ItemIdleState : IContentState
    {
        private ItemHandler _itemHandler;

        public void Enter()
        {

        }

        public void Exit()
        {
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
