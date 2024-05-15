using UnityEngine;

namespace MatchThreePrototype.UI
{
    public class HoriztonalAnchorAdjuster : MonoBehaviour
    {
        [SerializeField] private  float _xAnchorMinTablet = .15f;
        [SerializeField] private float _xAnchorMaxTablet = .85f;

        [SerializeField] private float _xAnchorMinOther = .005f;
        [SerializeField] private float _xanchorMaxOther = .995f;

        private Vector2 _anchorMinTablet;
        private Vector2 _anchorMaxTablet;

        private Vector2 _anchorMinOther;
        private Vector2 _anchorMaxOther;

        public static float TabletAspectRatioThreshold = 1.75f;      // above this a phone, under a tablet            

        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        // Start is called before the first frame update
        void Start()
        {
            SetupVectors();
            SetHoriztontalAnchors();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void SetupVectors()
        {
            _anchorMinTablet = new Vector2(_xAnchorMinTablet, _rect.anchorMin.y);
            _anchorMaxTablet = new Vector2(_xAnchorMaxTablet, _rect.anchorMax.y);

            _anchorMinOther = new Vector2(_xAnchorMinOther, _rect.anchorMin.y);
            _anchorMaxOther = new Vector2(_xanchorMaxOther, _rect.anchorMax.y);
        }

        // rect transform should spread across the entire screen on tall, thin devices (greater than 4:3)
        // It should spread across about three quarters of the screen on wider devices (4:3 or greater, only ipads? )
        private void SetHoriztontalAnchors()
        {
            float screenAspectRatio = (float)Screen.height / (float)Screen.width;
            
            if (screenAspectRatio < TabletAspectRatioThreshold)
            {
                // tablet
                _rect.anchorMax = _anchorMaxTablet;
                _rect.anchorMin = _anchorMinTablet;
            }
            else
            {
                // phone
                _rect.anchorMax = _anchorMaxOther;
                _rect.anchorMin = _anchorMinOther;
            }
        }

    }
}