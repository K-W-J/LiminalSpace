using System;
using Code.Define;
using UnityEngine;

namespace Code.SO
{
    [CreateAssetMenu(fileName = "PickUpableSO", menuName = "SO/PickUpableSO", order = 0)]
    public class PickUpableSO : ScriptableObject
    {
        public int ItemID;
        
        public ItemCategory ItemCategory;
        
        public int MaximumStack;
        
        public Sprite ItemIcon;
        
        public string ItemName;
        
        [TextArea] public string ItemExplanation;

        //public int SellPrice;
        //public int PurchasePrice;
    }
}