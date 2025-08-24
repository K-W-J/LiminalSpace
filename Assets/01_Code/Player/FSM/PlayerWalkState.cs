using Code.Entities;
using Code.Entities.FSM;

namespace Code.Players.FSM
{
    public class PlayerWalkState : PlayerState
    {
        private PlayerMovement _movement;
        private GroundChecker _groundChecker;
        private EntityAnimation _entityAnimation;
        private EntityStateMachine _entityStateMachine;
        public PlayerWalkState(Entity entity, EntityStateType stateType, int animationHash) : base(entity, stateType, animationHash)
        {
            _movement = entity.GetCompo<PlayerMovement>();
            _groundChecker = entity.GetCompo<GroundChecker>();
            _entityAnimation = entity.GetCompo<EntityAnimation>();
            _entityStateMachine = entity.GetCompo<EntityStateMachine>();
        }
        
        private void OnEnableEvent()
        {
            _entityAnimation.OnAnimationEnd += PlaySound;
        }

        private void PlaySound()
        {
            if(_entityStateMachine.CurrentState != EntityStateType.Walk) return;
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
            if(_movement.Velocity.magnitude != 0 && _movement.IsRuning)
                _entityStateMachine.ChangeState(EntityStateType.Run);
            else if(_movement.Velocity.magnitude == 0)
                _entityStateMachine.ChangeState(EntityStateType.Idle);
            else if (!_groundChecker.GroundCheck())
            {
                if(_movement.IsJumping)
                    _entityStateMachine.ChangeState(EntityStateType.Jump);
                else if(_movement.IsFalling)
                    _entityStateMachine.ChangeState(EntityStateType.Fall);
            }
        }
    }
}