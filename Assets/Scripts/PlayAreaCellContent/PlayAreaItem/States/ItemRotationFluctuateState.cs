using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem.States
{


    public class ItemRotationFluctuateState : IContentState
    {

        private ItemHandler _itemHandler;

        private static Vector3 ROTATION_DEF_VEC = new Vector3(0, 0, 0);

        private static float ROTATION_DEF = 1;
        private static float ROTATION_MIN = -2.5f;
        private static float ROTATION_MAX = 2.5f;

        private static float MAX_STATE_DURATION = .5f;

        private static int NUM_CYCLES = 2;

        private float _secsInState;

        private float _cycleDuration;
        private float _secsInCycle;

        private float _durationPart1;
        private float _durationPart2;
        private float _durationPart3;

        private float _secsInPart1;
        private float _secsInPart2;
        private float _secsInPart3;

        private float _secsStartPart2;
        private float _secsStartPart3;


        private Transform _itemImageTransform;

        private void ResetCycle()
        {
            _secsInCycle = 0;
            _secsInPart1 = 0;
            _secsInPart2 = 0;
            _secsInPart3 = 0;
        }

        public void Enter()
        {
            _itemImageTransform = _itemHandler.GetImage().transform;

            _itemImageTransform.rotation = Quaternion.Euler(ROTATION_DEF_VEC);

            _secsInState = 0;

            ResetCycle();

            _cycleDuration = MAX_STATE_DURATION / NUM_CYCLES;

            _durationPart1 = _cycleDuration / 4;   // part 1 - move from Default  to MAX
            _durationPart2 = _cycleDuration / 2;   // part 2 - move from MAX to MIN
            _durationPart3 = _cycleDuration / 4;  // part 3 - move from MIN to Default

            _secsStartPart2 = _durationPart1;
            _secsStartPart3 = _durationPart1 + _durationPart2;
        }

        public void Exit()
        {
            //_itemHandler.GetImage().transform.rotation = Quaternion.Euler(SCALE_DEF_ROT);
            _itemImageTransform.rotation = Quaternion.Euler(ROTATION_DEF_VEC);
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

            _secsInState += Time.deltaTime;

            if (_secsInState < MAX_STATE_DURATION)
            {
                _secsInCycle += Time.deltaTime;

                if (_secsInCycle < _secsStartPart2)
                {
                    _secsInPart1 += Time.deltaTime;

                    // part 1 - move from DEF to UP
                    //float scaleLerp = Mathf.Lerp(SCALE_DEF, SCALE_UP_MAX, _secsInPart1 / _durationPart1);
                    //_itemImageTransform.localScale = new Vector3(scaleLerp, scaleLerp, 1);

                    // part 1 - move from DEF to MAX
                    float rotLerp = Mathf.Lerp(ROTATION_DEF, ROTATION_MAX, _secsInPart1 / _durationPart1);
                    _itemImageTransform.rotation = Quaternion.Euler(new Vector3(0,0,rotLerp));

                }
                else if (_secsInCycle < _secsStartPart3)
                {
                    _secsInPart2 += Time.deltaTime;

                    // part 2 - move from UP to DOWN
                    //float scaleLerp = Mathf.Lerp(SCALE_UP_MAX, SCALE_DOWN_MIN, _secsInPart2 / _durationPart2);
                    //_itemImageTransform.localScale = new Vector3(scaleLerp, scaleLerp, 1);

                    // part 2 - from MAX to MIN
                    float rotLerp = Mathf.Lerp(ROTATION_MAX, ROTATION_MIN, _secsInPart2 / _durationPart2);
                    _itemImageTransform.rotation = Quaternion.Euler(new Vector3(0, 0, rotLerp));
                }
                else if (_secsInCycle < _cycleDuration)
                {
                    _secsInPart3 += Time.deltaTime;

                    // part 3 - move from DOWN to DEF
                    //float scaleLerp = Mathf.Lerp(SCALE_DOWN_MIN, SCALE_DEF, _secsInPart3 / _durationPart3);
                    //_itemImageTransform.localScale = new Vector3(scaleLerp, scaleLerp, 1);

                    // part 3 - move from DOWN to DEF
                    float rotLerp = Mathf.Lerp(ROTATION_MIN, ROTATION_DEF, _secsInPart3 / _durationPart3);
                    _itemImageTransform.rotation = Quaternion.Euler(new Vector3(0, 0, rotLerp));
                }
                else
                {
                    ResetCycle();
                }
            }
            else
            {
                _itemHandler.StateMachine.TransitionTo(_itemHandler.StateMachine.IdleState);
            }
        }

        public ItemRotationFluctuateState(ItemHandler itemHandler)
        {
            _itemHandler = itemHandler;
        }

    }
}