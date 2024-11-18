using System;
using GameplayAbilitySystem;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGUI
{
    public class HealthBar : MonoBehaviour
    {
        private AbilitySystemComponent _abilitySystemComponent;

        public AbilitySystemComponent AbilitySystemComponent =>
            _abilitySystemComponent ??= GetComponentInParent<AbilitySystemComponent>();

        public AttributeName _health;
        public AttributeName _maxHealth;

        private Slider _healthBar;

        public Slider HPBar => _healthBar ??= GetComponent<Slider>();

        private bool isInit = false;
        
        private void OnChangeHealth(AttributeName attributeName, float oldValue, float newValue, GameplayEffect ge)
        {
            if (attributeName == _health)
            {
                HPBar.value = newValue;
            }

            if (attributeName == _maxHealth)
            {
                HPBar.maxValue = newValue;
                if (!isInit)
                {
                    HPBar.value = newValue;
                    isInit = true;
                }
            }
        }
        
        public void OnEnable()
        {
            AbilitySystemComponent.OnAttributeChanged += OnChangeHealth;
        }

        private void OnDisable()
        {
            AbilitySystemComponent.OnAttributeChanged -= OnChangeHealth;
        }
    }
}