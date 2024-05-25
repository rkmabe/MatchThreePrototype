using MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States
{

    public class ItemIdleState : IContentState
    {
        private ItemHandler _itemHandler;

        private static float MIN_SECS_IDLE = 2;
        private static float MAX_SECS_IDLE = 20;

        private float _secsIdle = 0;

        private float _randomDuration = MIN_SECS_IDLE;


        public void Enter()
        {
            _secsIdle = 0;

            _randomDuration = UnityEngine.Random.Range(MIN_SECS_IDLE, MAX_SECS_IDLE);

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

            if (_itemHandler.IsProcessingRemoval)
            {
                _itemHandler.StateMachine.TransitionTo(_itemHandler.StateMachine.RemovingState);
                return;
            }

            if (_secsIdle < _randomDuration)
            {
                _secsIdle += Time.deltaTime;
            }
            else
            {
                if (UnityEngine.Random.value > .5f)
                {
                    _itemHandler.StateMachine.TransitionTo(_itemHandler.StateMachine.SizeFluctuateState);
                }
                else
                {
                    _itemHandler.StateMachine.TransitionTo(_itemHandler.StateMachine.RotationFluctuateState);
                }
            }

        }


        public ItemIdleState(ItemHandler itemHandler)
        {
            _itemHandler = itemHandler;
        }
    }
}
