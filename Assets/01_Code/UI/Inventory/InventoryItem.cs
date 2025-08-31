using KWJ.Code.Define;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using KWJ.Code.Interactable.PickUpable;

namespace KWJ.Code.UI.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _itemCountText;
        [SerializeField] private CanvasGroup _canvasGroup;
        public PickUpableObject CurrentPickUp => _currentPickUp;
        private PickUpableObject _currentPickUp;
        
        private InventoryManager _inventoryManager;
        
        public InventorySlot CurrentSlot => _currentSlot;
        private InventorySlot _currentSlot;
        
        private Vector3 _returnPosition;
        public int MaximumStack => _maximumStack;
        private int _maximumStack;
        public int CurrentStack => _currentStack;
        private int _currentStack;
        
        //첫 클릭 다음에 두번째 클릭하고 땔 때에 행동을 실행하기 위해 필요
        public ItemDragStateType DragStateType => _dragStateType;
        private ItemDragStateType _dragStateType = ItemDragStateType.Default;
        public bool CanStack => _currentStack < _maximumStack;
        
        public void OnBeginLeftDrag(PointerEventData eventData)
        {

        }
        
        public void OnBeginRightDrag(PointerEventData eventData)
        {
            int halfA = _currentStack / 2;
            if(halfA <= 0) return;
                
            int halfB = _currentStack - halfA;
            _currentStack = halfA;

            PickUpableObject halfPickUp = Instantiate(_currentPickUp, _currentSlot.transform);
            halfPickUp.AddStack(halfB);

            InventoryItem invenItem = _inventoryManager.CreateInvenItem(transform.parent);
            invenItem.InitInvenItem(halfPickUp, _inventoryManager, null, 0, true);
                
            invenItem.transform.position = _currentSlot.transform.position;
            invenItem.transform.SetParent(transform.parent.parent.parent);
                
            _inventoryManager.SetCurrentItem(invenItem);
                
            SetStackText();
        }
        
        private void HandleDragDrop(PointerEventData eventData, PointerEventData.InputButton button)
        {
            GameObject invenSlot = eventData.pointerCurrentRaycast.gameObject;

            if (invenSlot == null)
            {
                RollBackCurrentSlot();
                
                return;
            }
            
            if (invenSlot.TryGetComponent<InventorySlot>(out var slot))
            {
                if (button == PointerEventData.InputButton.Left)
                {
                    if (_currentSlot != null)
                    {
                        if(_currentSlot.InvenItem == this)
                            _currentSlot.SetInvenSlotItem(null);
                    }
                    
                    _currentSlot = slot;
                    _currentSlot.SetInvenSlotItem(this);

                    _returnPosition = _currentSlot.transform.position;

                    RollBackCurrentSlot();
                }
                else
                {
                    PickUpableObject halfPickUp = Instantiate(_currentPickUp, _currentSlot.transform);
                    halfPickUp.AddStack(1);

                    InventoryItem invenItem = _inventoryManager.CreateInvenItem(slot.transform);
                    invenItem.InitInvenItem(halfPickUp, _inventoryManager, _currentSlot);
                    
                    ModifyStack(-1);
                    
                    _dragStateType = ItemDragStateType.PickedUp;
                    _canvasGroup.blocksRaycasts = true;
                }
                
                return;
            }
            else if (invenSlot.TryGetComponent<InventoryItem>(out var item))
            {
                if (item.CurrentPickUp.PickUpableSO.ItemID == _currentPickUp.PickUpableSO.ItemID && item.CanStack)
                {
                    if (button == PointerEventData.InputButton.Left)
                    {
                        int remain = item.ModifyStack(CurrentStack);
                        
                        if (remain > 0)
                        {
                            ModifyStack(remain, true);
                            _dragStateType = ItemDragStateType.PickedUp;
                            _canvasGroup.blocksRaycasts = true;
                        }
                        else
                            Destroy(gameObject);
                        
                        
                    }
                    else
                    {
                        item.ModifyStack(1);
                        ModifyStack(-1);
                        
                        if (_currentStack > 0)
                        {
                            _dragStateType = ItemDragStateType.PickedUp;
                            _canvasGroup.blocksRaycasts = true;
                        }
                        else
                            Destroy(gameObject);
                    }
                }
                else
                {
                    SwapSlot(item);
                }
                
                return;
            }
            
            RollBackCurrentSlot();
        }
        
        public void OnEndLeftDrag(PointerEventData eventData)
        { 
            HandleDragDrop(eventData, PointerEventData.InputButton.Left);
        }
        
        public void OnEndRightDrag(PointerEventData eventData)
        {
            HandleDragDrop(eventData, PointerEventData.InputButton.Right);
        }

        public void ChangeDragState()
        {
            if (_dragStateType == ItemDragStateType.Default)
            {
                _dragStateType = ItemDragStateType.PickedUp;
                    
                _inventoryManager.SetCurrentItem(this);
                MoveInvenItem();
            }
            else if(_dragStateType == ItemDragStateType.PickedUp)
            {
                _dragStateType = ItemDragStateType.Placing;
                _canvasGroup.blocksRaycasts = false;
            }
        }

        private void SwapSlot(InventoryItem item)
        {
            SetCurrentSlot(item.CurrentSlot);

            item.MoveInvenItem();
            _inventoryManager.SetCurrentItem(item);
        }

        private void RollBackCurrentSlot()
        {
            _dragStateType = ItemDragStateType.Default;
            _inventoryManager.SetCurrentItem(null);
            
            if(_currentSlot != null)
                transform.SetParent(_currentSlot.transform);
            
            transform.position = _returnPosition;
            _canvasGroup.blocksRaycasts = true;
        }

        private void MoveInvenItem()
        {
            _dragStateType = ItemDragStateType.PickedUp;
            transform.SetParent(transform.parent.parent.parent);
            _returnPosition = _currentSlot.transform.position;
        }
        
        public void SetCurrentSlot(InventorySlot item)
        {
            if(_currentSlot != null)
                _currentSlot.SetInvenSlotItem(null);
            
            _currentSlot = item;
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
            
            if(isItemPickUp)
                _dragStateType = ItemDragStateType.PickedUp;
            
            _itemIcon.sprite = pickUpable.PickUpableSO.ItemIcon;
            
            SetStackText();
        }

        private void SetStackText() => _itemCountText.text = _currentStack.ToString();
    }
}