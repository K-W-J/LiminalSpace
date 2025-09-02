using KWJ.Code.Define;
using UnityEngine.EventSystems;
using UnityEngine;

namespace KWJ.Code.UI.Inventory
{
    public class InventoryItemDragHandler : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
    {
        [SerializeField] private InventoryItem _currentItem;
        
        private int _pointerId;
        private bool _isClick;
        public void OnPointerDown(PointerEventData eventData)
        {
            if(_isClick || eventData.button == PointerEventData.InputButton.Middle) 
                return;
            
            _isClick = true;
            _pointerId = eventData.pointerId;

            /*
            버튼을 처음 눌렀을 때 (ClickBefore) -> 버튼을 처음 눌렀다가 땠을 때 (PickedUp)
            -> 버튼을 두번째 눌르면서 땠을 때 (Placing)
            위 과정의 반복이라 NextChangeState()으로 다음 State를 설정하여 버튼 클릭으로 인해 State를 꼬이지 않게 만듦
            */
            
            if (_currentItem.CurrentDragStateType == ItemDragStateType.ClickBefore)
            {
                _currentItem.ChangeState(ItemDragStateType.ClickBefore, eventData);
                
                if(_currentItem.NextDragStateType == ItemDragStateType.None)
                    _currentItem.NextChangeState(ItemDragStateType.PickedUp);
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if(_pointerId != eventData.pointerId)
                return;
            
            if (_currentItem.CurrentDragStateType == ItemDragStateType.PickedUp)
            {
                _currentItem.ChangeState(ItemDragStateType.PickedUp, eventData);
                
                if(_currentItem.NextDragStateType == ItemDragStateType.None)
                    _currentItem.NextChangeState(ItemDragStateType.Placing);
            }
            else if (_currentItem.CurrentDragStateType == ItemDragStateType.Placing)
            {
                _currentItem.ChangeState(ItemDragStateType.Placing, eventData);
                
                if(_currentItem.NextDragStateType == ItemDragStateType.None)
                    _currentItem.NextChangeState(ItemDragStateType.ClickBefore);
            }

            _isClick = false;
            _pointerId = -1;
        }
    }
}