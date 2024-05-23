using System.Collections.Generic;
using UnityEngine;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock
{

    public class BlockPool : MonoBehaviour
    {

        [Header("Must be at least max PlayAreaSize")]
        [SerializeField] private int _maxPerType = 81;

        [SerializeField] private List<BlockPrefabConfig> _blockPrefabConfig = new List<BlockPrefabConfig>();


        private List<Block> _availableBlocks = new List<Block>();

        public bool IsInitialized { get => _isInitialized; }
        private bool _isInitialized = false;

        internal Block GetNextAvailable(BlockTypes type)
        {
            if (_availableBlocks.Count > 0)
            {
                for (int i = 0; i < _availableBlocks.Count; i++)
                {
                    if (_availableBlocks[i].BlockType == type)
                    {
                        Block returnBlock = _availableBlocks[i];
                        _availableBlocks.RemoveAt(i);
                        return returnBlock;
                    }
                }
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("No more " + type.ToString() + " BLOCKS in pool");
            return null;

        }
        internal Block GetNextAvailable()
        {
            if (_availableBlocks.Count > 0)
            {
                Block returnBlock = _availableBlocks[0];
                _availableBlocks.RemoveAt(0);
                return returnBlock;
            }

            // TODO: handle possible exception - not enough pins in pool
            Debug.LogError("NO more BLOCKS in pool");
            return null;
        }

        private void InitializePool()
        {

            for (int i = 0; i < _blockPrefabConfig.Count; i++)
            {
                for (int j = 0; j < _maxPerType; j++)
                {
                    PutInPool(Instantiate(_blockPrefabConfig[i].Prefab) as Block);
                }
            }

            _isInitialized = true;
        }
        private void PutInPool(Block block)
        {
            block.transform.SetParent(transform);
            block.transform.localPosition = Statics.Vector3Zero();
            block.gameObject.SetActive(false);

            _availableBlocks.Add(block);
        }


        internal void OnBlockReturn(Block block)
        {
            PutInPool(block);
        }


        private void OnDestroy()
        {
            BlockHandler.OnPooledBlockReturn -= OnBlockReturn;
        }

        private void Awake()
        {
            InitializePool();

            BlockHandler.OnPooledBlockReturn += OnBlockReturn;
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
            public Block Prefab;
        }

    }

}
