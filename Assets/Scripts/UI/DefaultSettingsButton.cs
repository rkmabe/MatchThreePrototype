using Lofelt.NiceVibrations;
using MatchThreePrototype.Controllers;
using UnityEngine;
using UnityEngine.UI;


namespace MatchThreePrototype.UI
{

    public class DefaultSettingsButton : MonoBehaviour
    {

        //[SerializeField] private Button _button;

        private Button _button;

        [SerializeField] SettingsController _settingsController;
        [SerializeField] SettingsScreen _settingsScreen;

        private void OnButtonClick()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

            _settingsController.SetDefaults();
            _settingsScreen.SetDefaults();

        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(delegate { OnButtonClick(); });
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
