using TMPro;
using UnityEngine;

namespace MatchThreePrototype.UI
{

    public class WaveTimer : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _waveTimerText;

        private bool _isRunning = false;

        private float _currentTime;

        private static string INITIAL_VALUE = "00:00";

        private void Awake()
        {
            PausePlayButton.OnPauseDelegate += OnPause;
            PausePlayButton.OnPlayDelegate += OnPlay;
        }

        // Start is called before the first frame update
        void Start()
        {
            StartTimer();
        }

        // Update is called once per frame
        void Update()
        {
            if (_isRunning)
            {
                _currentTime += Time.deltaTime;

                UpdateTimerWithCurrentTime();
            }
        }

        private void OnDestroy()
        {
            PausePlayButton.OnPauseDelegate -= OnPause;
            PausePlayButton.OnPlayDelegate -= OnPlay;
        }

        private void UpdateTimerWithCurrentTime()
        {
            string formattedCurrentTime;

            if (_currentTime < 1)
            {
                formattedCurrentTime = INITIAL_VALUE;
            }
            else
            {
                formattedCurrentTime = System.TimeSpan.FromSeconds(_currentTime).ToString(@"mm\:ss");
            }

            _waveTimerText.text = formattedCurrentTime;
        }

        internal void StartTimer()
        {
            _isRunning = true;
        }
        internal void StopTimer()
        {
            _isRunning = false;
        }
        internal void ResetTimer()
        {
            _currentTime = 0;
            //_mainController.UI.UpdateWaveTimerText(INITIAL_VALUE);
            UpdateTimerWithCurrentTime();
        }


        private void OnPlay()
        {
            // only start the timer if a wave is actually active (possible that game is paused when wave timer should not be running eg wave end or game over screen active)
            //if (_mainController.WaveController.IsWaveActive)

            // TODO: shoud the timer always start OnPlay?
            if (true)
            {
                StartTimer();
            }
      
        }
        private void OnPause(bool showScreen)
        {            
                StopTimer();            
        }

    }
}