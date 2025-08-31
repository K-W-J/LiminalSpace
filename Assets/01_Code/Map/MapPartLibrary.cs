using KWJ.Code.SO;
using UnityEngine;

namespace KWJ.Code.Map
{
    public class MapPartLibrary : MonoBehaviour
    {
        [field: SerializeField] public MapPartGroupSO MapPartGroupSO { get; private set; }

        public bool IsCreateComplete { get; set; }
    }
}