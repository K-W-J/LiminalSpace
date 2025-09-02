﻿using System.Collections.Generic;
using KWJ.Code.SO;
using UnityEngine;

namespace KWJ.Code.Random
{
    public enum GRADE
    {
        None = -1,
        
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        
        Max
    }
    
    public enum LEVEL_TYPE
    {
        None = -1,
        
        Factory,
        Tunnel, 
        Mine,
        Shop,
        Mart,
        School,
        
        Max
    }
    public class PickRandomObjectGrade : PickRandomObject
    {
        private int _commoneProbability;
        private int _uncommonProbability;
        private int _rareProbability;
        private int _epicProbability;
        private int _legendaryProbability;
        
        private List<PickObjectGroupGradeSO> _pickObjectSoList = new List<PickObjectGroupGradeSO>();

        public PickRandomObjectGrade(System.Random seed) : base(seed)
        {
            
        }

        public void GroupGradeSetting(PickObjectGroupGradeSO pickObjectGroupGradeSo)
        {
            if (!_pickObjectSoList.Contains(pickObjectGroupGradeSo))
                _pickObjectSoList.Add(pickObjectGroupGradeSo);
            
            PickObjectGroupGradeSO pickObjectSo = _pickObjectSoList
                .Find(x => x == pickObjectGroupGradeSo);
            
            _uncommonProbability = pickObjectSo.UncommonProbability;
            _rareProbability = pickObjectSo.RareProbability;
            _epicProbability = pickObjectSo.EpicProbability;
            _legendaryProbability = pickObjectSo.LegendaryProbability;
        }

        public GRADE PickRandomGrade(PickObjectGroupGradeSO mapPartGroupGradeSo)
        {
            int range = RandomRange(100);

            if (range < _legendaryProbability && mapPartGroupGradeSo.UseLegendary)
            {
                return GRADE.Legendary;
            }
            else if (range < _epicProbability && mapPartGroupGradeSo.UseEpic)
            {
                return GRADE.Epic;
            }
            else if (range < _rareProbability && mapPartGroupGradeSo.UseRare)
            {
                return GRADE.Rare;
            }
            else if (range < _uncommonProbability && mapPartGroupGradeSo.UseUncommon)
            {
                return GRADE.Uncommon;
            }
            else 
                return GRADE.Common;
        }

        public GameObject PickRandomObject(PickObjectGroupGradeSO mapPartGroupGradeSo)
        {
            GroupGradeSetting(mapPartGroupGradeSo);
            
            switch (PickRandomGrade(mapPartGroupGradeSo))
            {
                case GRADE.Common:
                {
                    return PickRandomObjectInGroup(mapPartGroupGradeSo.CommonObject);
                }
                case GRADE.Uncommon:
                {
                    return PickRandomObjectInGroup(mapPartGroupGradeSo.UncommonObject);
                }
                case GRADE.Rare:
                {
                    return PickRandomObjectInGroup(mapPartGroupGradeSo.RareObject);
                }
                case GRADE.Epic:
                {
                    return PickRandomObjectInGroup(mapPartGroupGradeSo.EpicObject);
                }
                case GRADE.Legendary:
                {
                    return PickRandomObjectInGroup(mapPartGroupGradeSo.LegendaryObject);
                }
                case GRADE.None:
                case GRADE.Max:
                {
                    Debug.LogWarning("ClickBefore 이거나 Max 이다.");
                    return null;
                }
                default:
                    return null;
            }
        }
    }
}