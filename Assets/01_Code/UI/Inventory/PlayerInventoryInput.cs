using Code.Interactable.PickUpable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Code.UI.Inventory
{
    public class PlayerInventoryInput : MonoBehaviour//, IPointerDownHandler , IPointerUpHandler
    {
        private InventoryItem _currentItem;
        private PickUpableObject _tempPickUp;
        /*private void Update()
        {
            if(_currentItem == null) return;
            
            if(_currentItem.IsItemPickUp)
                transform.position = Mouse.current.position.ReadValue();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_currentItem == null)
            {   
                _currentItem = eventData.hovered[^1].GetComponent<InventoryItem>();
                print(gameObject);
            }
            
            if(_currentItem == null) return;
            
            if (eventData.button == PointerEventData.InputButton.Left) //아이템 들어올리기  
            {
                _currentItem.InputMouseDownLeft(_currentItem.gameObject);
                
                /*_halfPickUp = Instantiate(_currentPickUp, _currentSlot.transform);
                _halfPickUp.AddStack(halfB);

                InventoryItem invenItem = _currentSlot.InvenManager.CreateInvenItem(transform.parent);
                invenItem.InitInvenItem(_halfPickUp, null, 0, true);
                
                _currentSlot.InvenManager.PutInInventorySlot(_halfPickUp);#1#
            }
            else if (eventData.button == PointerEventData.InputButton.Right) //아이템 나눠서 들어올리기
            {
                _currentItem.InputMouseDownRight(_currentItem.gameObject);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(_currentItem == null) return;
            
            if (eventData.button == PointerEventData.InputButton.Left) //아이템 하나씩 놓기
            {
                _currentItem.InputMouseUpLeft(_currentItem.gameObject);
                
                /*if (_halfPickUp != null)
                {
                    _currentSlot.InvenManager.PutInInventorySlot(_halfPickUp);
                    _halfPickUp = null;
                    return;    
                }#1#
            }
            else if (eventData.button == PointerEventData.InputButton.Right) //아이템 전체 놓기
            {
                _currentItem.InputMouseUpRight(_currentItem.gameObject);
                
                            
            }
            
            if (_currentItem == null)
            {
                _currentItem = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventoryItem>();
            }
        }*/
    }
}