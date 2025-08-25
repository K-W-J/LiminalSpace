using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.Inventory
{
    public class InventoryItemDragHandler : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
    {
        [SerializeField] private InventoryItem _item;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) //아이템 들어올리기
            {
                _item.OnBeginLeftDrag(eventData);
            }
            else if (eventData.button == PointerEventData.InputButton.Right) //아이템 나눠서 들어올리기
            {
                _item.OnBeginRightDrag(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) //아이템 전체 합치기, 아이템 바꾸기  
            {
                if (!_item.IsItemPutDown) return;
                
                _item.OnEndLeftDrag(eventData);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)  //아이템 하나씩 합치기, 아이템 바꾸기
            {
                            
            }
        }
    }
}