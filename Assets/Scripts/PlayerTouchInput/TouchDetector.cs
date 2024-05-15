using UnityEngine;
using UnityEngine.EventSystems;
using MatchThreePrototype.UI;

namespace MatchThreePrototype.PlayerTouchInput
{

    public class TouchDetector : MonoBehaviour
    {

        private bool _isCollectingInput = true;  // set to FALSE when game is paused (see PlayPauseButton)

        public delegate void OnTouchInputDown(Vector3 position);
        public static OnTouchInputDown OnTouchInputDownDelegate;

        public delegate void OnTouchInputUp(Vector3 position);
        public static OnTouchInputUp OnTouchInputUpDelegate;

        public delegate void OnTouchInputDrag(UnityEngine.Touch dragTouch);
        public static OnTouchInputDrag OnTouchInputDragDelegate;

        private void Awake()
        {
            PausePlayButton.OnPauseDelegate += OnPause;
            PausePlayButton.OnPlayDelegate += OnPlay;
        }

        private void OnPause(bool showScreen)
        {
            _isCollectingInput = false;
        }
        private void OnPlay()
        {
            _isCollectingInput = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            _isCollectingInput = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isCollectingInput)
            {
                return;
            }

            if (Input.touches.Length == 0)
            {
                return;
            }

            // touching an event system object? e.g. UI button
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                return;
            }

            UnityEngine.Touch touch = Input.touches[0];  // get the first touch

            if (touch.phase == TouchPhase.Began)
            {
                OnTouchInputDownDelegate(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                OnTouchInputDragDelegate(touch);
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                OnTouchInputUpDelegate(touch.position);
            }

        }
    }

}
