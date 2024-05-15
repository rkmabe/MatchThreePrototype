using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MatchThreePrototype.UI
{

    public class ResetButton : MonoBehaviour
    {

        [SerializeField] private Button _button;



        private void OnButtonClick()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

            //Debug.Log("SHOW HELP!");
            //_helpController.ShowScreen();

            //ResetScene()

            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }

            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }


        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void Awake()
        {
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