using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Code.Interactable.PickUpable;

namespace Code.UI.Inventory
{
    public class InventoryItem : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _itemCountText;
        [SerializeField] private CanvasGroup _canvasGroup;
        public PickUpableObject CurrentPickUp => _currentPickUp;
        private PickUpableObject _currentPickUp;
        
        private InventorySlot _currentSlot;
        
        private PickUpableObject _halfPickUp;
        
        private Vector3 _returnPosition;
        
        public int MaximumStack => _maximumStack;
        private int _maximumStack;
        public int CurrentStack => _currentStack;
        private int _currentStack;
        
        public bool CanStack => _currentStack < _maximumStack;
        
        public bool IsItemPickUp => _isItemPickUp;
        private bool _isItemPickUp;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) //아이템 들어올리기  
            {
                InputMouseDownLeft(eventData);
            }
            else if (eventData.button == PointerEventData.InputButton.Right) //아이템 나눠서 들어올리기
            {
                InputMouseDownRight(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) //아이템 하나씩 놓기
            {
            }
            else if (eventData.button == PointerEventData.InputButton.Right) //아이템 전체 놓기
            {
                            
            }
        }

        public void InputMouseDownRight(PointerEventData eventData)
        {
            int halfA = _currentStack / 2;
            int halfB = _currentStack - halfA;

            _currentStack = halfA;

            _halfPickUp = Instantiate(_currentPickUp, _currentSlot.transform);
            _halfPickUp.SetStack(halfB);

            InventoryItem invenItem = _currentSlot.InvenManager.CreateInvenItem(transform.parent);
            invenItem.InitInvenItem(_halfPickUp, null, 0, true);
                
            _currentSlot.InvenManager.PutInInventorySlot(_halfPickUp);
                
            SetItemCountText();
        }
        
        public void InputMouseDownLeft(PointerEventData eventData)
        {
            if (_isItemPickUp)
            {
                _isItemPickUp = false;
                _canvasGroup.blocksRaycasts = false;
                transform.SetParent(_currentSlot.transform);
                return;
            }
            
            _isItemPickUp = true;
            
            transform.SetParent(transform.parent.parent.parent);
            
            _returnPosition = transform.position;
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
            
            SetItemCountText();
            
            return currentStack;
        }
        
        public void InitInvenItem(PickUpableObject pickUpable, InventorySlot inventoryItem = null, int currentStack = 0, bool isItemPickUp = false)
        {
            _currentSlot = inventoryItem;
            
            _currentPickUp = pickUpable;
            _maximumStack = pickUpable.PickUpableSO.MaximumStack;
            
            if(currentStack == 0)
                _currentStack = pickUpable.CurrentStack;
            else
                _currentStack = currentStack;
            
            _isItemPickUp = isItemPickUp;
            
            _itemIcon.sprite = pickUpable.PickUpableSO.ItemIcon;
            
            SetItemCountText();
        }

        private void SetItemCountText()
        {
            _itemCountText.text = _currentStack.ToString();
        }
    }
}