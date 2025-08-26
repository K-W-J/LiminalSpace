using Code.Define;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.Inventory
{
    public class InventoryItemDragHandler : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
    {
        [SerializeField] private InventoryItem _invenItem;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) //아이템 들어올리기
            {
                print(2424);
                _invenItem.OnBeginLeftDrag(eventData);
            }
            else if (eventData.button == PointerEventData.InputButton.Right) //아이템 나눠서 들어올리기
            {
                _invenItem.OnBeginRightDrag(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) //아이템 전체 합치기, 아이템 바꾸기  
            {
                if (_invenItem.DragState != ItemDragState.Placing) return;
                
                _invenItem.OnEndLeftDrag(eventData);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)  //아이템 하나씩 합치기, 아이템 바꾸기
            {
                _invenItem.OnEndRightDrag(eventData);
            }
        }
    }
}