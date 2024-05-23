using System;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock
{

    public class BlockStateMachine
    {
        public IContentState CurrentState { get; private set; }

        public event Action<IContentState> BlockStateChanged;

        public BlockIdleState IdleState;
        public BlockRemovingState RemovingState;

        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }

        // set the starting state
        public void Initialize(IContentState state)
        {
            CurrentState = state;
            state.Enter();

            // notify other objects that state has changed
            BlockStateChanged?.Invoke(state);
        }

        // exit this state and enter another
        public void TransitionTo(IContentState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();

            // notify other objects that state has changed
            BlockStateChanged?.Invoke(nextState);
        }

        public void CleanUpOnDestroy()
        {
            //IdleState.CleanUp..
            RemovingState.CleanUpOnDestroy();
        }

        public BlockStateMachine(BlockHandler blockHandler)
        {
            IdleState = new BlockIdleState(blockHandler);
            RemovingState = new BlockRemovingState(blockHandler);
        }
    }
}
