using UnityEngine;
using UnityEngine.AI;

namespace Code.Entities
{
    //적의 모든 육체적 움직임 관리.
    public class EntityMovement : MonoBehaviour, IEntityComponent
    {
        public GameObject Target { get; set; }
        private GameObject _patrolPoint;
        
        [SerializeField] private NavMeshAgent navMeshAgent;
        //private Enemy.Enemy _agent;
        
        public void Initialize(Entity entity)
        {
            //_agent = entity as Enemy.Enemy;
        }

        private void Start()
        {
            MovementReset(false);
            MovementReset(true);
        }
        
        public void MovementReset(bool isReset)
        {
            if (navMeshAgent == null) return;

            navMeshAgent.enabled = isReset;
        }

        /*public void OnChase()
        {
            if(Target == null) return;
            
            navMeshAgent.speed = _agent.BaseEnemyStatsSo.ChaseSpeed;
            navMeshAgent.SetDestination(Target.transform.position);
        }
        
        public void OnPatrol()
        {
            if(_patrolPoint == null) return;
            
            navMeshAgent.speed = _agent.BaseEnemyStatsSo.PatrolSpeed;
            
            if(_patrolPoint != null)
                navMeshAgent.SetDestination(_patrolPoint.transform.position);
        }*/

        public void OnStop()
        {
            if(!navMeshAgent.isOnNavMesh) return;
            
            navMeshAgent.ResetPath();
        }

        public void OnWarp(Vector3 position)
        {
            navMeshAgent.Warp(position);
        }

        public void PickPatrolPoint()
        {
            //int random = Random.Range(0, MapManager.Instance.EntityPoint.Count);
            //_patrolPoint = MapManager.Instance.EntityPoint[random].gameObject;
            
            NavMeshPath path = new NavMeshPath();
            
            if (NavMesh.CalculatePath(transform.position, _patrolPoint.transform.position,
                    NavMesh.AllAreas, path) && path.status != NavMeshPathStatus.PathComplete) 
            //|| MapManager.EntityPoint[random] != null)
            {
                //foreach (var entityPoint in MapManager.Instance.EntityPoint)
                {
                    //_patrolPoint = entityPoint.gameObject;
                    
                    if (NavMesh.CalculatePath(transform.position, _patrolPoint.transform.position,
                            NavMesh.AllAreas, path))
                    {
                        if (path.status == NavMeshPathStatus.PathComplete)
                        {
                            //break;
                        }
                    }
                }
            }
        }

        public bool IsMoveing()
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                return false;
            }
            return true;
        }
        
        public bool IsVelocityZero() => navMeshAgent.velocity.magnitude <= 0;

    }
}