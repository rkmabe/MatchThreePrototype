using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class HeldItemCell : MonoBehaviour 
    {
        public IItemHandler ItemHandler { get => _itemHandler; }
        private IItemHandler _itemHandler;

        private void Awake()
        {
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
