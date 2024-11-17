using System;
using GameplayAbilitySystem.Attributes;
using GameplayAbilitySystem.GameplayAbilities;
using UnityEngine;

namespace GameplayAbilitySystem.SOs.GroupData
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/DataGroup/AbilitySystemComponent", fileName = "GroupASC_")]
    public class GroupASC : PrefixedScriptableObject
    {
        public GroupAttribute attributes;
        public GroupAttributeProcessor attributeProcessors;
        public GroupGameplayAbility gameplayAbilities;

        public void AddAttribute(AbilitySystemComponent asc)
        {
            asc.attributes.Clear();

            if (attributes == null) { Debug.LogWarning(asc.name + " has no attributes."); return; }

            foreach (var init in attributes.group)
            {
                asc.attributes.Add(new GameplayAttribute()
                {
                    attributeName =  init.attributeName,
                    name = init.name,
                    baseValue = init.baseValue,
                });
            }
        }

        public void AddAttributePorcessors(AbilitySystemComponent asc)
        {
            asc.attributeProcessors.Clear();
            if (attributeProcessors == null)
            {
                Debug.LogWarning(asc.name + " has no attribute processors."); return;
            }

            foreach (var attributeProcessor in attributeProcessors.group)
            {
                Type processorType = attributeProcessor.GetType();
                GameplayAttributeProcessor newProcessor = Activator.CreateInstance(processorType) as GameplayAttributeProcessor;
                JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(attributeProcessor),newProcessor);
                asc.attributeProcessors.Add(newProcessor);
            }
        }

        public void GrantAbilities(AbilitySystemComponent asc)
        {
            asc.grantedGameplayAbilities.Clear();
            if (gameplayAbilities == null)
            {
                Debug.LogWarning(asc.name + " has no gameplay abilities.");
                return;
            }

            foreach (var gameplayAbilitySo in gameplayAbilities.group)
            {
                if (gameplayAbilitySo == null)
                {
                    Debug.LogWarning(asc.name + " has no gameplay abilities.");
                    continue;
                }

                if (gameplayAbilitySo is GameplayAbilitySO abilitySo)
                {
                    GameplayAbility ga = abilitySo.ga;
                    asc.GrantAbility(ga);
                }
                else {
                    Debug.LogError($"Invalid type in the initialGameplayAbilitiesSO list.");
                }
            }
        }
        
        public void OnEnable() {
            if (attributes == null) Debug.LogError("NULL attributes in " + name);
        }

        public override void OnValidate() {
            base.OnValidate();
        }
    }
}