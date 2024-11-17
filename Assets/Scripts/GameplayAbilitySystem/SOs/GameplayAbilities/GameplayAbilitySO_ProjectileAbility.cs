using System;
using GameplayAbilitySystem.GameplayAbilities;
using UnityEngine;

namespace GameplayAbilitySystem.SOs
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Ability/GameplayAbilitySO_ProjectileAbility", fileName = "GA_ProjectileAbility")]
    [Serializable]
    public class GameplayAbilitySO_ProjectileAbility : GameplayAbilitySO
    {
        public GameplayAbilitySO_ProjectileAbility()
        {
            ga = new ProjectileAbility();
        }
    }
}