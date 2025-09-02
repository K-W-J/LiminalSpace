using UnityEngine.EventSystems;
using KWJ.Code.Interactable.PickUpable;
using KWJ.Code.Define;

namespace KWJ.Code.UI.Inventory
{
    public class ItemDragClickBeforeState : ItemDragState
    {
        private int _currentStack;
        private PickUpableObject _currentPickUp;
        private InventorySlot _currentSlot;
        private InventoryManager _inventoryManager;
        
        private bool _isFirstClick;
        
        public override void Initialize(InventoryItem currentItem)
        {
            base.Initialize(currentItem);
            
            _currentPickUp = m_currentItem.CurrentPickUp;
            _inventoryManager = m_currentItem.InventoryManager;
        }

        public override void Enter(PointerEventData eventData)
        {
            m_eventData = eventData;
            _currentStack = m_currentItem.CurrentStack;
            _currentSlot = m_currentItem.CurrentSlot;
            
            if(m_eventData == null) return;
            
            if (m_eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick();
            }
            else if (m_eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick();
            }
            
            m_eventData = null;
        }

        protected override void OnLeftClick()
        {
            _inventoryManager.SetCurrentItem(m_currentItem);
            m_currentItem.PickUpInvenItem();
        }

        protected override void OnRightClick()
        {
            m_currentItem.NextChangeState(ItemDragStateType.ClickBefore);
            
            int halfA = _currentStack / 2;
            if(halfA <= 0) return;
                
            int halfB = _currentStack - halfA;
            m_currentItem.ModifyStack(halfA, true);

            PickUpableObject halfPickUp = Instantiate(_currentPickUp, _currentSlot.transform);
            halfPickUp.AddStack(halfB);

            InventoryItem invenItem = _inventoryManager.CreateInvenItem(transform.parent);
            invenItem.InitInvenItem(halfPickUp, _inventoryManager, null, 0, true);
                
            invenItem.transform.position = _currentSlot.transform.position;
            invenItem.transform.SetParent(transform.parent.parent.parent);
                
            m_currentItem.SetStackText();
        }
    }
}