using Lofelt.NiceVibrations;
using MatchThreePrototype.PlayAreaCellMatching;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using UnityEngine;

namespace MatchThreePrototype
{

    public class UIFeedbackController : MonoBehaviour
    {

        private AudioSource _audioSource;
        private ItemPool _itemPool;

        [SerializeField] private AudioClip _settingsOpenSound = null;
        [SerializeField] private AudioClip _settingsCloseSound = null;

        private void OnMatchCaught(MatchRecord match)
        {
            //PlayItemMatchSound(match.ItemType);

            AudioClip clip = _itemPool.GetItemTypeAudioClip(match.ItemType);
            if (clip != null)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }

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
            PlayAreaCellMatchDetector.OnMatchCaughtDelegate -= OnMatchCaught;

            SettingsButton.OnSettingsCloseDelegate -= OnSettingsClose;
            SettingsButton.OnSettingsOpenDelegate -= OnSettingsOpen;
        }

        private void Awake()
        {
            PlayAreaCellMatchDetector.OnMatchCaughtDelegate += OnMatchCaught;

            _itemPool = FindAnyObjectByType<ItemPool>();

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
