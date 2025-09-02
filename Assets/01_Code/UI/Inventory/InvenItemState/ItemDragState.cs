using KWJ.Code.Define;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KWJ.Code.UI.Inventory
{
    public class ItemDragState : MonoBehaviour
    {
        [field: SerializeField] public ItemDragStateType DragStateType { get; private set; }

        protected InventoryItem m_currentItem;
        protected PointerEventData m_eventData;

        public virtual void Initialize(InventoryItem currentItem)
        {
            m_currentItem = currentItem;
        }
        public virtual void Enter(PointerEventData eventData)
        {
            m_eventData = eventData;
        }
        public virtual void Exit()
        {
            
        }
        public virtual void StateUpdate()
        {

        }
        protected virtual void OnLeftClick()
        {
            
        }
        protected virtual void OnRightClick()
        {
            
        }
        protected virtual void OnCtrlRightClick()
        {
            
        }
        protected virtual void OnShiftRightClick()
        {
            
        }
    }
}