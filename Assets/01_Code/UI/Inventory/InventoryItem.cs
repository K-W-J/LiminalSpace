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
        
        public bool IsItemPickUp => _isItemPickUp;
        private bool _isItemPickUp;
        
        public bool IsItemPutDown => _isItemPutDown;
        private bool _isItemPutDown;
        
        public bool CanStack => _currentStack < _maximumStack;
        
        public void OnBeginLeftDrag(PointerEventData eventData)
        {
            if (!_isItemPickUp)
            {
                _isItemPickUp = true;
                _isItemPutDown = false;
                    
                _inventoryManager.SetCurrentItem(this);
                MoveInvenItem();
            }
            else
            {
                _isItemPutDown = true;
                
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
            halfPickUp.SetStack(halfB);

            InventoryItem invenItem = _currentSlot.InvenManager.CreateInvenItem(transform.parent);
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
                    && item.CanStack)
                {
                    /*int remain = item.AddStack(pickUpable.CurrentStack);
                    pickUpable.SetStack(-remain);*/
                }
                else
                {
                    SetCurrentSlot(item.CurrentSlot);

                    item.MoveInvenItem();
                    _inventoryManager.SetCurrentItem(item);
                }
                
                return;
            }

            RollBackCurrentSlot();
        }

        private void RollBackCurrentSlot()
        {
            _isItemPickUp = false;
            _inventoryManager.SetCurrentItem(null);
            transform.SetParent(_currentSlot.transform);
            transform.position = _returnPosition;
        }

        private void MoveInvenItem()
        {
            _isItemPickUp = true;
            transform.SetParent(transform.parent.parent.parent);
            _returnPosition = _currentSlot.transform.position;
        }
        
        private void SetCurrentSlot(InventorySlot item)
        {
            _currentSlot.SetInvenSlotItem(null);
            _currentSlot = item;
            _currentSlot.SetInvenSlotItem(this);
            
            transform.SetParent(_currentSlot.transform);
            _returnPosition = _currentSlot.transform.position;
        }
        
        public void OnEndLeftDrag(PointerEventData eventData)
        { 
            HandleDragDrop(eventData);

            _canvasGroup.blocksRaycasts = true;
            
            _isItemPickUp = false;
            transform.SetParent(_currentSlot.transform);
            transform.position = _returnPosition;
        }
        
        public void OnEndRightDrag(PointerEventData eventData)
        {
            
        }

        
        public int AddStack(int stack)
        {
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
            
            _isItemPickUp = isItemPickUp;
            _isItemPutDown = isItemPickUp;
            
            _itemIcon.sprite = pickUpable.PickUpableSO.ItemIcon;
            
            SetStackText();
        }

        private void SetStackText() => _itemCountText.text = _currentStack.ToString();
    }
}