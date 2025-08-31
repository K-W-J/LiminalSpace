using UnityEngine;
using KWJ.Code.Entities;
using KWJ.Code.SO;

namespace KWJ.Code.Interactable.PickUpable
{
    public class PickUpCommand : InteractCommand
    {
        [field:SerializeField] public PickUpableSO PickUpableSO { get; private set; }
        public override void Execute(Entity entity)
        {
            
        }
    }
}