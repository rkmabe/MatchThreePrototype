using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent
{

    public class ImageFader : MonoBehaviour, IImageFader
    {

        //public bool IsProcessingItemRemoval { get => _isProcessingItemRemoval; }
        //internal bool _isProcessingItemRemoval;
        //private float _secsItemRemovalProcessing = 0;

        internal static float DEFAULT_FADE_DURATION = .5f;
        private float _fadeDuration = DEFAULT_FADE_DURATION;

        public bool IsFading { get => _isFading; }
        private bool _isFading;
        private float _secsFading = 0;

        private static float ALPHA_ON = 1;
        private static float ALPHA_OFF = 0;

        public void UpdateItemRemovalAnimation(Image image)
        {
            float alphaLerp;
            if (_secsFading < _fadeDuration)
            {
                alphaLerp = Mathf.Lerp(ALPHA_ON, ALPHA_OFF, _secsFading / _fadeDuration);
                image.color = new Color(image.color.r, image.color.g, image.color.b, alphaLerp);

                _secsFading += Time.deltaTime;
            }
            else
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                image.sprite = null;

                _isFading = false;
            }
        }
    }
}