using System.Collections.Generic;
using GameplayAbilitySystem.Attributes;
using GameplayAbilitySystem.Enums;
using GameplayAbilitySystem.Utils;
using UnityEngine;

namespace GameplayAbilitySystem.SOs.GroupData
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/DataGroup/Attribute Processors", fileName = "GroupAttributeProcessor_")]
    public class GroupAttributeProcessor : PrefixedScriptableObject
    {
        [SerializeReference] public List<GameplayAttributeProcessor> group = new List<GameplayAttributeProcessor>();

  
        public override void OnValidate()
        {
            base.OnValidate();
            NameClampers(group);
        }

        public void NameClampers(List<GameplayAttributeProcessor> processors)
        {
            processors.ForEach(x =>
            {
                if(x is Clamper) x.name = $"{(x as Clamper).min} < {(x as Clamper).clampedAttributeName.name} < {(x as Clamper).max}";
                if(x is ClamperMaxGameplayAttributeValue) x.name =  $"{(x as ClamperMaxGameplayAttributeValue).clampedAttributeName.name} < {(x as ClamperMaxGameplayAttributeValue).max.name}";
                if (x is ClamperMinGameplayAttributeValue) x.name = $"{(x as ClamperMinGameplayAttributeValue).min.name} < {(x as ClamperMinGameplayAttributeValue).clampedAttributeName.name}";   
            });
        }
        
        [ContextMenu("ADD ATTRIBUTE PROCESSOR")]
        public void AddAttributeProcessor(EAttributeProcessorType processorType) {
            Helpers.AddAttributeProcessor(processorType, group);
        }
    }
}