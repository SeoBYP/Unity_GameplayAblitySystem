using System;

namespace GameplayAbilitySystem.GameplayAbilities
{
    [Serializable]
    public class InstantAbility : GameplayAbility
    {
        public override void ActivateAbility(AbilitySystemComponent source, AbilitySystemComponent target, string activationGUID)
        {
            base.ActivateAbility(source, target, activationGUID);
            for (int i = 0; i < effects.Count; i++)
            {
                target.ApplyGameplayEffect(source, target, effects[i], base.activationGUID);
            }
            DeactivateAbility(activationGUID);
        }

        public override void DeactivateAbility(string activationGUID = null)
        {
            isActive = false;
        }
    }
}