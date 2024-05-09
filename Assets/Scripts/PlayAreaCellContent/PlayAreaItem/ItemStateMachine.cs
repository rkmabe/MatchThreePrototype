using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.CullingGroup;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{

    public class ItemStateMachine 
    {
        public IContentState CurrentState { get; private set; }

        public event Action<IContentState> ItemStateChanged;

        public ItemIdleState IdleState;
        public ItemRemovingState RemovingState;

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
        }
    }
}
