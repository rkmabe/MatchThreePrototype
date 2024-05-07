using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.Block
{

    public class BlockPool : MonoBehaviour
    {

        [Header("Must be at least max PlayAreaSize")]
        [SerializeField] private int _maxPerType = 81;

        [SerializeField] private List<BlockPrefabConfig> _blockPrefabConfig = new List<BlockPrefabConfig>();


        private List<PlayAreaBlock> _availableBlocks = new List<PlayAreaBlock>();

        public bool IsInitialized { get => _isInitialized; }
        private bool _isInitialized = false;

        internal PlayAreaBlock GetNextAvailable(BlockTypes type)
        {
            if (_availableBlocks.Count > 0)
            {
                for (int i = 0; i < _availableBlocks.Count; i++)
                {
                    if (_availableBlocks[i].BlockType == type)
                    {
                        PlayAreaBlock returnBlock = _availableBlocks[i];
                        _availableBlocks.RemoveAt(i);
                        return returnBlock;
                    }
                }
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("No more " + type.ToString() + " BLOCKS in pool");
            return null;

        }
        internal PlayAreaBlock GetNextAvailable()
        {
            if (_availableBlocks.Count > 0)
            {
                PlayAreaBlock returnBlock = _availableBlocks[0];
                _availableBlocks.RemoveAt(0);
                return returnBlock;
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("NO more BLOCKS in pool");
            return null;
        }

        internal void Return(PlayAreaBlock block)
        {
            PutInPool(block);
        }

        private void InitializePool()
        {

            for (int i = 0; i < _blockPrefabConfig.Count; i++)
            {
                for (int j = 0; j < _maxPerType; j++)
                {
                    PutInPool(Instantiate(_blockPrefabConfig[i].Prefab) as PlayAreaBlock);
                }
            }

            _isInitialized = true;
        }
        private void PutInPool(PlayAreaBlock block)
        {
            //block.transform.parent = transform;
            block.transform.SetParent(transform);
            block.transform.localPosition = Statics.Vector3Zero();
            block.gameObject.SetActive(false);

            _availableBlocks.Add(block);
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
        public struct BlockPrefabConfig
        {
            public BlockTypes Type;
            public PlayAreaBlock Prefab;
        }

        //[System.Serializable]
        //public struct BlockLevel
        //{
        //    public int Num;
        //    public Sprite Sprite;
        //}
    }
    //public enum BlockTypes
    //{
    //    None = 0,
    //    PlateLight = 1,
    //    PlateMedium = 2,
    //    PlateHeavy = 3,
    //}
}
