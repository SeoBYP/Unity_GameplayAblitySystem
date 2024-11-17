using System;
using System.Numerics;
using GameplayAbilitySystem.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;
using Vector3 = UnityEngine.Vector3;

namespace GameplayAbilitySystem.GameplayAbilities
{
    public enum EProjectileMovement
    {
        Forward,
        MousePosition,
    }
    
    [Serializable]
    public class ProjectileAbility : GameplayAbility
    {
        public EProjectileMovement movementType = EProjectileMovement.MousePosition;
        public float projectileSpeed = 10f;
        public GameObject projectilePrefab = null;
        public GameObject projectile = null;
        public string projectileName = "";

        public override void SerializeAdditionalData()
        {
            base.SerializeAdditionalData();
            if (projectilePrefab != null) projectileName = projectilePrefab?.name;
        }

        public override void DeserializeAdditionalData()
        {
            base.DeserializeAdditionalData();
            projectilePrefab = Resources.Load<GameObject>(projectileName);
        }

        public override GameplayAbility Instantiate(AbilitySystemComponent Owner)
        {
            ProjectileAbility newInstance = base.Instantiate(Owner) as ProjectileAbility;
            newInstance.projectilePrefab = projectilePrefab;
            return newInstance;
        }

        public override void ActivateAbility(AbilitySystemComponent source, AbilitySystemComponent target, string activationGUID)
        {
            base.ActivateAbility(source, target, activationGUID);

            if (projectilePrefab == null)
                projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            else
                projectile = GameObject.Instantiate(projectilePrefab);
            
            projectile.name = "projectile";
            projectile.transform.position = source.transform.position;
            projectile.transform.rotation = source.transform.rotation;
            
            var rb = projectile.AddComponent<Rigidbody>();
            rb.drag = 0;
            rb.useGravity = false;
            rb.velocity = GetProjectileDirection();

            var projectileComponent = projectile.AddComponent<Projectile>();
            projectileComponent.OnHit += (hitAsc) => {
                // Debug.Log($"ProjectileAbility hitAsc.name {hitAsc.name}");
                effects.ForEach(ge => hitAsc.ApplyGameplayEffect(source, hitAsc, ge, activationGUID));
            };
            projectileComponent.source = source;
            
            base.DeactivateAbility(activationGUID);
        }

        public Vector3 GetProjectileDirection()
        {
            Vector3 projectileDirection = Vector3.zero;
            switch (movementType)
            {
                case EProjectileMovement.Forward:
                    projectileDirection = projectile.transform.forward * projectileSpeed;
                    break;
                case EProjectileMovement.MousePosition:
                    Vector3 mousePosition = MouseExtentions.GetMouseToWorldPosition();
                    projectileDirection = (mousePosition - projectile.transform.position).normalized;
                    projectileDirection *= projectileSpeed;
                    break;
            }

            projectileDirection.y = 0;
            return projectileDirection;
        }


    }
}