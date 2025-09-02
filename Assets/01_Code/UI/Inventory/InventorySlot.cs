using System;
using UnityEngine.UI;
using UnityEngine;
using KWJ.Code.Interactable.PickUpable;
using UnityEngine.EventSystems;

namespace KWJ.Code.UI.Inventory
{
    [Serializable]
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPlaceableInven
    {
        [SerializeField] private Image _slotImage;
        [SerializeField] [ColorUsage(true, false)] private Color _selectColor;
        [SerializeField] [ColorUsage(true, false)] private Color _defaultColor;
        public InventoryItem InvenItem => _invenItem;
        private InventoryItem _invenItem;
        public InventoryManager InvenManager => _invenManager;
        private InventoryManager _invenManager;
        public bool IsSlotEmpty => _invenItem == null;
        public void OnPointerEnter(PointerEventData eventData)
        {
            _slotImage.color = _selectColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _slotImage.color = _defaultColor;
        }
        
        public void InitInvenSlot(InventoryManager inventoryManager)
        {
            _invenManager = inventoryManager;
        }
        
        public void SetInvenSlotItem(InventoryItem inventoryItem)
        {
            _invenItem = inventoryItem;
        }
        
        public bool CanPutInSlot(PickUpableObject pickUpable)
        {
            if (IsSlotEmpty)
                return true;

            if (pickUpable.PickUpableSO.ItemID == InvenItem.CurrentPickUp.PickUpableSO.ItemID
                    && InvenItem.CanStack)
                return true;
            
            return false;
        }

        public void ClearSlot()
        {
            _invenItem = null;
        }

        public void OnLeftClick(InventoryItem item) //item 전체를 슬롯에 넣기
        {
            if (item.CurrentSlot != null)
            {
                if(InvenItem == item) //item의 원래 슬롯에서 제거
                    item.CurrentSlot.SetInvenSlotItem(null);
            }

            item.SetCurrentSlot(this); //item 슬롯을 현재 슬롯으로 설정
            _invenManager.SetCurrentItem(null);
            
            item.transform.position = item.ReturnPosition;
        }

        public void OnRightClick(InventoryItem item) //item 하나를 슬롯에 넣기
        {
            PickUpableObject halfPickUp = Instantiate(item.CurrentPickUp, transform);
            halfPickUp.AddStack(1);

            InventoryItem invenItem = _invenManager.CreateInvenItem(transform);
            invenItem.InitInvenItem(halfPickUp, _invenManager, this);
                    
            item.ModifyStack(-1);
                    
            item.PickUpStateMaintain();
        }
    }
}