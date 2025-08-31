using UnityEngine;

namespace KWJ.Code.SO
{
    [CreateAssetMenu(fileName = "MapObjectSO", menuName = "SO/MapObjectSO", order = 0)]
    public class MapPartObjectGroupSO : ScriptableObject
    {
        [Header("Wall")] public PickObjectGroupGradeSO WallObjectGroup;

        [Header("Floor")] public PickObjectGroupGradeSO FloorObjectGroup;

        [Header("Ceiling")] public PickObjectGroupGradeSO CeilingObjectGroup;
    }
}