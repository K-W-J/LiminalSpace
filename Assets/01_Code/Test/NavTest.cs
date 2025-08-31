using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace KWJ.Code.Test
{
    public class NavTest : MonoBehaviour
    {
        private NavMeshData _navMeshData;
        private NavMeshSurface _navMeshSurface;
        private Vector3 _transform;

        private void Awake()
        {
            _navMeshSurface = GetComponent<NavMeshSurface>();
            //_navMeshSurface.BuildNavMesh();
            StartCoroutine(sdf());
        }
        
        private IEnumerator sdf()
        {
            while (true)
            {
                
                yield return new WaitForSeconds(3f);
                
                if(_transform == transform.position) continue;

                print(243223432432);
                
                if (_navMeshData == null)
                {
                    _navMeshData = new NavMeshData();
                    NavMesh.AddNavMeshData(_navMeshData);
                }
            
                var settings = NavMesh.GetSettingsByID(0);
                var sources = new List<NavMeshBuildSource>();
                var bounds = new Bounds(transform.position, Vector3.one * 250);
                _transform = transform.position;
            
                NavMeshBuilder.CollectSources(bounds, LayerMask.GetMask("Map"),
                    NavMeshCollectGeometry.PhysicsColliders, 0, new List<NavMeshBuildMarkup>(), sources);
            
                NavMeshBuilder.UpdateNavMeshDataAsync(_navMeshData, settings, sources, bounds);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, Vector3.one * 250);
        }
    }
}