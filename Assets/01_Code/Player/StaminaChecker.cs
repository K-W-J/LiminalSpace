using System.Collections;
using Code.Entities;
using UnityEngine;

namespace Code.Players
{
    public class StaminaChecker : MonoBehaviour, IEntityComponent
    {
        public bool CanRun { get; private set; } = true;
        
        public float MaxStamina;
        public float CurrentStamina;
        
        private Player _agent;
        private PlayerMovement _movement;
        
        private bool _isStop = true;
        
        public void Initialize(Entity entity)
        {
            _agent = entity as Player;

            _movement = entity.GetCompo<PlayerMovement>();
            
            InitializeStamina();
        }

        private void InitializeStamina()
        {
            MaxStamina = _agent.PlayerStatsSo.MaxStamina;
            CurrentStamina = MaxStamina;
        }

        private void Update()
        {
            StaminaCheck();
        }

        private void StaminaCheck()
        {
            if(_movement == null) return;
            
            if (_movement.IsRuning && _movement.Velocity.magnitude != 0)
            {
                if (CurrentStamina > 0)
                {
                    CurrentStamina -= 2 * Time.deltaTime;
                }
                else
                {
                    CanRun = false;
                }
            }
            else
            {
                if (MaxStamina > CurrentStamina)
                {
                    if (CurrentStamina <= 0)
                    {
                        StartCoroutine(StaminaCheckCoroutine());
                    }
                    else
                    {
                        CurrentStamina += 2 * Time.deltaTime;
                        CanRun = true;
                    }
                }
            }
        }

        private IEnumerator StaminaCheckCoroutine()
        {
            if (!_isStop) yield break;
            
            _isStop = false;
            
            yield return new WaitForSeconds(_agent.PlayerStatsSo.StaminaDaley);
            
            CurrentStamina += 5;
            CanRun = true;
            
            _isStop = true;
        }
    }
}