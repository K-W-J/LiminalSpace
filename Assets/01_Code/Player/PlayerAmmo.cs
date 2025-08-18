using Code.Entities;
using UnityEngine;

namespace Code.Players
{
    public class PlayerAmmo : MonoBehaviour, IEntityComponent
    {
        public int AmmoCount { get; set; }
        public void Initialize(Entities.Entity entity)
        {
            
        }
        public void AddAmmo(int ammo) => AmmoCount += ammo;
        public int TakeAmmo() => AmmoCount;
    }
}