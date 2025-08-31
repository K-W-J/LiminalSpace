using KWJ.Code.Entities.FSM;

namespace KWJ.Code.Players.FSM
{
    public class PlayerState : EntityState
    {
        protected Player m_entity;
        public PlayerState(Entities.Entity entity, EntityStateType stateType, int animationHash) : base(entity, stateType, animationHash)
        {
            m_entity = entity as Player;
        }
    }
}