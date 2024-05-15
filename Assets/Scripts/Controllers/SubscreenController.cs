using UnityEngine;
using MatchThreePrototype.UI;

namespace MatchThreePrototype.Controllers
{

    public class SubscreenController : MonoBehaviour
    {
        [SerializeField] private Canvas _settingsCanvas;

        private void OnSettingsScreenCloseComplete()
        {
            _settingsCanvas.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            SettingsScreen.settingsScreenCloseCompleteDelegate -= OnSettingsScreenCloseComplete;
        }

        private void Awake()
        {
            SettingsScreen.settingsScreenCloseCompleteDelegate += OnSettingsScreenCloseComplete;
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
