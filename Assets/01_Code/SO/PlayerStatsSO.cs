using UnityEngine;

namespace Code.SO
{
    [CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "SO/Player/PlayerStatsSO", order = 0)]
    public class PlayerStatsSO : ScriptableObject
    {
        public int MaxHealth;
        public int AttackPower;
        public int MaxHunger;
        
        [Space]
        
        public float WalkSpeed;
        public float RunSpeed;
        public int JumpPower;
        public int AttackSpeed;
        public int InteractionRange;
        public int MaxStamina;
        public int StaminaDaley;
        
        [Space]
        
        public int CameraRotationSmooth;
        public int CameraRotationZMaxClamp;
        public int CameraRotationZMinClamp;
        public float CameraRotationZSmoothSpeed;
    }
}