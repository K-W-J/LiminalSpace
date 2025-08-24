using Code.Define;
using UnityEngine;
using Code.UI.Inventory;
using Code.UI;
using Code.Entities;

namespace Code.Players
{
    public class PlayerUIHandler : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private RootUI _rootUI;
        
        private Player _agent;
        private StaminaChecker _staminaChecker;
        private PlayerInventoryBar _inventoryBar;
          
        public void Initialize(Entity entity)
        {
            _agent = entity as Player;
            
            _staminaChecker = entity.GetCompo<StaminaChecker>();
            _inventoryBar = entity.GetCompo<PlayerInventoryBar>();
            
            //CursorActive(false);
        }

        private void OnEnable()
        {
            _agent.PlayerInputSo.OnInventroyNumberAction += OnInventory;
            _agent.PlayerInputSo.OnOpenInventoryAction += OnOpenInventory;
            _agent.PlayerInputSo.OnEsc += OnEse;
        }

        private void OnDisable()
        {
            _agent.PlayerInputSo.OnInventroyNumberAction -= OnInventory;
            _agent.PlayerInputSo.OnOpenInventoryAction -= OnOpenInventory;
            _agent.PlayerInputSo.OnEsc -= OnEse;
        }

        private void Update()
        {
            
        }
        private void OnEse()
        {
            if(_rootUI.GetPanel(PanelType.Pause) == null) return;
                
            if (!_rootUI.GetPanel(PanelType.Pause).gameObject.activeSelf)
            {
                _rootUI.GetPanel(PanelType.Pause).gameObject.SetActive(true);
                Time.timeScale = 0;
                CursorActive(true);
            }
            else
            {
                _rootUI.GetPanel(PanelType.Pause).gameObject.SetActive(false);
                Time.timeScale = 1;
                CursorActive(false);
            }
        }
        
        private void OnOpenInventory()
        {
            
        }
        
        public void CursorActive(bool isActive)
        {
            Cursor.visible = isActive;
        }

        private void OnInventory(int cellNumder)
        {
            //_inventoryHandler.SelectSolt(cellNumder);
        }
    }
}