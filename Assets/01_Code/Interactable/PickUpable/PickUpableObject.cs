using Code.Entities;
using Code.SO;
using Code.UI.Inventory;
using UnityEngine;

namespace Code.Interactable.PickUpable
{
    public class PickUpableObject : InteractCommand
    {
        [field:SerializeField] public PickUpableSO PickUpableSO { get; private set; }
        [SerializeField] private int _startStack;
        public int CurrentStack => _currentStack;
        private int _currentStack;

        public bool IsInvenPutIn => _isInvenPutIn;
        private bool _isInvenPutIn;

        private void Awake() {
            if(_startStack > 1)
                _currentStack = _startStack;
        }
        
        public void SetIsInvenPutIn(bool isInvenPutIn)
        {
            _isInvenPutIn = isInvenPutIn;
        }

        public void AddStack(int stack)
        {
            _currentStack += stack;
        }
        
        public override void Execute(Entity entity)
        {
            entity.GetCompo<PlayerInventoryBar>().InventoryManager.PutInInventorySlot(this);
        }
    }
}