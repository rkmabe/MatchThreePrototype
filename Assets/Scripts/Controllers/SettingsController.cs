using MatchThreePrototype.PlayAreaElements;
using MatchThreePrototype.PlayerTouchInput;
using UnityEngine;

namespace MatchThreePrototype.Controllers
{

    public class SettingsController : MonoBehaviour
    {

        [SerializeField] private PlayerTouchInput.Player _player;
        [SerializeField] private GameObject _playAreaContainer6x6;
        [SerializeField] private GameObject _playAreaContainer9x9;


        [SerializeField] private int _editorTestNativeFrameRate = 90;

        public delegate void OnNewMoveSpeed(float speed);
        public static OnNewMoveSpeed OnNewMoveSpeedDelegate;

        public delegate void OnNewDropSpeed(float speed);
        public static OnNewDropSpeed OnNewDropSpeedDelegate;

        public delegate void OnNewRemoveDuration(float speed);
        public static OnNewRemoveDuration OnNewRemoveDurationDelegate;

        public delegate void OnChangeLimitSwapRange(bool isLimited);
        public static OnChangeLimitSwapRange OnChangeLimitSwapRangeDelegate;

        


        private const int TOGGLE_OFF = 0;
        private const int TOGGLE_ON = 1;

        //private static string NUM_BLOCKS = "NUM_BLOCKS";
        //private static string NUM_OBSTACLES = "NUM_OBSTACLES";
        private static string PCT_BLOCK = "PCT_BLOCK";
        private static string PCT_OBSTACLE = "PCT_OBSTACLE";

        private static string NUM_ITEM_TYPES = "NUM_ITEM_TYPES";

        private static string LIMIT_SWAP_RANGE = "LIMIT_SWAP_RANGE";

        private static string SHOW_DEBUG_TEXTS = "SHOW_DEBUG_TEXTS";
        private static string MOVE_SPEED = "MOVE_SPEED";
        private static string DROP_SPEED = "DROP_SPEED";
        private static string REMOVE_DURATION = "REMOVE_DURATION";

        private static string PLAY_AREA_SELECTION = "PLAY_AREA_SELECTION";
        private static int PLAY_AREA_6X6 = 0;
        private static int PLAY_AREA_9X9 = 1;

        private const float DEFAULT_PCT_BLOCK = .06f;
        private const float DEFAULT_PCT_OBSTACLE = .12f;

        private void SetTargetFrameRate()
        {
            //Application.targetFrameRate = Screen.currentResolution.refreshRate;
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#if UNITY_EDITOR
            if (_editorTestNativeFrameRate != 0)
            {
                Application.targetFrameRate = _editorTestNativeFrameRate; //120;  // 30 // 60 // 90 // 120 // 240 //300 
            }
#endif
        }


        internal void SetDefaults()
        {
            SetDebugTextOn(false);
            SetMoveSpeed(MoveItemCell.DEFAULT_MOVE_SPEED);
            SetDropSpeed(DropCell.DEFAULT_DROP_SPEED);
            SetRemoveDuration(PlayAreaCell.DEFAULT_REMOVAL_DURATION);

            SetPctBlock(DEFAULT_PCT_BLOCK);
            SetPctObstacle(DEFAULT_PCT_OBSTACLE);


            SetLimitSwapRange(false);
            SetNumItemTypes(4);
            SetPlayAreaSelection(PLAY_AREA_9X9);
        }


        private void EnableDebugTexts(bool enabled)
        {
            const string DEBUG_OBJECTS = "DEBUG_TEXT_CONTAINER";

            GameObject[] debugObjects = GameObject.FindGameObjectsWithTag(DEBUG_OBJECTS);

            foreach (var item in debugObjects)
            {
                item.GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = enabled;
            }
        }

        // REMOVAL DURATION
        internal float GetRemoveDuration()
        {
            if (!PlayerPrefs.HasKey(REMOVE_DURATION))
            {
                PlayerPrefs.SetFloat(REMOVE_DURATION, PlayAreaCell.DEFAULT_REMOVAL_DURATION);
            }
            return PlayerPrefs.GetFloat(REMOVE_DURATION);
        }
        internal void SetRemoveDuration(float duration)
        {
            PlayerPrefs.SetFloat(REMOVE_DURATION,duration);
            SignalNewRemoveDuration(duration);
        }
        private void SignalNewRemoveDuration(float duration)
        {
            OnNewRemoveDurationDelegate?.Invoke(duration);
        }


        // MOVE SPEED
        internal float GetMoveSpeed()
        {
            if (!PlayerPrefs.HasKey(MOVE_SPEED))
            {
                PlayerPrefs.SetFloat(MOVE_SPEED, MoveItemCell.DEFAULT_MOVE_SPEED);
            }
            return PlayerPrefs.GetFloat(MOVE_SPEED);
        }
        internal void SetMoveSpeed(float speed) 
        {
            PlayerPrefs.SetFloat(MOVE_SPEED, speed);
            SignalNewMoveSpeed(speed);
        }
        private void SignalNewMoveSpeed(float speed)
        {
            OnNewMoveSpeedDelegate?.Invoke(speed);
        }


        // DROP SPEED
        internal float GetDropSpeed()
        {
            if (!PlayerPrefs.HasKey(DROP_SPEED))
            {
                PlayerPrefs.SetFloat(DROP_SPEED, DropCell.DEFAULT_DROP_SPEED);
            }
            return PlayerPrefs.GetFloat(DROP_SPEED);
        }
        internal void SetDropSpeed(float speed)
        {
            PlayerPrefs.SetFloat(DROP_SPEED, speed);
            SignalNewDropSpeed(speed);
        }
        private void SignalNewDropSpeed(float speed)
        {
            OnNewDropSpeedDelegate?.Invoke(speed);
        }

        // PLAY AREA
        internal int GetPlayAreaSelection()
        {
            if (!PlayerPrefs.HasKey(PLAY_AREA_SELECTION))
            {
                PlayerPrefs.SetInt(PLAY_AREA_SELECTION, PLAY_AREA_9X9);
            }
            return PlayerPrefs.GetInt(PLAY_AREA_SELECTION);

        }
        internal void SetPlayAreaSelection(int selection)
        {
            PlayerPrefs.SetInt(PLAY_AREA_SELECTION, selection);
        }

        // NUM BLOCKS
        internal float GetPctBlock()
        {
            if (!PlayerPrefs.HasKey(PCT_BLOCK))
            {
                PlayerPrefs.SetFloat(PCT_BLOCK, DEFAULT_PCT_BLOCK);
            }
            return PlayerPrefs.GetFloat(PCT_BLOCK);
        }
        internal void SetPctBlock(float pctBlock)
        {
            PlayerPrefs.SetFloat(PCT_BLOCK, pctBlock);
        }

        // NUM OBSTACLES
        internal float GetPctObstacle()
        {
            if (!PlayerPrefs.HasKey(PCT_OBSTACLE))
            {
                PlayerPrefs.SetFloat(PCT_OBSTACLE, DEFAULT_PCT_OBSTACLE);
            }
            return PlayerPrefs.GetFloat(PCT_OBSTACLE);
        }
        internal void SetPctObstacle(float pctObstacle)
        {
            PlayerPrefs.SetFloat(PCT_OBSTACLE, pctObstacle);
        }


        // SWAP RANGE
        internal bool GetLimitSwapRange()
        {
            if (!PlayerPrefs.HasKey(LIMIT_SWAP_RANGE))
            {
                // if no key yet exists, as in first time load, set to default
                PlayerPrefs.SetInt(LIMIT_SWAP_RANGE, TOGGLE_OFF);
            }

            int isToggled = PlayerPrefs.GetInt(LIMIT_SWAP_RANGE);
            if (isToggled == TOGGLE_ON)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal void SetLimitSwapRange(bool isToggled)
        {
            if (isToggled)
            {
                PlayerPrefs.SetInt(LIMIT_SWAP_RANGE, TOGGLE_ON);
            }
            else
            {
                PlayerPrefs.SetInt(LIMIT_SWAP_RANGE, TOGGLE_OFF);
            }
            SignalChangeLimitSwapRange(isToggled);
        }
        private void SignalChangeLimitSwapRange(bool isLimited)
        {
            OnChangeLimitSwapRangeDelegate?.Invoke(isLimited);
        }

        // DEBUG TEXTS
        internal bool GetDebugTextOn()
        {
            if (!PlayerPrefs.HasKey(SHOW_DEBUG_TEXTS))
            {
                // if no key yet exists, as in first time load, set to default
                PlayerPrefs.SetInt(SHOW_DEBUG_TEXTS, TOGGLE_OFF);
            }

            int isToggled = PlayerPrefs.GetInt(SHOW_DEBUG_TEXTS);
            if (isToggled == TOGGLE_ON)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void SetDebugTextOn(bool isToggled)
        {
            if (isToggled)
            {
                PlayerPrefs.SetInt(SHOW_DEBUG_TEXTS, TOGGLE_ON);

                EnableDebugTexts(true);
            }
            else
            {
                PlayerPrefs.SetInt(SHOW_DEBUG_TEXTS, TOGGLE_OFF);

                EnableDebugTexts(false);
            }
        }

        // NUM ITEM TYPES
        internal int GetNumItemTypes()
        {
            if (!PlayerPrefs.HasKey(NUM_ITEM_TYPES))
            {
                PlayerPrefs.SetInt(NUM_ITEM_TYPES, 3);
            }
            return PlayerPrefs.GetInt(NUM_ITEM_TYPES);
        }
        internal void SetNumItemTypes(int numItemTypes)
        {
            PlayerPrefs.SetInt(NUM_ITEM_TYPES, numItemTypes);
        }

        private void PublishChangesToDefaults()
        {
            float moveSpeed = GetMoveSpeed();
            if (moveSpeed != MoveItemCell.DEFAULT_MOVE_SPEED)
            {
                SignalNewMoveSpeed(moveSpeed);
            }

            float dropSpeed = GetDropSpeed();
            if (dropSpeed != DropCell.DEFAULT_DROP_SPEED)
            {
                SignalNewDropSpeed(dropSpeed);
            }

            float removalDuration = GetRemoveDuration();
            if (removalDuration != PlayAreaCell.DEFAULT_REMOVAL_DURATION)
            {
                SignalNewRemoveDuration(removalDuration);
            }

            bool isSwapRangeLimited = GetLimitSwapRange();
            if (isSwapRangeLimited != TouchInfoProvider.DEFAULT_SWAP_RANGE_LIMITED)
            {
                SignalChangeLimitSwapRange(isSwapRangeLimited);
            }
        }


        private void Awake()
        {
            SetTargetFrameRate();

            int selection = GetPlayAreaSelection();
            if (selection == PLAY_AREA_6X6)
            {
                _playAreaContainer6x6.SetActive(true);
                PlayArea playArea6x6 = _playAreaContainer6x6.GetComponentInChildren<PlayArea>();
                _player.PlayArea = playArea6x6;
            }
            else
            {
                _playAreaContainer9x9.SetActive(true);
                PlayArea playArea9x9 = _playAreaContainer9x9.GetComponentInChildren<PlayArea>();
                _player.PlayArea = playArea9x9;
            }

            if (GetDebugTextOn())
            {
                EnableDebugTexts(true);
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            PublishChangesToDefaults();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
