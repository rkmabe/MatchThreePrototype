
namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock
{

    public class BlockIdleState : IContentState
    {
        private BlockHandler _blockHandler;

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {
            if (_blockHandler.GetBlock()==null)
            {
                return;
            }

            // .. periodically blink, sparkle, etc

            if (_blockHandler.IsProcessingRemoval)
            {
                _blockHandler.StateMachine.TransitionTo(_blockHandler.StateMachine.RemovingState);
            }
        }

        public BlockIdleState(BlockHandler blockHandler)
        {
            _blockHandler = blockHandler;
        }
    }
}