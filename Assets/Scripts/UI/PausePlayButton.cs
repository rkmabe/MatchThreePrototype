using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

namespace MatchThreePrototype.UI
{

    public class PausePlayButton : MonoBehaviour
    {

        private Button _pausePlayButton;
        private Image _pausePlayButtonImage;

        [SerializeField] private Sprite _playSprite;
        [SerializeField] private Sprite _pauseSprite;

        private bool _isPaused = false;
        public bool IsPaused { get => _isPaused; }

        private bool _isSystematicPause = false;
        public bool IsSystematicPause { get => _isSystematicPause; }

        public delegate void OnPause(bool showScreen);
        public static OnPause OnPauseDelegate;

        public delegate void OnPlay();
        public static OnPlay OnPlayDelegate;

        private void Awake()
        {
            _pausePlayButton = GetComponent<Button>();
            _pausePlayButtonImage = _pausePlayButton.GetComponent<Image>();

            PauseScreen.pauseFadeInCompleteDelegate += OnPauseFadeInComplete;
            PauseScreen.pauseFadeOutCompleteDelegate += OnPauseFadeOutComplete;

            SettingsButton.OnSettingsCloseDelegate += OnSettingsClose;
            SettingsButton.OnSettingsOpenDelegate += OnSettingsOpen;

            //CollapsibleScreen.OnOpenDelegate += OnSubscreenOpen;
            //CollapsibleScreen.OnCloseDelegate += OnSubscreenClose;
        }

        // Start is called before the first frame update
        void Start()
        {
            _pausePlayButton.onClick.AddListener(delegate { OnPausePlayButtonClick(); });
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            //CollapsibleScreen.OnOpenDelegate -= OnSubscreenOpen;
            //CollapsibleScreen.OnCloseDelegate -= OnSubscreenClose;

            PauseScreen.pauseFadeInCompleteDelegate -= OnPauseFadeInComplete;
            PauseScreen.pauseFadeOutCompleteDelegate -= OnPauseFadeOutComplete;

            SettingsButton.OnSettingsCloseDelegate -= OnSettingsClose;
            SettingsButton.OnSettingsOpenDelegate -= OnSettingsOpen;

            _pausePlayButton.onClick.RemoveAllListeners();
        }

        private void OnSettingsOpen()
        {
            if (!_isPaused)
            {
                SwitchToPause(true);
            }
        }
        private void OnSettingsClose()
        {
            if (_isPaused && _isSystematicPause)
            {
                SwitchToPlay();
            }
        }

        private void OnSubscreenOpen()
        {
            SetInteractable(false);
        }
        private void OnSubscreenClose()
        {
            SetInteractable(true);
        }

        private void OnPauseFadeOutComplete()
        {
            SetInteractable(true);
        }

        private void OnPauseFadeInComplete()
        {
            SetInteractable(true);
        }

        internal void SetInteractable(bool interactable)
        {
            _pausePlayButton.interactable = interactable;
        }

        private void OnPausePlayButtonClick()
        {

            //SetInteractable(false);

            // TODO: add vibration
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

            if (Time.timeScale > 0)
            {
                // "PLAY MODE" .. switch to PAUSE
                SwitchToPause(false);
            }
            else if (Time.timeScale == 0)
            {
                // "PAUSE MODE" .. switch to PLAY
                SwitchToPlay();
            }
        }

        internal void SwitchToPlay()
        {
            Time.timeScale = 1;
            _pausePlayButtonImage.sprite = _pauseSprite;
            OnPlayDelegate();

            _isPaused = false;
            _isSystematicPause = false;
        }

        internal void SwitchToPause(bool isSettingScreenPause)
        {
            _isSystematicPause = isSettingScreenPause;


            if (!isSettingScreenPause)
            {
                _pausePlayButtonImage.sprite = _playSprite;
            }

            //Debug.Log("set time scale 0");
            Time.timeScale = 0;


            bool showScreen;
            if (isSettingScreenPause)
            {
                showScreen = false;
            }
            else
            {
                showScreen = true;
            }
            OnPauseDelegate(showScreen);



            _isPaused = true;
        }

    }
}