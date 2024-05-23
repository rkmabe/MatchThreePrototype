using System;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaBlock
{

    public class BlockHandler : MonoBehaviour , IBlockHandler 
    {
        [SerializeField] private Image _blockImage;
        private Block _block;

        public BlockStateMachine StateMachine { get => _stateMachine; }
        private BlockStateMachine _stateMachine;

        public bool IsProcessingRemoval { get => _isProcessingRemoval; }
        private bool _isProcessingRemoval;

        public static Action<Block> OnPooledBlockReturn;

        public void SetBlock(Block block)
        {
            _block = block;

            _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, Statics.BLOCK_ALPHA_ON);
            _blockImage.sprite = block.CurrentSprite;
        }

        public void RemoveBlockLevel()
        {

            bool allLevelsRemoved;
            _block.RemoveLevel(out allLevelsRemoved);
            if (allLevelsRemoved)
            {
                _block.RestoreFromCache();

                OnPooledBlockReturn?.Invoke(_block);

                _block = null;
            }
            else
            {
                _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, Statics.BLOCK_ALPHA_ON);
                _blockImage.sprite = _block.CurrentSprite;
            }
        }

        public Block GetBlock()
        {
            return _block;
        }
        public Image GetImage()
        {
            return _blockImage;
        }


        public void StartRemoval()
        {
            _isProcessingRemoval = true;
        }
        public bool GetIsProcessingRemoval()
        {
            return _isProcessingRemoval;
        }
        public void FinishRemoval()
        {
            _isProcessingRemoval = false;

            RemoveBlockLevel();

        }

        public void UpdateStateMachine()
        {
            _stateMachine.Update();
        }

        private void Awake()
        {
            _stateMachine = new BlockStateMachine(this);
            _stateMachine.Initialize(_stateMachine.IdleState);
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