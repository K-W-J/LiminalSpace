using System;
using UnityEngine;
using Code.Entities;
using Code.Interactable;

namespace Code.Players
{
    public class InteractableChecker : MonoBehaviour, IEntityComponent
    {
        public InteractCommand InteractCommand => _interactCommand;
        private InteractCommand _interactCommand;
        
        private Player _agent;
        private Camera _camera;
        
        public void Initialize(Entity entity)
        {
            _agent = entity as Player;
            _camera = Camera.main;
        }

        private void Update()
        {
            InteractableCheck();
        }

        private void InteractableCheck()
        {
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, _agent.PlayerStatsSo.InteractionRange, ~0, QueryTriggerInteraction.Collide))
            {
                if (hit.transform.TryGetComponent<InteractCommand>(out var interactable))
                {
                    _interactCommand = interactable;
                    return;
                }
            }

            _interactCommand = null;
        }

        private void OnDrawGizmos()
        {
            if (_camera == null) return;
                
            Gizmos.color = Color.red;
            
            Gizmos.DrawRay(_camera.transform.position,
                _camera.transform.forward * _agent.PlayerStatsSo.InteractionRange);
        }
    }
}
