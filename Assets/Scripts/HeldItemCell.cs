using MatchThreePrototype.PlayAreaManagment;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class HeldItemCell : MonoBehaviour   //, IItemHandler
    {
        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;


        private TMPro.TextMeshProUGUI _debugText;


        private void Awake()
        {
            //_image = GetComponentInChildren<Image>();
            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();

            _itemHandler = GetComponent<ItemHandler>();
        }

        void Start()
        {

        }

        void Update()
        {

        }


    }
}
