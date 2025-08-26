using Code.Define;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Code.Interactable.PickUpable;

namespace Code.UI.Inventory
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
        
        //첫번재 클릭 다음에 두번째 클릭하고 땔 때에 행동을 실행하기 위해 필요
        public ItemDragState DragState => _dragState;
        private ItemDragState _dragState = ItemDragState.None;
        public bool CanStack => _currentStack < _maximumStack;
        
        public void OnBeginLeftDrag(PointerEventData eventData)
        {
            if (_dragState == ItemDragState.None)
            {
                _dragState = ItemDragState.PickedUp;
                    
                _inventoryManager.SetCurrentItem(this);
                MoveInvenItem();
            }
            else if(_dragState == ItemDragState.PickedUp)
            {
                _dragState = ItemDragState.Placing;
                _canvasGroup.blocksRaycasts = false;
            }
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
            invenItem.InitInvenItem(halfPickUp, _inventoryManager, _currentSlot, 0, true);
                
                
            invenItem.transform.position = _currentSlot.transform.position;
            invenItem.transform.SetParent(transform.parent.parent.parent);
                
            _inventoryManager.SetCurrentItem(invenItem);
                
            SetStackText();
        }
        
        private void HandleDragDrop(PointerEventData eventData)
        {
            GameObject invenSlot = eventData.pointerCurrentRaycast.gameObject;

            if (invenSlot == null)
            {
                RollBackCurrentSlot();
                return;
            }
            
            if (invenSlot.TryGetComponent<InventorySlot>(out var slot))
            {
                if(_currentSlot.InvenItem == this)
                    _currentSlot.SetInvenSlotItem(null);
                
                _currentSlot = slot;
                _currentSlot.SetInvenSlotItem(this);
                
                _returnPosition = _currentSlot.transform.position;
                
                return;
            }
            else if (invenSlot.TryGetComponent<InventoryItem>(out var item))
            {
                if (item.CurrentPickUp.PickUpableSO.ItemID == _currentPickUp.PickUpableSO.ItemID
                    && item.CanStack && CanStack)
                {
                    int remain = item.AddStack(CurrentStack);

                    if (remain > 0)
                    {
                        AddStack(remain, true);
                        _dragState = ItemDragState.PickedUp;
                    }
                    else
                        Destroy(gameObject);
                }
                else
                {
                    SwapSlot(item);
                }
                
                return;
            }

            RollBackCurrentSlot();
        }

        private void SwapSlot(InventoryItem item)
        {
            SetCurrentSlot(item.CurrentSlot);

            item.MoveInvenItem();
            _inventoryManager.SetCurrentItem(item);
        }

        private void RollBackCurrentSlot()
        {
            _dragState = ItemDragState.None;
            _inventoryManager.SetCurrentItem(null);
            
            transform.SetParent(_currentSlot.transform);
            transform.position = _returnPosition;
        }

        private void MoveInvenItem()
        {
            _dragState = ItemDragState.PickedUp;
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
        
        public void OnEndLeftDrag(PointerEventData eventData)
        { 
            HandleDragDrop(eventData);
            
            if(_dragState == ItemDragState.PickedUp) return;
            
            _canvasGroup.blocksRaycasts = true;
            _dragState = ItemDragState.None;
            
            transform.SetParent(_currentSlot.transform);
            transform.position = _returnPosition;

        }
        
        public void OnEndRightDrag(PointerEventData eventData)
        {
            if(_currentSlot == null)
                _inventoryManager.PutInInventorySlot(this);
        }

        
        public int AddStack(int stack, bool isStackReset = false)
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
            InventorySlot inventoryItem = null, int currentStack = 0, bool isItemPickUp = false)
        {
            _inventoryManager = inventoryManager;
            _currentSlot = inventoryItem;
            
            _currentPickUp = pickUpable;
            _maximumStack = pickUpable.PickUpableSO.MaximumStack;
            
            if(currentStack == 0)
                _currentStack = pickUpable.CurrentStack;
            else
                _currentStack = currentStack;
            
            _dragState = ItemDragState.PickedUp;
            
            _itemIcon.sprite = pickUpable.PickUpableSO.ItemIcon;
            
            SetStackText();
        }

        private void SetStackText() => _itemCountText.text = _currentStack.ToString();
    }
}