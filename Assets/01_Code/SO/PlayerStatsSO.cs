using UnityEngine;

namespace KWJ.Code.SO
{
    [CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "SO/Player/PlayerStatsSO", order = 0)]
    public class PlayerStatsSO : ScriptableObject
    {
        public int MaxHealth;
        public int MaxHunger;
        
        [Space]
        
        public float WalkSpeed;
        public float RunSpeed;
        public int AttackSpeed;
        
        [Space]
        
        public int JumpPower;
        public int AttackPower;
        
        [Space]
        
        public int InteractionRange;
        
        [Space]
        
        public int MaxStamina;
        public int StaminaDaley;
    }
}