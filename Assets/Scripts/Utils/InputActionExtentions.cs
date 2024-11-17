using System;
using UnityEngine.InputSystem;

namespace GameplayAbilitySystem.Utils
{
    public static class InputActionExtentions
    {
        public static void AddBindInputAction(this InputAction inputAction, Action<InputAction.CallbackContext> callback)
        {
            inputAction.started += callback;
            inputAction.canceled += callback;
            inputAction.performed += callback;
        }
        
        public static void RemoveBindInputAction(this InputAction inputAction, Action<InputAction.CallbackContext> callback)
        {
            inputAction.started -= callback;
            inputAction.canceled -= callback;
            inputAction.performed -= callback;
        }
    }
}