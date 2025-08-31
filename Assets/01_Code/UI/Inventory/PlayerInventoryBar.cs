using KWJ.Code.Interactable.PickUpable;
using KWJ.Code.Entities;
using UnityEngine;

namespace KWJ.Code.UI.Inventory
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
