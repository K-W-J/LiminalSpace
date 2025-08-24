using Code.Entities;
using Code.Entities.FSM;

namespace Code.Players.FSM
{
    public class PlayerFallState : PlayerState
    {
        private GroundChecker _groundChecker;
        private EntityAnimation _entityAnimation;
        private EntityStateMachine _entityStateMachine;
        public PlayerFallState(Entity entity, EntityStateType stateType, int animationHash) : base(entity, stateType, animationHash)
        {
            _groundChecker  = entity.GetCompo<GroundChecker>();
            _entityAnimation = entity.GetCompo<EntityAnimation>();
            _entityStateMachine = entity.GetCompo<EntityStateMachine>();
        }

        private void OnEnableEvent()
        {
            _entityAnimation.OnAnimationEnd += PlaySound;
        }

        private void PlaySound()
        {
            //SoundManager.Instance.PlaySFX("FootStep", m_entity.transform);
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
            
            PlaySound();
            _entityAnimation.SetBool(m_animationHash, false);
        }
        
        public override void StateUpdate()
        {
            Condition();
        }

        protected override void Condition()
        {
            if (_groundChecker.GroundCheck())
            {
                _entityStateMachine.ChangeState(EntityStateType.Idle);
            }
        }
    }
}