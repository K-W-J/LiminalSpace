using UnityEngine;
using Code.Entities;

namespace Code.Interactable
{
    public abstract class InteractCommand : MonoBehaviour
    {
        public abstract void Execute(Entity entity);
    }
}