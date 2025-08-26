using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Transform _target;
        private void FixedUpdate()
        {
            _agent.SetDestination(_target.position);
        }
    }
}
