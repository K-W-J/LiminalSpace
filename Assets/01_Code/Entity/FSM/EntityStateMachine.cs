using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Entities.FSM
{
    public class EntityStateMachine : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        public EntityStateType CurrentState => _currentEntityState.StateType;
        
        [SerializeField] private StateSO[] stateSo;
        
        [SerializeField] private StateSO initEntityState;
        
        private Dictionary<EntityStateType, EntityState> _states = new Dictionary<EntityStateType, EntityState>();
        
        private EntityState _currentEntityState;

        private Entity _entity;
        
        private bool _isStopState;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        public void AfterInitialize()
        {
            foreach (var state in stateSo)
            {
                Type type = Type.GetType(state.className);
                
                EntityState entityState = Activator.CreateInstance(type, _entity, state.stateType, state.animationHash) as EntityState;
                _states[state.stateType] = entityState;
            }
            
            _currentEntityState = GetState(initEntityState.stateType);

            _currentEntityState.Enter();
        }

        private void Update()
        {
            if (_currentEntityState != null)
                _currentEntityState.StateUpdate();
            else
                ChangeState(initEntityState.stateType);
        }

        public void ChangeState(EntityStateType entityState)
        {
            if(_isStopState || _currentEntityState == null) return;
            
            _currentEntityState.Exit();
            _currentEntityState = GetState(entityState);
            _currentEntityState.Enter();
        }
        
        public void DeathState()
        {
            _isStopState = true;
            ChangeState(initEntityState.stateType);
        }
        
        private EntityState GetState(EntityStateType stateType)
        {
            foreach (var state in _states)
            {
                if (state.Key == stateType)
                {
                    return state.Value;
                }
            }
            
            return null;
        }
    }
}