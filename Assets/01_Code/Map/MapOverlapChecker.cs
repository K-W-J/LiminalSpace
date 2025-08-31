using UnityEngine;

namespace KWJ.Code.Map
{
    public class MapOverlapChecker : MonoBehaviour
    {
        [field:SerializeField] public GameObject MapRander { get; set; }
        [SerializeField] private GameObject colliderChecker;
        [SerializeField] private LayerMask isMapPart;
        private bool _isClipping;

        public bool OverlapCheck()
        {
            MapRander.SetActive(false);
            
            BoxCollider[] mapCollider = colliderChecker.GetComponents<BoxCollider>();
            
            if(mapCollider.Length == 0)
                Debug.LogError($"{gameObject}의 배열에 콜라이더가 없습니다.");
            
            foreach (BoxCollider boxCollider in mapCollider)
            {
                Vector3 center = colliderChecker.transform.TransformPoint(boxCollider.center);

                Vector3 size = boxCollider.size * 0.5f;
                    
                _isClipping = Physics.CheckBox(center, size, colliderChecker.transform.rotation, isMapPart);

                if (_isClipping == true)
                    break;
            }
            
            if(_isClipping == false) 
                MapRander.SetActive(true);
            
            return _isClipping;
        }   
        
        private void OnDrawGizmos()
        {
            if (colliderChecker == null) return;
            
            BoxCollider[] mapCollider = colliderChecker.GetComponents<BoxCollider>();
            
            Gizmos.color = Color.red;

            foreach (var boxCollider in mapCollider)
            {
                if (boxCollider == null) continue;

                Gizmos.matrix = boxCollider.transform.localToWorldMatrix;
                Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
            }
        }
    }
}