using System;
using GameplayAbilitySystem.GameplayEffects;
using UnityEngine;

namespace GameplayAbilitySystem.SOs.GameplayEffects
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Effect/Gameplay Effect Movement",fileName = "GE_")]
    [Serializable]
    public class GameplayEffectSO_Movement : GameplayEffectSO
    {
        public GameplayEffectSO_Movement()
        {
            ge = new GameplayEffect_KnockBackToPlayer();
        }
    }
}