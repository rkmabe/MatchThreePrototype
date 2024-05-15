using System;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaObstacle
{

    public class ObstacleStateMachine
    {
        public IContentState CurrentState { get; private set; }

        public event Action<IContentState> ObstacleStateChanged;

        public ObstacleIdleState IdleState;
        public ObstacleRemovingState RemovingState;

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
            ObstacleStateChanged?.Invoke(state);
        }

        // exit this state and enter another
        public void TransitionTo(IContentState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();

            // notify other objects that state has changed
            ObstacleStateChanged?.Invoke(nextState);
        }

        public void CleanUpOnDestroy()
        {
            //IdleState.CleanUp..
            RemovingState.CleanUpOnDestroy();
        }

        public ObstacleStateMachine(ObstacleHandler obstacleHandler)
        {
            IdleState = new ObstacleIdleState(obstacleHandler);
            RemovingState = new ObstacleRemovingState(obstacleHandler);
        }
    }
}
