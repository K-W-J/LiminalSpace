using Code.Entities;
using Code.Interactable.PickUpable;
using UnityEngine;

namespace Code.UI.Inventory
{
    public class PlayerInventoryBar : RootUIPanel, IEntityComponent
    {
        [field: SerializeField] public InventoryManager InventoryManager { get; private set; }

        private int _currentSelectSlot;
        public void Initialize(Entity entity)
        {
            
        }
    }
}
