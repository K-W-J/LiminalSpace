using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.AI;
using UnityEngine;
using Code.Random;

namespace Code.Map
{
    public class MapLoader : MonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private LayerMask mapLayer;
        [SerializeField] private Transform mapManager;
        
        [Header("NavBake")]
        
        [SerializeField] private Vector3 _navBoundSize;
        [SerializeField] private float _bakeDelay;
        
        private PickRandomObjectGrade _probability;
        
        private List<MapPartManager> _mapPartList = new List<MapPartManager>();
        private List<GameObject> _wallList = new List<GameObject>();
        
        private List<NavMeshBuildSource> _navSource = new List<NavMeshBuildSource>();
        private List<MeshCollider> _navCollider= new List<MeshCollider>();
        
        private List<Collider> _colliders = new List<Collider>();
        
        private NavMeshData _navMeshData;
        private Vector3 _currentTrm;
        
        private bool _isMapCreate;

        private void Awake()
        {
            if (null == _probability)
            {
                _probability = new PickRandomObjectGrade(SeedManager.Instance.CreateRandom("Map"));
            }
        }

        private void OnEnable()
        {
            StartCoroutine(BuildNavMesh());
        }

        private void OnDisable()
        {
            StopCoroutine(BuildNavMesh());
        }

        private void Update()
        {
            MapLoad();
        }
        
        private void MapLoad()
        {
            Collider[] allColldier = Physics.OverlapSphere(transform.position, radius, mapLayer, QueryTriggerInteraction.UseGlobal);
            Collider[] currentColliders = allColldier.Where(c => c.isTrigger).ToArray();
            _navCollider = allColldier.Where(c => !c.isTrigger).Cast<MeshCollider>().ToList();
            
            foreach (var overlap in currentColliders)
            {
                if (overlap.GetComponentInParent<MapPartManager>() is { } mapPart)
                {
                    if (!_mapPartList.Contains(mapPart))
                    {
                        //시드 값이 맵 파괴 등의 이유로 순서가 꼬여서 똑같은 맵의 Coroutine이 여러번 작동 될 수 있음
                        //순서 보장을 위해 먼저 리스트에 추가
                        _mapPartList.Add(mapPart);
                        
                        MapCreate(mapPart);
                    }
                    
                    mapPart.OverlapChecker.MapRander.gameObject.SetActive(true);
                }
            }
            
            MapUnload(currentColliders);
            
            _colliders = currentColliders.ToList();
        }

        private IEnumerator BuildNavMesh()
        {
            while (true)
            {
                
                yield return new WaitForSeconds(_bakeDelay);
                
                if(_currentTrm == transform.position) continue;

                if (_navMeshData == null)
                {
                    _navMeshData = new NavMeshData();
                    NavMesh.AddNavMeshData(_navMeshData);
                }
            
                var settings = NavMesh.GetSettingsByID(0);
                var bounds = new Bounds(transform.position, _navBoundSize);
                
                _currentTrm = transform.position;
                
                CollectNavSource();
            
                //네비 베이크에 사용할 리소스들을 모와 sources 리스트에 저장 (불필요한 콜라이더까지 탐색하여 렉 걸림)
                /*NavMeshBuilder.CollectSources(bounds, _navMeshLayer,
                    NavMeshCollectGeometry.PhysicsColliders, 0, new List<NavMeshBuildMarkup>(), sources);*/
            
                NavMeshBuilder.UpdateNavMeshDataAsync(_navMeshData, settings, _navSource, bounds);
                
                Debug.Log("BuildNavMesh");
            }
        }

        private void MapUnload(Collider[] colliders)
        {
            for (int i = 0; i < _colliders.Count; i++)
            {
                if (!colliders.Contains(_colliders[i]))
                {
                    if (_colliders[i].GetComponentInParent<MapOverlapChecker>() is { } mapPartManager)
                    {
                        mapPartManager.MapRander.gameObject.SetActive(false);
                    }
                }
            }
            
            _colliders.Clear();
        }
        
        private void MapCreate(MapPartManager mapPart)
        {
            mapPart.Initialize();
            mapPart.AfterInitialize();
            
            if (mapPart.MapPartSpawnPoints == null) return;
            
            foreach (var mapSpawnPoint in mapPart.MapPartSpawnPoints)
            {
                GameObject mapObject = Instantiate(_probability.PickRandomObject(mapSpawnPoint.MapPartGroupSO),
                    Vector3.zero, Quaternion.identity);
                
                MapPartManager mapPartManager = mapObject.GetComponentInChildren<MapPartManager>();
                
                //이전 맵 파츠 EndPoint와 현재 맵 파츠 StartPoint의 위치와 회전을 계산하여 두점을 겹치게 만듦
                mapObject.transform.rotation = mapSpawnPoint.transform.rotation
                                               * mapPartManager.MapPartStartPoint.rotation;

                mapObject.transform.position = mapSpawnPoint.transform.position
                                               - mapPartManager.MapPartStartPoint.position; 
                
                //무조건 위치 옮기고 나서 오버랩 체크
                if (mapPartManager.OverlapChecker.OverlapCheck())
                {
                    Destroy(mapObject);
                    CloseMapBoundaries(mapSpawnPoint.MapPartGroupSO.Wall, mapSpawnPoint.transform,
                        mapPart.OverlapChecker.MapRander.transform);
                    continue;
                }
                
                mapObject.transform.SetParent(mapManager);
                _colliders.Add(mapPartManager.OverlapChecker.MapRander.GetComponent<Collider>());
            }
        }
        
        private void CollectNavSource()
        {
            _navSource.Clear();
            
            if (_navCollider.Count <= 0) return;

            foreach (var mesh in _navCollider)
            {
                if(mesh == null) continue;
                
                var source = new NavMeshBuildSource
                {
                    shape = NavMeshBuildSourceShape.Mesh,
                    sourceObject = mesh.sharedMesh,
                    transform = mesh.transform.localToWorldMatrix,
                    area = 0
                };
                
                _navSource.Add(source);
            }
        }
        
        private void CloseMapBoundaries(GameObject walls, Transform mapPartEndPoint, Transform parent)
        {
            GameObject wall = Instantiate(walls,
                Vector3.zero, Quaternion.identity);

            wall.transform.rotation = mapPartEndPoint.rotation * Quaternion.Euler(0, 90, 0);
            wall.transform.position = mapPartEndPoint.position;
            wall.transform.SetParent(parent);
            _wallList.Add(wall);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
            Gizmos.color = Color.orange;
            Gizmos.DrawWireCube(transform.position, _navBoundSize);
        }
    }
}