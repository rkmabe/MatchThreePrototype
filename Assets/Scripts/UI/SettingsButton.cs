using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

namespace MatchThreePrototype.UI
{

    public class SettingsButton : MonoBehaviour
    {
        private Button _button;
        private PausePlayButton _pausePlayButton;

        [SerializeField] private Canvas _settingsCanvas;
        [SerializeField] private Animator _settingsCanvasAnimator;

        private static string IsDisplayingSettings = "IsDisplayingSettings";
        private static int IsDisplayingSettingsID = Animator.StringToHash(IsDisplayingSettings);


        public delegate void OnSettingsOpen();
        public static OnSettingsOpen OnSettingsOpenDelegate;

        public delegate void OnSettingsClose();
        public static OnSettingsClose OnSettingsCloseDelegate;

        private void ToggleSettingsMenu()
        {
            if (_settingsCanvas.gameObject.activeSelf)
            {
                OnSettingsCloseDelegate(); // play audio, handle play/pause

                _settingsCanvasAnimator.SetBool(IsDisplayingSettingsID, false);
            }
            else
            {
                OnSettingsOpenDelegate(); // play audio, handle play/pause

                _settingsCanvas.gameObject.SetActive(true);
                _settingsCanvasAnimator.SetBool(IsDisplayingSettingsID, true);
            }

        }
        private void OnSettingsButtonClick()
        {
            ToggleSettingsMenu();
        }

        private void Awake()
        {

            _button = GetComponent<Button>();
            _button.onClick.AddListener(delegate { OnSettingsButtonClick(); });            

            _pausePlayButton = FindObjectOfType<PausePlayButton>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
