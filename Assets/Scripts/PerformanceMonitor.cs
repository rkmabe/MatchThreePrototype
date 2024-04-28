using System.Text;
using UnityEngine;

namespace MatchThreePrototype
{

    public class PerformanceMonitor : MonoBehaviour
    {
        // reference https://catlikecoding.com/unity/tutorials/frames-per-second/
        public int frameRange = 60;

        private int _AverageFPS;
        private int _HighestFPS;
        private int _LowestFPS;

        private int[] _fpsBuffer;
        private int _fpsBufferIndex;


        static string NewLine = "\n";

        private StringBuilder _FPSTextREturnMessageSB = new StringBuilder(12);  // to avoid GC allocation that would happen if we returned new strings each time

        private TMPro.TextMeshProUGUI _fpsText;

        private void Awake()
        {
            //_UI = FindObjectOfType<UIController>();
            _fpsText = GetComponent<TMPro.TextMeshProUGUI>();
        }

        // Start is called before the first frame update
        void Start()
        {
            InitializeBuffer();
        }

        // Update is called once per frame
        void Update()
        {

            UpdateBuffer();
            CalculateFPS();

            _fpsText.text = GetFPSText();

        }

        void CalculateFPS()
        {
            int sum = 0;
            int highest = 0;
            int lowest = int.MaxValue;
            for (int i = 0; i < frameRange; i++)
            {
                int fps = _fpsBuffer[i];
                if (fps > highest)
                {
                    highest = fps;
                }
                if (fps < lowest)
                {
                    lowest = fps;
                }
                sum += fps;
            }
            _AverageFPS = sum / frameRange;
            _HighestFPS = highest;
            _LowestFPS = lowest;
        }

        void UpdateBuffer()
        {
            // we can just wrap the index back to the start of the array. That way we always override the oldest value with the newest, once the buffer has been filled.
            _fpsBuffer[_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
            if (_fpsBufferIndex >= frameRange)
            {
                _fpsBufferIndex = 0;
            }
        }

        void InitializeBuffer()
        {
            if (frameRange <= 0)
            {
                frameRange = 1;
            }
            _fpsBuffer = new int[frameRange];
            _fpsBufferIndex = 0;
        }


        internal string GetFPSText()
        {

            _FPSTextREturnMessageSB.Clear();
            _FPSTextREturnMessageSB.Append(_LowestFPS);
            _FPSTextREturnMessageSB.Append(NewLine);
            _FPSTextREturnMessageSB.Append(_AverageFPS);
            _FPSTextREturnMessageSB.Append(NewLine);
            _FPSTextREturnMessageSB.Append(_HighestFPS);

            return _FPSTextREturnMessageSB.ToString();

        }

    }

}
