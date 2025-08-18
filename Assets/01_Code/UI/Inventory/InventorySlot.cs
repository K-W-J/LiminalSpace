using System;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using Code.Interactable;
using Code.Interactable.PickUpable;

namespace Code.UI.Inventory
{
    [Serializable]
    public class InventorySlot : MonoBehaviour
    {
        [field: SerializeField] public Image SeledImage { get; private set; }
        
        [SerializeField] private Image image;

        public PickUpCommand InventorySolt { get; private set; }
        public bool IsSeled { get; set; }

        public bool InventorySlotCheck()
        {
            if (InventorySolt == null)
                return true;
            
            return false;
        }

        public void SettingInventorySlot(PickUpCommand cell)
        {
            InventorySolt = cell;
            image.sprite = InventorySolt.PickUpableSO.ItemIcon;
        }

        public void SetSeled(bool isSeled)
        {
            IsSeled = isSeled;
            SeledImage.gameObject.SetActive(isSeled);

            if (isSeled)
            {
                transform.DOScale(new Vector3(1.2f, 1.2f, 0f), 0.2f);
            }
            else
            {
                transform.DOScale(new Vector3(1f, 1f, 0f), 0.2f);
            }
        }
        
        public void SetActive(bool isActive) => InventorySolt.gameObject.SetActive(isActive);

        public void ResetInventorySolt(Vector3 dropPos = default)
        {
            InventorySolt = null;
            image.sprite = null;
        }
    }
}