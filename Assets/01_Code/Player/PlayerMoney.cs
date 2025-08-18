using Code.Entities;
using UnityEngine;

namespace Code.Players
{
    public class PlayerMoney : MonoBehaviour, IEntityComponent
    {
        public int MoneyCount { get; private set; }
        public void Initialize(Entities.Entity entity)
        {
            
        }
        
        public void AddMoney(int money)
        {
            MoneyCount += money;
        }
        
        public void DecreaseMoney(int money)
        {
            MoneyCount -= money;
        }

    }
}