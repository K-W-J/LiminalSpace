using Code.SO;
using UnityEngine;

namespace Code.Map
{
    public class MapPartLibrary : MonoBehaviour
    {
        [field: SerializeField] public MapPartGroupSO MapPartGroupSO { get; private set; }

        public bool IsCreateComplete { get; set; }
    }
}