using UnityEngine.EventSystems;
using UnityEngine;
using KWJ.Code.Define;

namespace KWJ.Code.UI.Inventory
{
    public class InventoryItemDragHandler : MonoBehaviour, IPointerDownHandler , IPointerUpHandler, IDragHandler
    {
        [SerializeField] private InventoryItem _invenItem;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) //아이템 들어올리기
            {
                _invenItem.ChangeDragState();
                _invenItem.OnBeginLeftDrag(eventData);
            }
            else if (eventData.button == PointerEventData.InputButton.Right) //아이템 나눠서 들어올리기
            {
                if (_invenItem.DragStateType == ItemDragStateType.Default)
                {
                    _invenItem.OnBeginRightDrag(eventData);
                }
                else
                {
                    _invenItem.ChangeDragState();
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(_invenItem.DragStateType != ItemDragStateType.Placing) return;
            
            if (eventData.button == PointerEventData.InputButton.Left) //아이템 전체 합치기, 아이템 바꾸기  
            {
                _invenItem.OnEndLeftDrag(eventData);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)  //아이템 하나씩 합치기, 아이템 바꾸기
            {
                _invenItem.OnEndRightDrag(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}