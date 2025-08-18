using UnityEngine;
using Code.Entities;

namespace Code.Players
{
    public class GroundChecker : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private Vector3 _boxSize;
        
        public void Initialize(Entity entity)
        {
            
        }

        public bool GroundCheck()
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, _boxSize * 0.5f,
                Quaternion.identity, _whatIsGround, QueryTriggerInteraction.Ignore);
            
            return colliders.Length > 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, _boxSize);
            
        }
    }
}