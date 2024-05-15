using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class HeldItemCell : MonoBehaviour 
    {
        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;


        private TMPro.TextMeshProUGUI _debugText;


        private void Awake()
        {
            //_image = GetComponentInChildren<Image>();
            _debugText = GetComponentInChildren<TMPro.TextMeshProUGUI>();

            _itemHandler = GetComponent<IItemHandler>();
        }

        void Start()
        {

        }

        void Update()
        {

        }


    }
}
