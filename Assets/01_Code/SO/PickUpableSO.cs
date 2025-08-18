using UnityEngine;
using Code.Enums;

namespace Code.SO
{
    public class PickUpableSO : MonoBehaviour
    {
        public ItemType ItemType;
        
        public int SellPrice;
        public int PurchasePrice;
        
        public Sprite ItemIcon;
        
        public string ItemName;
        
        [TextArea] public string ItemExplanation;
    }
}