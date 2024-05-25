using System.Collections.Generic;
using MatchThreePrototype.PlayAreaCellContent.PlayAreaItem;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaElements
{

    public class DrawnItemHandler : MonoBehaviour, IDrawnItemsHandler
    {
        private List<Item> _drawnItems = new List<Item>();

        private List<ItemTypes> _allowedItemTypes;
        private ItemPool _itemPool;

        public void Setup(List<ItemTypes> allowedItemTypes, ItemPool itemPool)
        {
            _allowedItemTypes = allowedItemTypes;
            _itemPool = itemPool;
        }

        public void DrawItems(int numCells)
        {
            // draw enough items to fully populate play area twice.
            // try to draw an equal amount of each type.
            // if not an even divide, draw a random type for each remainder.

            int drawCount = numCells * 2;

            int divTypesPerDrawCount = drawCount / _allowedItemTypes.Count;
            for (int i = 0; i < _allowedItemTypes.Count; i++)
            {
                for (int j = 0; j < divTypesPerDrawCount; j++)
                {
                    ItemTypes itemType = _allowedItemTypes[i];

                    Item item = _itemPool.GetNextAvailable(itemType);

                    _drawnItems.Add(item);
                }
            }

            int modTypesPerDrawCount = drawCount % _allowedItemTypes.Count;
            for (int i = 0; i < modTypesPerDrawCount; i++)
            {
                Item item = _itemPool.GetNextAvailable();
                _drawnItems.Add(item);
            }

        }

        public int GetDrawnItemsIndex(List<ItemTypes> excludedItemTypes)
        {
            if (excludedItemTypes.Count == 0)
            {
                return UnityEngine.Random.Range(0, _drawnItems.Count);
            }
            else
            {
                for (int i = 0; i < _drawnItems.Count; i++)
                {
                    bool isExcluded = false;
                    for (int j = 0; j < excludedItemTypes.Count; j++)
                    {
                        if (_drawnItems[i].ItemType == excludedItemTypes[j])
                        {
                            isExcluded = true;
                            break;
                        }
                    }
                    if (!isExcluded)
                    {
                        return i;
                    }
                }
            }

            // you are never going to get here. .. unless you have too few allowed item types!
            //Debug.LogError("oh yeah?");
            return UnityEngine.Random.Range(0, _drawnItems.Count);
        }

        public List<Item> GetDrawnItems()
        {
            return _drawnItems;
        }
        public Item GetRandomItem()
        {
            if (_drawnItems.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, _drawnItems.Count);
                Item item = _drawnItems[index];
                _drawnItems.RemoveAt(index);

                return item;
            }

            Debug.LogError("No more DRAWN items!");
            return null;
        }

        public void ShuffleItems()
        {
            for (int i = _drawnItems.Count - 1; i > 0; i--)
            {
                int k = UnityEngine.Random.Range(0, i + 1);
                Item itemToSwap = _drawnItems[k];
                _drawnItems[k] = _drawnItems[i];
                _drawnItems[i] = itemToSwap;
            }
        }

        public void OnDrawnItemReturn(Item item)
        {
            _drawnItems.Add(item);
        }

        private void OnDestroy()
        {
            ItemHandler.OnDrawnItemReturn -= OnDrawnItemReturn;
        }

        private void Awake()
        {
            ItemHandler.OnDrawnItemReturn += OnDrawnItemReturn;
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
