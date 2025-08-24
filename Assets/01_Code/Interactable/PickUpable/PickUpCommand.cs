using Code.Entities;
using UnityEngine;
using Code.SO;

namespace Code.Interactable.PickUpable
{
    public class PickUpCommand : InteractCommand
    {
        [field:SerializeField] public PickUpableSO PickUpableSO { get; private set; }
        public override void Execute(Entity entity)
        {
            
        }
    }
}