using System;
using GameplayAbilitySystem.GameplayAbilities;
using UnityEngine;

namespace GameplayAbilitySystem.SOs
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Ability/GameplayAbilitySO_InstantAbility", fileName = "GA_InstantAbility")]
    [Serializable]
    public class GameplayAbilitySO_InstantAbility : GameplayAbilitySO
    {
        public GameplayAbilitySO_InstantAbility() {
            ga = new InstantAbility();
        }
    }
}