using UnityEngine;

namespace Code.Players
{
    public class PlayerBuildingHandler : MonoBehaviour
    {
        [SerializeField] private Player _agent;
        [SerializeField] private Transform _buildingPos;
        [SerializeField] private GameObject Item;
        
        /*
        private GameObject tempBuilding;
        private BuildObject tempBuilding2;
        
        private float _localZ;
        
        private bool isBuilding;
        
        private void OnEnable()
        {
            _agent.PlayerInputSo.ScrollAction += BuildingDistanceControl;
            _agent.PlayerInputSo.OnUseAction += CreateFromBuilding;
        }
        
        private void OnDisable()
        {
            _agent.PlayerInputSo.ScrollAction -= BuildingDistanceControl;
            _agent.PlayerInputSo.OnUseAction -= CreateFromBuilding;
        }

        private void Start()
        {
            StartBudilding();
        }

        public void StartBudilding()
        {
            tempBuilding = Instantiate(Item, _buildingPos);
            tempBuilding2 = tempBuilding.GetComponent<BuildObject>();
            tempBuilding.GetComponent<Collider>().enabled = false;
        }

        private void CreateFromBuilding(bool obj)
        {
            if(obj)
                Instantiate(Item, _buildingPos.position, _buildingPos.rotation);
        }

        private void BuildingDistanceControl(float scrollY)
        {
            /*_buildingPos.localPosition += new Vector3(0, 0, scrollY * 0.3f);
            _localZ = _buildingPos.localPosition.z;#1#
            _localZ += scrollY * 0.3f;
            _localZ = Mathf.Clamp(_localZ, 0, 5);
        }

        private void Update()
        {
            Debug.DrawRay(_agent.CinemaCamera.transform.position, 
                _agent.CinemaCamera.transform.forward * 5, Color.red, 0.01f);
            
            _buildingPos.rotation = Quaternion.Euler(0, _agent.CinemaCamera.transform.eulerAngles.y, 0);

            if (Physics.Raycast(_agent.CinemaCamera.transform.position, 
                    _agent.CinemaCamera.transform.forward, out RaycastHit hit, 5))
            {
                if(hit.transform == null) return;
                
                float distance = Vector3.Distance(hit.point, _buildingPos.transform.position);
                float cameraDistance = Vector3.Distance(_agent.CinemaCamera.transform.position, hit.point);
                
                if(distance < 1 && _localZ > cameraDistance && isBuilding == false)
                {
                    isBuilding = true;
                    _buildingPos.SetParent(null);
                    tempBuilding.transform.SetParent(null);

                    if (hit.transform.TryGetComponent<BuildObject>(out var buildObject))
                    {
                        float maxDistance = float.MaxValue;
                        
                        Vector3 buildObjectPosition = Vector3.zero;
                        Quaternion buildObjectRotation = Quaternion.identity;

                        for (int i = 0; i < buildObject.BuildPoints.Count; i++)
                        {
                            float pointDistance = Vector3.Distance(buildObject.BuildPoints[i].position, hit.point);
                            
                            if (maxDistance > pointDistance)
                            {
                                maxDistance = pointDistance;
                                buildObjectPosition = buildObject.BuildPoints[i].position;
                                buildObjectRotation = buildObject.BuildPoints[i].rotation;
                            }
                        }
                        
                        tempBuilding.transform.rotation = buildObjectRotation;
                        tempBuilding.transform.position += buildObjectPosition - tempBuilding2.BuildPoints[0].position;
                        _buildingPos.transform.position = tempBuilding.transform.position;
                    }
                    else
                    {
                        _buildingPos.transform.position = hit.point;
                    }
                }
                else
                {
                    isBuilding = false;
                    _buildingPos.SetParent(_agent.CinemaCamera.transform);
                    
                    tempBuilding.transform.SetParent(_buildingPos);
                    tempBuilding.transform.localPosition = Vector3.zero;
                    
                    _buildingPos.localPosition = new Vector3(0, 0, _localZ);
                }
            }
            else
            {
                isBuilding = false;
                _buildingPos.SetParent(_agent.CinemaCamera.transform);
                
                tempBuilding.transform.SetParent(_buildingPos);
                tempBuilding.transform.localPosition = Vector3.zero;
                
                _buildingPos.localPosition = new Vector3(0, 0, _localZ);
            }
            
            
        }
        */
    }
}