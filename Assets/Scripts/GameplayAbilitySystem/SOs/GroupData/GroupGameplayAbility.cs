using System.Collections.Generic;
using UnityEngine;

namespace GameplayAbilitySystem.SOs.GroupData
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/DataGroup/GameplayAbilities", fileName = "GroupGA_")]
    public class GroupGameplayAbility : PrefixedScriptableObject
    {
        public List<GameplayAbilitySO> group = new List<GameplayAbilitySO>();
    }
}