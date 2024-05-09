using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThreePrototype.PlayAreaCellContent.PlayAreaItem
{

    public class ItemHandler : MonoBehaviour, IItemHandler
    {
        [SerializeField] private Image _itemImage;

        private Item _item;

        //public ItemStateMachine StateMachine;

        public ItemStateMachine StateMachine { get => _stateMachine; }
        private ItemStateMachine _stateMachine;

        public bool IsProcessingRemoval { get => _isProcessingRemoval;  }
        private bool _isProcessingRemoval;

        public void SetItem(Item item)
        {
            _item = item;
            _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_ON);
            _itemImage.sprite = item.Sprite;

            //_stateMachine.Initialize(_stateMachine.IdleState);
        }
        public Item GetItem()
        {
            return _item;
        }

        public void RemoveItemReferenceAndImage()
        {
            RemoveItemReference();
            RemoveItemImage();
        }

        public void RemoveItemReference()
        {
            _item = null;
        }
        public void RemoveItemImage()
        {
            _itemImage.color = new Color(_itemImage.color.r, _itemImage.color.g, _itemImage.color.b, Statics.ALPHA_OFF);
            _itemImage.sprite = null;
        }

        public Image GetImage()
        {
            return _itemImage;
        }

        public void StartRemoval()
        {
            // monitored by ItemIdleState; used to transition to RemovalState
            _isProcessingRemoval = true;
        }
        public bool GetIsProcessingRemoval()
        {
            return _isProcessingRemoval;
        }
        public void StopRemoval()
        {
            _isProcessingRemoval = false;
        }


        public void UpdateStateMachine()
        {
            _stateMachine.Update();
        }

        private void OnDestroy()
        {
            _stateMachine.CleanUpOnDestroy();
        }

        private void Awake()
        {
            _stateMachine = new ItemStateMachine(this);
            _stateMachine.Initialize(_stateMachine.IdleState);
        }

        //private void Update()
        //{
        //    _stateMachine.Update();
        //}



    }
}