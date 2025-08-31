using System;
using UnityEngine.UI;
using UnityEngine;
using KWJ.Code.Interactable.PickUpable;

namespace KWJ.Code.UI.Inventory
{
    [Serializable]
    public class InventorySlot : MonoBehaviour
    {
        public InventoryItem InvenItem => _invenItem;
        private InventoryItem _invenItem;
        
        public InventoryManager InvenManager => _invenManager;
        private InventoryManager _invenManager;
        
        public bool IsSlotEmpty => _invenItem == null;
        
        private Image _seledSlotImage;
        private Image _defaultImage;
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
    }
}