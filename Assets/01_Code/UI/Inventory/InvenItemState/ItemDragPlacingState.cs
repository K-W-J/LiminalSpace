using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using KWJ.Code.Interactable.PickUpable;

namespace KWJ.Code.UI.Inventory
{
    public class ItemDragPlacingState : ItemDragState
    {
        private PickUpableObject _currentPickUp;
        private InventorySlot _currentSlot;
        private InventoryManager _inventoryManager;
        
        public override void Initialize(InventoryItem currentItem)
        {
            base.Initialize(currentItem);
            
            _currentSlot = m_currentItem.CurrentSlot;
            _inventoryManager = m_currentItem.InventoryManager;
            _currentPickUp = m_currentItem.CurrentPickUp;
        }
        public override void Enter(PointerEventData eventData)
        {
            m_eventData = eventData;
            
            if(m_eventData == null) return;
            
            if (m_eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick();
            }
            else if (m_eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick();
            }
        }

        public override void Exit()
        {
            m_eventData = null;
        }

        protected override void OnLeftClick()
        {
            HandleDragDrop(m_eventData, PointerEventData.InputButton.Left);
        }
        
        protected override void OnRightClick()
        {
            HandleDragDrop(m_eventData, PointerEventData.InputButton.Right);
        }

        private GameObject UIRaycast(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            
            if (results.Count > 0 && results[1].gameObject != null) //현재 자기 자신을 제외한 바로 아래 UI가져오기
                return results[1].gameObject;
            
            return null;
        }
        
        private void HandleDragDrop(PointerEventData eventData, PointerEventData.InputButton button)
        {
            GameObject invenSlot = UIRaycast(eventData);
            
            if (invenSlot == null)
            {
                Debug.Log("아이템 버리기");
                //m_currentItem.PickUpStateMaintain();
                return;
            }

            if (invenSlot.TryGetComponent<IPlaceableInven>(out var inventory))
            {
                if (button == PointerEventData.InputButton.Left)
                {
                    inventory.OnLeftClick(m_currentItem);
                }
                else
                {
                    inventory.OnRightClick(m_currentItem);
                }
                return;
            }
            
            m_currentItem.PickUpStateMaintain();
        }
    }
}