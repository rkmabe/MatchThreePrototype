using Lofelt.NiceVibrations;
using MatchThreePrototype.MatchDetection;
using MatchThreePrototype.MatchReaction.MatchTypes;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using UnityEngine;

namespace MatchThreePrototype.MatchReaction
{


    public class MatchFeedback : MonoBehaviour
    {
        private AudioSource _audioSource;
        private ItemPool _itemPool;

        private void OnMatchCaught(Match match)
        {
            AudioClip clip = _itemPool.GetItemTypeAudioClip(match.ItemType);
            if (clip != null)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }

        private void OnDestroy()
        {
            PlayAreaCellMatchDetector.OnMatchCaughtDelegate -= OnMatchCaught;
        }

        private void Awake()
        {
            PlayAreaCellMatchDetector.OnMatchCaughtDelegate += OnMatchCaught;

            _itemPool = FindAnyObjectByType<ItemPool>();

            _audioSource = GetComponent<AudioSource>();
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
