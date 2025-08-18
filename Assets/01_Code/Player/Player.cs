using Unity.Cinemachine;
using UnityEngine;
using Settings.InputSystem;
using Code.Entities;
using Code.SO;

namespace Code.Players
{
    public class Player : Entity
    {
        [field:Header("PlayerSO")]
        [field: SerializeField] public PlayerInputSO PlayerInputSo { get; private set; }
        [field: SerializeField] public PlayerStatsSO PlayerStatsSo { get; private set; }
        
        [field:Header("Camera")]
        [field: SerializeField] public CinemachineCamera CinemaCamera { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            
            m_entityHealth.SetMaxHealth(PlayerStatsSo.MaxHealth);
        }
    }
}