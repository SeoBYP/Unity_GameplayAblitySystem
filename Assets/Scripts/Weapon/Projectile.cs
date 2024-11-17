using System;
using GameplayAbilitySystem;
using GameplayAbilitySystem.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Weapon
{
    public class Projectile: MonoBehaviour
    {
        public float speed;
        public Action<AbilitySystemComponent> OnHit;
        public AbilitySystemComponent source;

        private void Start()
        {
            Destroy(this.gameObject,30f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponentAround<AbilitySystemComponent>() != null && other.gameObject.GetComponentAround<AbilitySystemComponent>() != source) {
                OnHit?.Invoke(other.gameObject.GetComponentAround<AbilitySystemComponent>());
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.GetComponentAround<AbilitySystemComponent>() != null && other.gameObject.GetComponentAround<AbilitySystemComponent>() != source) {
                OnHit?.Invoke(other.gameObject.GetComponentAround<AbilitySystemComponent>());
                Destroy(gameObject);
            }
        }
    }
}