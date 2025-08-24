using Code.Entities;
using Code.Entities.FSM;

namespace Code.Players.FSM
{
    public class PlayerIdleState : PlayerState
    {
        private PlayerMovement _movement; 
        private GroundChecker _groundChecker;
        private EntityAnimation _entityAnimation;
        private EntityStateMachine _entityStateMachine;
        public PlayerIdleState(Entity entity, EntityStateType stateType, int animationHash) : base(entity, stateType, animationHash)
        {
            _groundChecker = entity.GetCompo<GroundChecker>();
            _movement = entity.GetCompo<PlayerMovement>();
            _entityAnimation = entity.GetCompo<EntityAnimation>();
            _entityStateMachine = entity.GetCompo<EntityStateMachine>();
        }
        
        public override void Enter()
        {
            _entityAnimation.SetBool(m_animationHash, true);
        }
        
        public override void Exit()
        {
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
            else if(_movement.Velocity.magnitude != 0)
                _entityStateMachine.ChangeState(EntityStateType.Walk);
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