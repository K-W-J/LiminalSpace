using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using UnityEngine;
using Code.Random;
using Code.SO;

namespace Code.Map

{
    public class MapPartManager : MonoBehaviour
    {
        [Header("ObjectSpawnPoints")]
        [SerializeField] private MapPartObjectGroupSO objectGroupSo;
        
        [field:Header("MapParts")]
        [field:SerializeField] public Transform MapPartSpawnPointGroup { get; private set; }
        public List<MapPartLibrary> MapPartSpawnPoints { get; private set; } = new List<MapPartLibrary>();
        [field:SerializeField] public Transform MapPartStartPoint { get; private set; }
        
        [field:Space]
        [field:SerializeField] public Transform ObjectSpawnPointGroup { get; private set; }
        private List<Transform> _objectSpawnPoints = new List<Transform>();
        
        [Header("EntitySpawnPoints")]
        [field:SerializeField] public Transform EntitySpawnPointGroup { get; private set; }
        
        public List<Transform> EntitySpawnPoints { get; private set; } = new List<Transform>();
        
        [field:Header("GetComponent")]
        [field: SerializeField] public MapOverlapChecker OverlapChecker { get; private set; }
        
        private PickRandomObjectGrade _probability;

        private NavMeshDataInstance _navMeshInstance;
        private NavMeshData _navMeshData;

        public void Initialize()
        {
            if (_probability == null)
            {
                _probability = new PickRandomObjectGrade(SeedManager.Instance.CreateRandom("MapPart"));
            }
            
            MapPartSpawnPoints = GetMapSpawnPointGroup(MapPartSpawnPointGroup);
            EntitySpawnPoints = GetPointGroup(EntitySpawnPointGroup);
            _objectSpawnPoints = GetPointGroup(ObjectSpawnPointGroup);
        }

        public void AfterInitialize()
        {
            CreateObjact();
        }
        
        private List<Transform> GetPointGroup(Transform pointGroup)
        {
            if (pointGroup == null) return null;
                
            List<Transform> children = pointGroup.GetComponentsInChildren<Transform>().ToList();
            
            if (children.Contains(pointGroup))
            {
                children.Remove(pointGroup);
            }
            
           return children;
        }
        
        private List<MapPartLibrary> GetMapSpawnPointGroup(Transform pointGroup)
        {
            if (pointGroup == null) return null;
            
            return pointGroup.GetComponentsInChildren<MapPartLibrary>().ToList();
        }
        
        private void CreateObjact()
        {
            if (ObjectSpawnPointGroup == null || objectGroupSo == null) return;
            
            for (int i = 0; i < _objectSpawnPoints.Count; i++)
            {
                GameObject currentObject = Instantiate(SpawnCheck(_objectSpawnPoints[i]), 
                    _objectSpawnPoints[i].position, Quaternion.identity);
                
                currentObject.transform.SetParent(OverlapChecker.MapRander.transform);
            
                currentObject.transform.rotation = _objectSpawnPoints[i].rotation * currentObject.transform.rotation;
                currentObject.transform.position = _objectSpawnPoints[i].position;
                
            }
        }

        private GameObject SpawnCheck(Transform objectSpawnPoint)
        {
            if (objectSpawnPoint.gameObject.layer == LayerMask.NameToLayer("SpawnWall"))
            {
                _probability.GroupGradeSetting(objectGroupSo.WallObjectGroup);
                return _probability.PickRandomObject(objectGroupSo.WallObjectGroup);
            }
            else if (objectSpawnPoint.gameObject.layer == LayerMask.NameToLayer("SpawnFloor"))
            {
                _probability.GroupGradeSetting(objectGroupSo.FloorObjectGroup);
                return _probability.PickRandomObject(objectGroupSo.FloorObjectGroup);
            }
            else if (objectSpawnPoint.gameObject.layer == LayerMask.NameToLayer("SpawnCeiling"))
            {
                _probability.GroupGradeSetting(objectGroupSo.CeilingObjectGroup);
                return _probability.PickRandomObject(objectGroupSo.CeilingObjectGroup);
            }
            else
                return null;

        }
    }
}