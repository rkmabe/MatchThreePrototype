using System;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{

    public class ItemStateMachine 
    {
        public IContentState CurrentState { get; private set; }

        public event Action<IContentState> ItemStateChanged;

        public ItemIdleState IdleState;
        public ItemRemovingState RemovingState;

        public ItemSizeFluctuateState SizeFluctuateState;
        public ItemRotationFluctuateState RotationFluctuateState;

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
            ItemStateChanged?.Invoke(state);
        }

        // exit this state and enter another
        public void TransitionTo(IContentState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();

            // notify other objects that state has changed
            ItemStateChanged?.Invoke(nextState);
        }

        public void CleanUpOnDestroy()
        {
            //IdleState.CleanUp..
            RemovingState.CleanUpOnDestroy();
        }

        public ItemStateMachine(ItemHandler itemHandler) 
        {
            IdleState = new ItemIdleState(itemHandler);
            RemovingState = new ItemRemovingState(itemHandler);
            SizeFluctuateState = new ItemSizeFluctuateState(itemHandler);
            RotationFluctuateState = new ItemRotationFluctuateState(itemHandler);
        }
    }
}
