using System;
using System.Collections.Generic;
using System.Linq;
using GameplayAbilitySystem;
using GameplayAbilitySystem.SOs;
using GameplayAbilitySystem.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Character
{
    public class PlayerController : MonoBehaviour, IAbilitySystemInterface
    {
        private Rigidbody rb;
        public bool selfCastIfNoTarget = true;
        public List<AbilitySystemComponent> targets = new List<AbilitySystemComponent>();

        [SerializeField] private float moveSpeed = 5;
        [HideInInspector] public Vector2 movementInput;

        public AttributeName movementSpeed;

        [Header("Input Actions")] public InputAction moveAction;
        public InputAction dashAction;

        public InputAction actionOnAbility_1;
        public InputAction actionOnAbility_2;
        public InputAction actionOnAbility_3;
        public InputAction actionOnAbility_4;
        public InputAction actionOnAbility_5;

        private AbilitySystemComponent _abilitySystemComponent;

        public AbilitySystemComponent AbilitySystemComponent =>
            _abilitySystemComponent ??= GetComponent<AbilitySystemComponent>();

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            InitAttribute();
        }

        private void InitAttribute()
        {
            AbilitySystemComponent.OnAttributeChanged += (attributeName, oldValue, newValue, ge) =>
            {
                if (attributeName == movementSpeed)
                    moveSpeed = newValue;
            };
        }

        public AbilitySystemComponent GetAbilitySystemComponent()
        {
            return AbilitySystemComponent;
        }

        private void FixedUpdate()
        {
            Vector3 newVelocity = new Vector3(movementInput.x * moveSpeed, rb.velocity.y,
                movementInput.y * moveSpeed);
            rb.velocity = newVelocity;
        }

        private void OnMoveAction(InputAction.CallbackContext obj)
        {
            movementInput = obj.ReadValue<Vector2>();
        }

        private void OnDashAction(InputAction.CallbackContext obj)
        {
            if (obj.started)
            {
                AbilitySystemComponent.TryActivateAbility("Dash", AbilitySystemComponent);
            }
        }

        private void OnAbility_1_Action(InputAction.CallbackContext obj)
        {
            if (obj.started)
            {
                TryActivateAbilityCommand(1);
            }
        }

        private void OnAbility_2_Action(InputAction.CallbackContext obj)
        {
            if (obj.started)
            {
                TryActivateAbilityCommand(2);
            }
        }

        private void OnAbility_3_Action(InputAction.CallbackContext obj)
        {
            if (obj.started)
            {
                TryActivateAbilityCommand(3);
            }
        }

        private void OnAbility_4_Action(InputAction.CallbackContext obj)
        {
            if (obj.started)
            {
                TryActivateAbilityCommand(4);
            }
        }

        private void OnAbility_5_Action(InputAction.CallbackContext obj)
        {
            if (obj.started)
            {
                TryActivateAbilityCommand(5);
            }
        }

        public void TryActivateAbilityCommand(int i)
        {
            if (selfCastIfNoTarget && targets.Count == 0)
                targets.Add(AbilitySystemComponent);

            //If targeted projectile ability, just get all enemies and put them as targets...
            //Cast on server if using mirror component, else just call it normally
            foreach (var target in targets)
            {
                AbilitySystemComponent.TryActivateAbility(i, target);
            }

            if (targets.Contains(AbilitySystemComponent)) targets.Remove(AbilitySystemComponent);
        }

        private void EnableInputActions()
        {
            // InputAction 활성화
            moveAction.Enable();
            dashAction.Enable();

            actionOnAbility_1.Enable();
            actionOnAbility_2.Enable();
            actionOnAbility_3.Enable();
            actionOnAbility_4.Enable();
            actionOnAbility_5.Enable();
        }

        private void DisableInputActions()
        {
            // InputAction 비활성화
            moveAction.Disable();
            dashAction.Disable();

            actionOnAbility_1.Disable();
            actionOnAbility_2.Disable();
            actionOnAbility_3.Disable();
            actionOnAbility_4.Disable();
            actionOnAbility_5.Disable();
        }

        private void OnEnable()
        {
            EnableInputActions();
            moveAction.AddBindInputAction(OnMoveAction);
            dashAction.AddBindInputAction(OnDashAction);

            actionOnAbility_1.AddBindInputAction(OnAbility_1_Action);
            actionOnAbility_2.AddBindInputAction(OnAbility_2_Action);
            actionOnAbility_3.AddBindInputAction(OnAbility_3_Action);
            actionOnAbility_4.AddBindInputAction(OnAbility_4_Action);
            actionOnAbility_5.AddBindInputAction(OnAbility_5_Action);
        }

        private void OnDisable()
        {
            DisableInputActions();

            moveAction.RemoveBindInputAction(OnMoveAction);
            dashAction.RemoveBindInputAction(OnDashAction);

            actionOnAbility_1.RemoveBindInputAction(OnAbility_1_Action);
            actionOnAbility_2.RemoveBindInputAction(OnAbility_2_Action);
            actionOnAbility_3.RemoveBindInputAction(OnAbility_3_Action);
            actionOnAbility_4.RemoveBindInputAction(OnAbility_4_Action);
            actionOnAbility_5.RemoveBindInputAction(OnAbility_5_Action);
        }
    }
}