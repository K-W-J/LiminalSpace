using System.Collections.Generic;
using UnityEngine;
using Code.Entities;
using Code.Interactable.PickUpable;
using Code.Players;

namespace Code.UI.Inventory
{
    public class PlayerInventory : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private int inventoryCount;
    
        [SerializeField] private Transform inventoryPanel;

        [SerializeField] private GameObject _inventorySolt;

        private List<InventorySlot> _inventorySlots = new List<InventorySlot>();
        
        private Player _agent;
    
        private int _currentSelectSlot;
        
        public void Initialize(Entity entity)
        {
            _agent = entity as Players.Player;

            SettingInventroy();
        }

        private void SettingInventroy()
        {
            if(_inventorySolt ==null) return;
            
            for (int i = 0; i < inventoryCount; i++)
            {
                GameObject solt = Instantiate(_inventorySolt, inventoryPanel);
                _inventorySlots.Add(solt.GetComponent<InventorySlot>());
            }
        }


        public PickUpCommand GetSoltItem()
        {
            if(!IsSlotSelected()) return null;

            return _inventorySlots[_currentSelectSlot].InventorySolt;
        }

        public void SelectSolt(int selectSlot)
        {
            if (_currentSelectSlot == selectSlot)
            {
                if (_inventorySlots[_currentSelectSlot].IsSeled)
                {
                    SetSeledSlot(false);
                }
                else
                {
                    SetSeledSlot(true);
                }
            }
            else
            {
                SetSeledSlot(false);
            
                _currentSelectSlot = selectSlot;
            
                SetSeledSlot(true);
            }
        }


        public void PutInInventory(PickUpCommand item)
        {
            if (!InventoryCheck()) return;
        
            if (_inventorySlots[_currentSelectSlot].InventorySlotCheck() && _inventorySlots[_currentSelectSlot].IsSeled)
            {
                _inventorySlots[_currentSelectSlot].SettingInventorySlot(item);
            
                return;
            }
            
            for (int i = 0; i < inventoryCount; i++)
            {
                if (_inventorySlots[i].InventorySlotCheck())
                {
                    _inventorySlots[i].SettingInventorySlot(item);
                    _inventorySlots[i].SetActive(false);
                
                    break;
                }
            }
        }
    
        public void TakeOutInventory(Vector3 dropPos)
        {
            if(_inventorySlots[_currentSelectSlot].InventorySlotCheck() || !_inventorySlots[_currentSelectSlot].IsSeled) return;
        
            _inventorySlots[_currentSelectSlot].SetActive(true);

            ResetSlotItem(dropPos);
        }
    
        public bool InventoryCheck()
        {
            foreach (var inventorySlot in _inventorySlots)
            {
                if(inventorySlot.InventorySlotCheck())
                    return true;
            }
        
            return false;
        }

        public void ResetSlotItem(Vector3 dropPos = default)
        {
            _inventorySlots[_currentSelectSlot].ResetInventorySolt(dropPos);
        }

        public bool IsSlotSelected()
        {
            return _inventorySlots[_currentSelectSlot].IsSeled;
        }

        public bool IsSelectedSlotEmpty()
        {
            return _inventorySlots[_currentSelectSlot].InventorySlotCheck();
        }
    
        public PickUpCommand GetInventorySlotObject()
        {
            return _inventorySlots[_currentSelectSlot].InventorySolt;
        }
        private void SetSeledSlot(bool isSeled)
        {
            _inventorySlots[_currentSelectSlot].SetSeled(isSeled);

            if (!_inventorySlots[_currentSelectSlot].InventorySlotCheck())
            {
                _inventorySlots[_currentSelectSlot].SetActive(isSeled);
            }
        }
    }
}
