using UnityEngine;
using KWJ.Code.Entities;

namespace KWJ.Code.Interactable
{
    public abstract class InteractCommand : MonoBehaviour
    {
        public abstract void Execute(Entity entity);
    }
}