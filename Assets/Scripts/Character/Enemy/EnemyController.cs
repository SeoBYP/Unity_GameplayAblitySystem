using System;
using GameplayAbilitySystem;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        private Rigidbody rb;

        private AbilitySystemComponent _abilitySystemComponent;

        public AbilitySystemComponent AbilitySystemComponent =>
            _abilitySystemComponent ??= GetComponent<AbilitySystemComponent>();

        public AttributeName _health;
        public AttributeName _maxHealth;
        private void OnHealthChanged(AttributeName attributeName, float oldValue, float newValue, GameplayEffect ge)
        {
            if (attributeName == _health)
            {
                if (newValue <= 0)
                {
                    Destroy(gameObject,1);
                }
            }
        }
        
        private void OnEnable()
        {
            AbilitySystemComponent.OnAttributeChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            AbilitySystemComponent.OnAttributeChanged -= OnHealthChanged;
        }
    }
}