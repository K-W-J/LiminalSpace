using UnityEngine;
using Code.UI.Inventory;
using Code.Entities;

namespace Code.Players
{
    public class PlayerInteract : MonoBehaviour, IEntityComponent
    {
        public bool IsInteracting { get; private set; }
        
        private Player _agent;
        private PlayerInventory _inventroy;
        private InteractableChecker _interactableChecker;
        
        private bool _isFirstClicking;
        private bool _isSecondClicking;
        
        public void Initialize(Entity entity)
        {
            _agent = entity as Player;
            
            _interactableChecker = entity.GetCompo<InteractableChecker>();
        }

        private void OnEnable()
        {
            _agent.PlayerInputSo.OnInteractAction += OnInteract;
        }
        private void OnDisable()
        {
            _agent.PlayerInputSo.OnInteractAction -= OnInteract;
        }
        
        private void Update()
        {
            Interact();
            
            if (_isFirstClicking && !_isSecondClicking)
                _isFirstClicking = false;
        }

        private void OnInteract(bool obj)
        {
            IsInteracting = obj;

            if (obj)
                _isFirstClicking = true;
        }
        

        private void Interact()
        {
            if (IsInteracting && _isFirstClicking)
            {
                if(_interactableChecker.InteractCommand == null) return;
                
                _interactableChecker.InteractCommand.Execute(_agent);
            }
        }
    }
}