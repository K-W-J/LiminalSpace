using Code.Entities;
using Code.Entities.FSM;

namespace Code.Players.FSM
{
    public class PlayerJumpState : PlayerState
    {
        private EntityAnimation _entityAnimation;
        private EntityStateMachine _entityStateMachine;
        private PlayerMovement _movement;
        public PlayerJumpState(Entity entity, EntityStateType stateType, int animationHash) : base(entity, stateType, animationHash)
        {
            _movement = entity.GetCompo<PlayerMovement>();
            _entityAnimation = entity.GetCompo<EntityAnimation>();
            _entityStateMachine = entity.GetCompo<EntityStateMachine>();
        }

        private void OnEnableEvent()
        {
            _entityAnimation.OnAnimationEnd += PlaySound;
        }

        private void PlaySound()
        {
            if(_entityStateMachine.CurrentState != EntityStateType.Run) return;
        }

        private void OnDisableEvent()
        {
            _entityAnimation.OnAnimationEnd -= PlaySound;
        }

        public override void Enter()
        {
            OnEnableEvent();
            _entityAnimation.SetBool(m_animationHash, true);
        }
        
        public override void Exit()
        {
            OnDisableEvent();
            _entityAnimation.SetBool(m_animationHash, false);
        }
        
        public override void StateUpdate()
        {
            Condition();
        }

        protected override void Condition()
        {
            if(_movement.IsFalling)
                _entityStateMachine.ChangeState(EntityStateType.Fall);
        }
    }
}