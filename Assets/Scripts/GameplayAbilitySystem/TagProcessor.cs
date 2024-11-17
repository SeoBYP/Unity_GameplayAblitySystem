using System;
using System.Collections.Generic;
using System.Linq;
using GameplayAbilitySystem.GameplayAbilities;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;

namespace GameplayAbilitySystem
{
    public static class TagProcessor
    {
        public static bool HasAnyTags(List<GameplayTag> tagsToCheck, List<GameplayTag> tagList)
        {
            foreach (var tagToCheck in tagsToCheck)
            {
                if (tagList.Contains(tagToCheck)) return true;
            }
            return false;
        }

        public static bool HasTag(GameplayTag tagToCheck, List<GameplayTag> tagList)
        {
            return tagList.Contains(tagToCheck);
        }
        
        public static bool CheckTagRequirements(AbilitySystemComponent asc, List<GameplayTag> currentTags, List<GameplayTag> requiredTags, List<GameplayTag> forbiddenTags) {
            return true;
        }

        public static bool CheckApplicationTagRequirementsGE(AbilitySystemComponent asc, GameplayEffect ge,
            List<GameplayTag> currentTags)
        {
            return CheckTagRequirements(asc, currentTags, ge.gameplayEffectTags.ApplicationTagRequirementsRequired,
                ge.gameplayEffectTags.ApplicationTagRequirementsForbidden);
        }

        public static void UpdateTags(AbilitySystemComponent source, AbilitySystemComponent target,
            ref List<GameplayTag> currentTags, List<GameplayEffect> appliedGameplayEffects,
            List<GameplayAbility> gameplayAbilities, Action<List<GameplayTag>,
                AbilitySystemComponent, AbilitySystemComponent, string> OnTagsChanged, string activationGUID)
        {
            var geTags = new List<GameplayTag>();
            foreach (var appliedGE in appliedGameplayEffects)
            {
                foreach (var tag in appliedGE.gameplayEffectTags.GrantedTags)
                {
                    geTags.Add(tag);
                }
            }
            
            var gaTags = new List<GameplayTag>();
            foreach (var ability in gameplayAbilities)
            {
                if (ability.isActive)
                {
                    foreach (var tag in ability.abiltyTags.ActivationOwnedTags)
                    {
                        gaTags.Add(tag);
                    }
                }
            }
            
            var newTags = new List<GameplayTag>();
            newTags.AddRange(geTags);
            newTags.AddRange(gaTags);
            
            if (!currentTags.SequenceEqual(newTags)) { //Must run BEFORE calling OnTagsAdded/OnTagsRemoved. Because they will use currentTags to calculate their tag diff. If currentTags doesnt update, then we'll run into a infinite loop with TriggerAbilities where applied GEs still dont have their tags on currentTags, and will be retriggered because the appliedGEs tags will be put into newTags, even tough its a different GE being applied.
                currentTags = new List<GameplayTag>(newTags);
                OnTagsChanged?.Invoke(currentTags, source, target, activationGUID);
            }
        }
    }
}