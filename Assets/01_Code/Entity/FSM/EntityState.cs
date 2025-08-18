
namespace Code.Entities.FSM
{
    public class EntityState
    {
        public EntityStateType StateType { get; protected set; }
        
        protected Entity m_entity;
        protected int m_animationHash;

        public EntityState(Entity entity, EntityStateType stateType, int animationHash)
        {
            m_entity = entity;
            StateType = stateType;
            m_animationHash = animationHash;
        }
        
        public virtual void Enter()
        {
            
        }
        
        public virtual void Exit()
        {
            
        }
        
        public virtual void StateUpdate()
        {
            
        }

        protected virtual void Condition()
        {
            
        }
    }
}