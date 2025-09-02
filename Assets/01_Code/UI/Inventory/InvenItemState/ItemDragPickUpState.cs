using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace KWJ.Code.UI.Inventory
{
    public class ItemDragPickUpState : ItemDragState
    {
        public override void Enter(PointerEventData eventData)
        {
            m_eventData = eventData;
        }

        public override void StateUpdate()
        {
            m_currentItem.transform.position = Mouse.current.position.ReadValue();
        }
    }
}