using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.Block
{

    public class PlayAreaBlockHandler : MonoBehaviour , IPlayAreaBlockHandler 
    {
        [SerializeField] private Image _blockImage;
        private PlayAreaBlock _block;

        public void SetBlock(PlayAreaBlock block)
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
                // TODO: restore this to prefab state before returning to pool .. 
                // any missing levels must be restored .. just cache them .. 
                //_blockPool.Return(_block);
                Debug.Log("RETURN TO POOL!");

                _block = null;
            }
            else
            {
                _blockImage.color = new Color(_blockImage.color.r, _blockImage.color.g, _blockImage.color.b, Statics.BLOCK_ALPHA_ON);
                _blockImage.sprite = _block.CurrentSprite;
            }
        }

        public PlayAreaBlock GetBlock()
        {
            return _block;
        }
        public Image GetImage()
        {
            return _blockImage;
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