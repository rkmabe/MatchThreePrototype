using Lofelt.NiceVibrations;
using MatchThreePrototype.UI;
using UnityEngine;

namespace MatchThreePrototype.Controllers
{

    public class UIFeedbackController : MonoBehaviour
    {

        private AudioSource _audioSource;

        [SerializeField] private AudioClip _settingsOpenSound = null;
        [SerializeField] private AudioClip _settingsCloseSound = null;

        private void OnSettingsOpen()
        {
            _audioSource.clip = _settingsOpenSound;
            _audioSource.Play();

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }
        private void OnSettingsClose()
        {

            _audioSource.clip = _settingsCloseSound;
            _audioSource.Play();

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }

        private void OnDestroy()
        {

            SettingsButton.OnSettingsCloseDelegate -= OnSettingsClose;
            SettingsButton.OnSettingsOpenDelegate -= OnSettingsOpen;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            SettingsButton.OnSettingsCloseDelegate += OnSettingsClose;
            SettingsButton.OnSettingsOpenDelegate += OnSettingsOpen;

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
