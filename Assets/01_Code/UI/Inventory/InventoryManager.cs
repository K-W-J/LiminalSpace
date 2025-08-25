using System;
using System.Collections.Generic;
using System.Linq;
using Code.Interactable.PickUpable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.UI.Inventory
{
    //인벤토리의 전체 Slot 관리, Slot 컨트롤 및 Item 생성
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject _invenItemPrefab;
        
        private List<InventorySlot> _inventorySlots = new List<InventorySlot>();

        private InventoryItem _currentItem;
        private bool _isCurrentItemNull => _currentItem == null;

        private void Awake()
        {
            InventorySlot[] inventorySlots = GetComponentsInChildren<InventorySlot>();
            _inventorySlots = inventorySlots.ToList();

            foreach (var inventorySlot in _inventorySlots)
            {
                inventorySlot.InitInvenSlot(this);
            }
        }

        private void Update()
        {
            if(_isCurrentItemNull) return;
            
            if(_currentItem.IsItemPickUp)
                _currentItem.transform.position = Mouse.current.position.ReadValue();
        }

        public void SetCurrentItem(InventoryItem item)
        {
            _currentItem = item;
        }

        // ReSharper disable Unity.PerformanceAnalysis 
        public void PutInInventorySlot(PickUpableObject pickUpable)
        {
            InventorySlot putInableSlot = FindEmptyInventorySlot(pickUpable);

            if (putInableSlot == null)
            {
                if (pickUpable.IsInvenPutIn)
                {
                    //Take Out
                }
                
                return;
            }

            if (putInableSlot.IsSlotEmpty)
            {
                InventoryItem invenItem = CreateInvenItem(putInableSlot.transform);
                
                invenItem.InitInvenItem(pickUpable, this, putInableSlot);
                putInableSlot.InitInvenSlot(this);
                putInableSlot.SetInvenSlotItem(invenItem);
                
                pickUpable.SetIsInvenPutIn(true);
            }
            else
            {
                int remain = putInableSlot.InvenItem.AddStack(pickUpable.CurrentStack);
                pickUpable.SetStack(-remain);

                if (pickUpable.IsInvenPutIn)
                {
                    Destroy(pickUpable);
                }
                
                if (remain > 0)
                {
                    PutInInventorySlot(pickUpable);
                    return;
                }
            }
            
            pickUpable.gameObject.SetActive(false);
        }
        
        public void PutInInventorySlot(InventoryItem invenItem)
        {
            //InventorySlot putInableSlot = FindEmptyInventorySlot(invenItem);
        }
        
        public InventoryItem CreateInvenItem(Transform parent)
        {
            GameObject invenItemGameObject = Instantiate(_invenItemPrefab, parent);
            InventoryItem invenItem = invenItemGameObject.GetComponent<InventoryItem>();

            return invenItem;
        }

        private InventorySlot FindEmptyInventorySlot(PickUpableObject pickUpable)
        {
            foreach (var slot in _inventorySlots)
            {
                if (slot.CanPutInSlot(pickUpable))
                    return slot;
            }

            return null;
        }
    }
}