using UnityEngine;
using Code.UI.Inventory;
using Code.Entities;

namespace Code.Players
{
    public class PlayerUI : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private GameObject EseObject;
        
        private Player _agent;
        private StaminaChecker _staminaChecker;
        private PlayerInventory _inventory;
        private PlayerMoney _money;
        private PlayerAmmo _ammo;
        
          
        public void Initialize(Entities.Entity entity)
        {
            _agent = entity as Player;
            
            _staminaChecker = entity.GetCompo<StaminaChecker>();
            _inventory = entity.GetCompo<PlayerInventory>();
            _money = entity.GetCompo<PlayerMoney>();
            _ammo = entity.GetCompo<PlayerAmmo>();
            
            //CursorActive(false);
        }

        private void OnEnable()
        {
            _agent.PlayerInputSo.OnInventroyNumberAction += OnInventory;

            _agent.PlayerInputSo.OnEsc += Ese;
        }

        private void OnDisable()
        {
            _agent.PlayerInputSo.OnInventroyNumberAction -= OnInventory;
            _agent.PlayerInputSo.OnEsc -= Ese;
        }

        private void Update()
        {
            /*_agent.HealthText.text = $"{_agent.EntityHealth.CurrentHealth.ToString()}" +
                                     $" / {_agent.EntityHealth.MaxHealth.ToString()}";
            
            _agent.StaminaText.text = $"{((int)_staminaChecker.CurrentStamina).ToString()}" +
                                      $" / {((int)_staminaChecker.MaxStamina).ToString()}";
            
            _agent.MoneyText.text = _money.MoneyCount + "$";

            
            if (_inventory.GetSoltItem() is Pistol pistol)
            {
                _agent.AmmoText.gameObject.SetActive(true);
                _agent.AmmoText.text = pistol.CurrentAmmo + "/" +_ammo.AmmoCount;
            }
            else
            {
                _agent.AmmoText.gameObject.SetActive(false);
            }
            

            if (_inventory.GetSoltItem() != null)
                _agent.ItemExplanationText.text = _inventory.GetSoltItem()
                    .PickUpableSo.ItemExplanation;
            else
                _agent.ItemExplanationText.text = "";*/
        }
        public void Ese()
        {
            if (!EseObject.gameObject.activeSelf)
            {
                EseObject.gameObject.SetActive(true);
                Time.timeScale = 0;
                Cursor.visible = true;
            }
            else
            {
                EseObject.gameObject.SetActive(false);
                Time.timeScale = 1;
                Cursor.visible = false;
            }
        }
        
        
        public void CursorActive(bool isActive)
        {
            Cursor.visible = isActive;
        }

        private void OnInventory(int cellNumder)
        {
            _inventory.SelectSolt(cellNumder);
        }
    }
}