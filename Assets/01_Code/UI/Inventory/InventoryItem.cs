using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using KWJ.Code.Interactable.PickUpable;
using KWJ.Code.Define;

namespace KWJ.Code.UI.Inventory
{
    public class InventoryItem : MonoBehaviour, IPlaceableInven
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _itemCountText;
        
        [SerializeField] private List<ItemDragState> _dragStates = new List<ItemDragState>();
        public ItemDragState CurrentDragState => _currentDragState;
        private ItemDragState _currentDragState;
        public ItemDragStateType CurrentDragStateType { get; private set; }
        public ItemDragStateType NextDragStateType { get; private set; }

        private Dictionary<ItemDragStateType, ItemDragState> _states = new Dictionary<ItemDragStateType, ItemDragState>();
        public PickUpableObject CurrentPickUp => _currentPickUp;
        private PickUpableObject _currentPickUp;
        public InventoryManager InventoryManager => _inventoryManager;
        private InventoryManager _inventoryManager;
        public InventorySlot CurrentSlot => _currentSlot;
        private InventorySlot _currentSlot;
        public Vector3 ReturnPosition => _returnPosition;
        private Vector3 _returnPosition;
        public int MaximumStack => _maximumStack;
        private int _maximumStack;
        public int CurrentStack => _currentStack;
        private int _currentStack;
        public bool CanStack => _currentStack < _maximumStack;

        public void ChangeState(ItemDragStateType dragState, PointerEventData eventData) //현재 State를 설정
        {
            NextDragStateType = ItemDragStateType.None;
            
            _currentDragState.Exit();
            _currentDragState = GetState(dragState);
            _currentDragState.Enter(eventData);
        }
        
        public void NextChangeState(ItemDragStateType dragState) // 다음에 올 State를 설정
        {
            //버튼을 처음 눌렀을 때 -> 버튼을 처음 눌렀다가 땠을 때 -> 버튼을 두번째 눌르면서 땠을 때 -> 반복
            NextDragStateType = dragState;
            CurrentDragStateType = NextDragStateType;
        }
        
        private ItemDragState GetState(ItemDragStateType stateType)
        {
            foreach (var state in _states)
            {
                if (state.Key == stateType)
                {
                    CurrentDragStateType = stateType;
                    return state.Value;
                }
            }
            
            return null;
        }
        
        public void OnLeftClick(InventoryItem item) // 매개변수 item의 모든 Stack을 현재 item에 합치기
        {
            if (item.IsCanItemStack(this) == false)
            {
                item.SwapSlot(this);
                return;
            }
            
            int remain = ModifyStack(item.CurrentStack);
                        
            if (remain > 0)
            {
                // 나머지가 있다면 매개변수 item의 Stack을 나머지로 초기화하고 PickUpState 유지하기
                item.ModifyStack(remain, true);
                item.PickUpStateMaintain();
            }
            else
                item.DestoryInvenItem();
        }

        public void OnRightClick(InventoryItem item) // 매개변수 item의 Stack 하나를 현재 item에 합치기
        {
            if (item.IsCanItemStack(this) == false)
            {
                item.PickUpStateMaintain();
                return;
            }
            
            ModifyStack(1);
            item.ModifyStack(-1);
                        
            if (item.CurrentStack > 0)
            {
                // 나머지가 있다면 매개변수 item의 Stack을 나머지로 초기화하고 PickUpState 유지하기
                item.PickUpStateMaintain();
            }
            else
                item.DestoryInvenItem();
        }

        private void SwapSlot(InventoryItem item)
        {
            SetCurrentSlot(item.CurrentSlot);
            transform.position = _returnPosition;
            
            if (_currentSlot != null)
            {
                NextChangeState(ItemDragStateType.ClickBefore);
            }

            _inventoryManager.SetCurrentItem(item);
            
            item.PickUpInvenItem();
            item.ChangeState(ItemDragStateType.PickedUp, null);
            item.NextChangeState(ItemDragStateType.Placing);
        }
        public void PickUpInvenItem()
        {
            if(_currentSlot == null) return;

            transform.SetParent(transform.parent.parent.parent);
            _returnPosition = _currentSlot.transform.position;
        }
        
        public void SetCurrentSlot(InventorySlot slot)
        {
            _currentSlot = slot;
            _currentSlot.SetInvenSlotItem(this);
            
            transform.SetParent(_currentSlot.transform);
            _returnPosition = _currentSlot.transform.position;
        }
        
        public int ModifyStack(int stack, bool isStackReset = false)
        {
            if (isStackReset)
                _currentStack = 0;
            
            int currentStack = _currentStack + stack;

            if (_maximumStack >= currentStack)
            {
                _currentStack = currentStack;
                currentStack = 0;
            }
            else
            {
                _currentStack = _maximumStack;
                currentStack -= _maximumStack;
            }
            
            SetStackText();
            
            return currentStack;
        }
        
        public void InitInvenItem(PickUpableObject pickUpable, InventoryManager inventoryManager,
            InventorySlot inventorySlot = null, int currentStack = 0, bool isItemPickUp = false)
        {
            _inventoryManager = inventoryManager;
            _currentSlot = inventorySlot;

            _currentPickUp = pickUpable;
            _currentPickUp.transform.SetParent(transform);
            
            _currentPickUp.SetIsInvenPutIn(true);
            
            _maximumStack = pickUpable.PickUpableSO.MaximumStack;
            
            if(currentStack == 0)
                _currentStack = pickUpable.CurrentStack;
            else
                _currentStack = currentStack;
            
            _itemIcon.sprite = pickUpable.PickUpableSO.ItemIcon;
            
            SetStackText();
            
            foreach (var state in _dragStates)
            {
                state.Initialize(this);
                _states[state.DragStateType] = state;
            }

            if (isItemPickUp)
            {
                _currentDragState = GetState(ItemDragStateType.PickedUp);
                _inventoryManager.SetCurrentItem(this);
                NextChangeState(ItemDragStateType.Placing);
            }
            else
            {
                _currentDragState = GetState(ItemDragStateType.ClickBefore);
            }
        }
        
        //PickUpState 상태 유지시키기
        public void PickUpStateMaintain()
        {
            ChangeState(ItemDragStateType.PickedUp, null);
            NextChangeState(ItemDragStateType.Placing);
        }
        public void SetStackText() => _itemCountText.text = _currentStack.ToString();
        
        public void DestoryInvenItem()
        {
            Destroy(gameObject);
        }

        private bool IsCanItemStack(InventoryItem item)
        {
           return item.CurrentPickUp.PickUpableSO.ItemID == _currentPickUp.PickUpableSO.ItemID
                  && item.CanStack;
        }

    }
}