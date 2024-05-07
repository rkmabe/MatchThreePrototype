using MatchThreePrototype.PlayAreaCellContent.Item;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype
{

    public class HeldItemCell : MonoBehaviour   //, IItemHandler
    {
        public IPlayAreaItemHandler ItemHandler { get => _itemHandler; }
        private IPlayAreaItemHandler _itemHandler;


        private TMPro.TextMeshProUGUI _debugText;


        private void Awake()
        {
            //_image = GetComponentInChildren<Image>();
            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();

            _itemHandler = GetComponent<IPlayAreaItemHandler>();
        }

        void Start()
        {

        }

        void Update()
        {

        }


    }
}
