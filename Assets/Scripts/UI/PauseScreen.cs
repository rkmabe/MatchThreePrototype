using UnityEngine;

namespace MatchThreePrototype.UI
{

    public class PauseScreen : MonoBehaviour
    {

        public delegate void OnPauseFadeOutComplete();
        public static OnPauseFadeOutComplete pauseFadeOutCompleteDelegate;

        public delegate void OnPauseFadeInComplete();
        public static OnPauseFadeInComplete pauseFadeInCompleteDelegate;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void PauseFadeInFinish()
        {
            pauseFadeInCompleteDelegate();
        }

        private void PauseFadeOutFinish()
        {
            pauseFadeOutCompleteDelegate();
        }

    }
}