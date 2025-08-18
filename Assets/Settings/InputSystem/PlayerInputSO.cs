using System;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Settings.InputSystem
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "InputAction", order = 0)]
    public class PlayerInputSO : ScriptableObject, InputSystem_Actions.IPlayerActions
    {
        private InputSystem_Actions _inputSystem;

        public Action<Vector2> OnMoveAction;
        public Action<Vector2> AtLootAction;
        public Action<float> ScrollAction;
        public Action<bool> OnInteractAction;
        public Action<bool> OnCrouchAction;
        public Action<bool> OnRunAction;
        public Action OnJumpAction;
        
        public Action<bool> OnSecondaryUseAction;
        public Action<bool> OnUseAction;
        public Action OnDropItemAction;
        public Action OnGunLoadAction;
        public Action OnOpenInventoryAction;
        
        public Action<int> OnInventroyNumberAction;

        public Action OnEsc;
        
        private void OnEnable()
        {
            if (_inputSystem == null)
            {
                _inputSystem = new InputSystem_Actions();
                _inputSystem.Player.SetCallbacks(this);
            }

            _inputSystem.Player.Enable();
        }

        private void OnDisable()
        {
            _inputSystem.Player.Disable();

        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector3 moveDir = context.ReadValue<Vector2>();
            OnMoveAction?.Invoke(moveDir);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Vector3 atLoot = context.ReadValue<Vector2>();
            AtLootAction?.Invoke(atLoot);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
                OnInteractAction?.Invoke(true);
            if (context.canceled)
                OnInteractAction?.Invoke(false);
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnCrouchAction?.Invoke(true);
            else if(context.canceled)
                OnCrouchAction?.Invoke(false);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnJumpAction?.Invoke();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnRunAction?.Invoke(true);
            else if(context.canceled)
                OnRunAction?.Invoke(false);
        }

        public void OnUseItem(InputAction.CallbackContext context)
        {
            if (context.started)
                OnUseAction?.Invoke(true);
            if (context.canceled)
                OnUseAction?.Invoke(false);
        }

        public void OnSecondaryUse(InputAction.CallbackContext context)
        {
            if (context.started)
                OnSecondaryUseAction?.Invoke(true);
            if (context.canceled)
                OnSecondaryUseAction?.Invoke(false);
        }

        public void OnDropItem(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnDropItemAction?.Invoke();
        }

        public void OnInventorySeled(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnInventroyNumberAction?.Invoke(GetInventoryNumber(context.control.name));
        }

        public void OnEse(InputAction.CallbackContext context)
        {
            if (context.started)
                OnEsc?.Invoke();
        }

        public void OnScroll(InputAction.CallbackContext context)
        {
            float scrollY = context.ReadValue<float>();
            ScrollAction?.Invoke(scrollY);
        }

        public void OnGunLoad(InputAction.CallbackContext context)
        {
            if (context.started)
                OnGunLoadAction?.Invoke();
        }

        public void OnOpenInventory(InputAction.CallbackContext context)
        {
            if (context.started)
                OnOpenInventoryAction?.Invoke();
        }

        private int GetInventoryNumber(string keyName)
        {
            if(keyName.Contains("numpad"))
                keyName = keyName.Replace("numpad", "");

            int number = int.Parse(keyName) - 1;
            
            if (number == -1)
                return 9;
            
            return number;
        }
    }
}