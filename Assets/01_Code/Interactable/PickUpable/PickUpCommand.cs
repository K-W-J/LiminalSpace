using UnityEngine;
using Code.SO;

namespace Code.Interactable.PickUpable
{
    public abstract class PickUpCommand : InteractCommand
    {
        [field:SerializeField] public PickUpableSO PickUpableSO { get; private set; }
    }
}