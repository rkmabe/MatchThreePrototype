using Lofelt.NiceVibrations;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class SettingsScreen : MonoBehaviour
    {

        [SerializeField] private ScrollRect _contentScrollRect;

        [Header("Settings Fields - APPLE")]
        [SerializeField] private Toggle _debugTextToggleA;
        [SerializeField] private TMPro.TextMeshProUGUI _moveSpeedTextA;

        [SerializeField] private Slider _moveSpeedTextSliderA;

        [SerializeField] private TMPro.TextMeshProUGUI _dropSpeedTextA;

        [SerializeField] private Slider _dropSpeedTextSliderA;

        [SerializeField] private TMPro.TextMeshProUGUI _removeDurationTextA;

        [SerializeField] private Slider _removeDurationSliderA;

        [SerializeField] private Slider _numBlocksSliderA;
        [SerializeField] private Slider _numObstaclesSliderA;

        [SerializeField] private Toggle _limitSwapRangeToggleA;
        [SerializeField] private TMPro.TMP_Dropdown _numItemTypesDropdownA;

        [SerializeField] private TMPro.TMP_Dropdown _playAreaDropdownA;

        [SerializeField] private TMPro.TextMeshProUGUI _versionTextA;
        [SerializeField] private Button _privacyPolicyLinkButtonA;
        [SerializeField] private Button _supportLinkButtonA;
        [SerializeField] private RectTransform _contentA;

        [Header("Settings Fields - GOOGLE")]
        [SerializeField] private Toggle _debugTextToggleG;
        [SerializeField] private TMPro.TextMeshProUGUI _moveSpeedTextG;

        [SerializeField] private Slider _moveSpeedTextSliderG;

        [SerializeField] private TMPro.TextMeshProUGUI _dropSpeedTextG;

        [SerializeField] private Slider _dropSpeedTextSliderG;

        [SerializeField] private TMPro.TextMeshProUGUI _removeDurationTextG;

        [SerializeField] private Slider _removeDurationSliderG;

        [SerializeField] private Slider _numBlocksSliderG;
        [SerializeField] private Slider _numObstaclesSliderG;

        [SerializeField] private Toggle _limitSwapRangeToggleG;
        [SerializeField] private TMPro.TMP_Dropdown _numItemTypesDropdownG;

        [SerializeField] private TMPro.TMP_Dropdown _playAreaDropdownG;

        [SerializeField] private TMPro.TextMeshProUGUI _versionTextG;
        [SerializeField] private Button _privacyPolicyLinkButtonG;
        [SerializeField] private Button _supportLinkButtonG;
        [SerializeField] private RectTransform _contentG;

        // ONLY these variables should be used by the class (other than the assignment method)
        private Toggle _debugTextToggle;
        private TMPro.TextMeshProUGUI _moveSpeedText;

        private Slider _moveSpeedSlider;

        private TMPro.TextMeshProUGUI _dropSpeedText;

        private Slider _dropSpeedSlider;

        private TMPro.TextMeshProUGUI _removeDurationText;

        private Slider _removeDurationSlider;

        private Slider _numBlocksSlider;
        private Slider _numObstaclesSlider;

        private Toggle _limitSwapRangeToggle;
        private TMPro.TMP_Dropdown _numItemTypesDropdown;

        private TMPro.TMP_Dropdown _playAreaDropdown;

        private TMPro.TextMeshProUGUI _versionText;
        private Button _privacyPolicyLinkButton;
        private Button _supportLinkButton;

        public delegate void OnSettingsScreenCloseComplete();
        public static OnSettingsScreenCloseComplete settingsScreenCloseCompleteDelegate;


        private SettingsController _settingsController;

        private void SettingsScreenCloseComplete()
        {
            settingsScreenCloseCompleteDelegate();
        }

        private void AssignScreenFieldsForPlatform()
        {
#if (UNITY_IOS)
            _contentA.gameObject.SetActive(true);
            _contentG.gameObject.SetActive(false);

            _contentScrollRect.content = _contentA;
            _debugTextToggle = _debugTextToggleA;
            _moveSpeedText = _moveSpeedTextA;

            _moveSpeedSlider = _moveSpeedTextSliderA;

            _dropSpeedText = _dropSpeedTextA;

            _dropSpeedSlider = _dropSpeedTextSliderA;

            _removeDurationText = _removeDurationTextA;

            _removeDurationSlider = _removeDurationSliderA;

            _numBlocksSlider = _numBlocksSliderA;
            _numObstaclesSlider = _numObstaclesSliderA;

            _limitSwapRangeToggle= _limitSwapRangeToggleA;
            _numItemTypesDropdown= _numItemTypesDropdownA;

            _playAreaDropdown = _playAreaDropdownA;

            _versionText = _versionTextA;
            _privacyPolicyLinkButton = _privacyPolicyLinkButtonA;
            _supportLinkButton = _supportLinkButtonA;
#elif (UNITY_ANDROID)
            _contentA.gameObject.SetActive(false);
            _contentG.gameObject.SetActive(true);

            _contentScrollRect.content = _contentG;
            _debugText = _debugTextToggleG;
            _moveSpeedText = _moveSpeedTextG;
            _moveSpeedSlider = _moveSpeedTextSliderG;
            _dropSpeedText = _dropSpeedTextG;
            _dropSpeedSlider = _dropSpeedTextSliderG;
            _removeDurationText = _removeDurationTextG;
            _removeDurationSlider = _removeDurationSliderG;
            _numBlocksSlider = _numBlocksSliderG;
            _numObstaclesSlider = _numObstaclesSliderG;
            _limitSwapRangeToggle = _limitSwapRangeToggleG;
            _numItemTypesDropdown = _numItemTypesDropdownG;
            _playAreaDropdown = _playAreaDropdownG;
            _versionText= _versionTextG;
            _privacyPolicyLinkButton= _privacyPolicyLinkButtonG;
            _supportLinkButton= _supportLinkButtonG;
#endif
        }

        private static string VERSION = "Version: ";
        private StringBuilder _versionStringBuilder = new StringBuilder();
        private void SetVersionText()
        {
            _versionStringBuilder.Append(VERSION);
            _versionStringBuilder.Append(Application.version);
            _versionText.text = _versionStringBuilder.ToString();
        }

        private void OnDebugTextToggleValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetDebugTextOn(_debugTextToggle.isOn);
        }

        private void OnMoveSpeedSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetMoveSpeed(_moveSpeedSlider.value);
        }

        private void OnDropSpeedSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetDropSpeed(_dropSpeedSlider.value);
        }

        private void OnRemoveDurationSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            float removeDuration = Statics.Interpolate(_removeDurationSlider.value, 0, 1, .05f,2.5f);

            _settingsController.SetRemoveDuration(removeDuration);
        }

        private void OnNumBlocksSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetPctBlock(_numBlocksSlider.value);
        }

        private void OnNumObstaclesSliderValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetPctObstacle(_numObstaclesSlider.value);
        }

        private void OnLimitSwapRangeToggleValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetLimitSwapRange(_limitSwapRangeToggle.isOn);
        }

        private void OnNumItemTypesDropdownValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetNumItemTypes(_numItemTypesDropdown.value);

        }

        private void OnPlayAreaDropdownValueChanged()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);

            _settingsController.SetPlayAreaSelection(_playAreaDropdown.value);
        }

        private void SetDefaultNumBlocks()
        {
            _numBlocksSlider.value = _settingsController.GetPctBlock();
        }

        private void SetDefaultNumObstacles()
        {
            _numObstaclesSlider.value = _settingsController.GetPctObstacle();
        }

        private void SetDefaultLimitSwapRange()
        {
            _limitSwapRangeToggle.isOn = _settingsController.GetLimitSwapRange();
        }
        private void SetDefaultNumItemTypes()
        {
            _numItemTypesDropdown.value = _settingsController.GetNumItemTypes();
        }

        private void SetDefaultShowCellIDs()
        {
            _debugTextToggle.isOn = _settingsController.GetDebugTextOn();

        }
        private void SetDefaultMoveSpeed()
        {
            _moveSpeedSlider.value = _settingsController.GetMoveSpeed();
        }
        private void SetDefaultDropSpeed()
        {
            _dropSpeedSlider.value = _settingsController.GetDropSpeed();
        }
        private void SetDefaultRemoveSpeed()
        {
            float sliderValue = Statics.Interpolate(_settingsController.GetRemoveDuration(), .05f, 2.5f, 0, 1);
            _removeDurationSlider.value = sliderValue;
        }

        private void SetDefaultPlayArea()
        {
            _playAreaDropdown.value = _settingsController.GetPlayAreaSelection();
        }

        internal void SetDefaults()
        {
            SetDefaultNumBlocks();
            SetDefaultNumObstacles();
            SetDefaultLimitSwapRange();
            SetDefaultNumItemTypes();
            SetDefaultShowCellIDs();
            SetDefaultMoveSpeed();
            SetDefaultDropSpeed();
            SetDefaultRemoveSpeed();
            SetDefaultPlayArea();
        }

        private void Awake()
        {
            _settingsController = FindObjectOfType<SettingsController>();

            AssignScreenFieldsForPlatform();  // must come before .AddListeners for screen fields!

            SetVersionText();

            _debugTextToggle.onValueChanged.AddListener(delegate { OnDebugTextToggleValueChanged(); });

            _moveSpeedSlider.onValueChanged.AddListener(delegate { OnMoveSpeedSliderValueChanged(); });

            _dropSpeedSlider.onValueChanged.AddListener(delegate { OnDropSpeedSliderValueChanged(); });

            _removeDurationSlider.onValueChanged.AddListener(delegate { OnRemoveDurationSliderValueChanged(); });

            _numBlocksSlider.onValueChanged.AddListener(delegate { OnNumBlocksSliderValueChanged(); });

            _numObstaclesSlider.onValueChanged.AddListener(delegate { OnNumObstaclesSliderValueChanged(); });

            _limitSwapRangeToggle.onValueChanged.AddListener(delegate { OnLimitSwapRangeToggleValueChanged(); });

            _numItemTypesDropdown.onValueChanged.AddListener(delegate { OnNumItemTypesDropdownValueChanged(); });

            _playAreaDropdown.onValueChanged.AddListener(delegate { OnPlayAreaDropdownValueChanged(); });

            SetDefaults();
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
