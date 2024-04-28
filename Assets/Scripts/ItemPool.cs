using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype
{
    public class ItemPool : MonoBehaviour
    {
        [Header("Must be at least max PlayAreaSize * 2 / Num Item Types")]
        [SerializeField] private int _maxPerType = 30;    // MUST be at least (<MaxPlayAreaSize=81> * 2 / 6)!  27 is min for max play area size 81

        public bool IsInitialized { get => _isInitialized; }


        private bool _isInitialized = false;

        private List<Item> _availableItems = new List<Item>();

        [SerializeField] private List<ItemTypeConfig> _itemTypeConfig = new List<ItemTypeConfig>();

        internal AudioClip GetItemTypeAudioClip(ItemTypes itemType)
        {
            for (int i = 0; i < _itemTypeConfig.Count; i++)
            {
                if (_itemTypeConfig[i].Type == itemType)
                {
                    if (_itemTypeConfig[i].MatchAudioClip.Count == 1)
                    {
                        return _itemTypeConfig[i].MatchAudioClip[0];
                    }
                    else if (_itemTypeConfig[i].MatchAudioClip.Count > 1)
                    {
                        int x = UnityEngine.Random.Range(0, _itemTypeConfig[i].MatchAudioClip.Count);
                        return _itemTypeConfig[i].MatchAudioClip[x];
                    }
                }
            }

            Debug.LogWarning("No Audio Clip found for " + itemType);
            return null;
        }

        private void InitializePool()
        {
            for (int i = 0; i < _itemTypeConfig.Count; i++)
            {
                for (int j = 0; j < _maxPerType; j++)
                {
                    PutInPool(Instantiate(_itemTypeConfig[i].Prefab) as Item);
                }
            }

            _isInitialized = true;
        }



        private void PutInPool(Item item)
        {
            //item.transform.parent = transform;
            item.transform.SetParent(transform);
            item.transform.localPosition = Statics.Vector3Zero();
            item.gameObject.SetActive(false);

            _availableItems.Add(item);
        }

        // get next available with given type
        internal Item GetNextAvailable(ItemTypes type)
        {
            if (_availableItems.Count > 0)
            {
                for (int i = 0; i < _availableItems.Count; i++)
                {
                    if (_availableItems[i].ItemType == type)
                    {
                        Item returnItem = _availableItems[i];
                        _availableItems.RemoveAt(i);
                        return returnItem;
                    }
                }
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("No more " + type.ToString() + " ITEMS in pool");
            return null;

        }
        // get next available without regard to type
        internal Item GetNextAvailable()
        {
            if (_availableItems.Count > 0)
            {
                Item returnItem = _availableItems[0];
                _availableItems.RemoveAt(0);
                return returnItem;
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("NO more ITEMS in pool");
            return null;
        }


        internal void Return(Item item)
        {
            PutInPool(item);
        }

        private void Awake()
        {
            InitializePool();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        [System.Serializable]
        public struct ItemTypeConfig
        {
            public ItemTypes Type;
            public Item Prefab;
            public List<AudioClip> MatchAudioClip;
        }
    }
    public enum ItemTypes
    {
        None = 0,
        WhitePin = 1,
        GreenPin = 2,
        BluePin = 3,
        RedPin = 4,
        PurplePin = 5,
        BlackBall = 6,
        PinkPin = 7,
    }
}
