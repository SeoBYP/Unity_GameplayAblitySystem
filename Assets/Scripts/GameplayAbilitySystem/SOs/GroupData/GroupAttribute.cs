using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameplayAbilitySystem.SOs.GroupData
{
    [Serializable]
    public class AttributeInitialData
    {
        [ReadOnly] public string name;
        [SerializeReference] public AttributeName attributeName;
        public float baseValue;
    }
    
    [CreateAssetMenu(menuName = "Gameplay Ability System/DataGroup/Attributes", fileName = "GroupAttribute_")]
    public class GroupAttribute : PrefixedScriptableObject
    {
        public List<AttributeInitialData> group = new();
        
        public override void OnValidate()
        {
            base.OnValidate();
            
            group.ForEach(x => x.name = x.attributeName + ": " + x.baseValue);

            HashSet<string> uniqueNames = new HashSet<string>();
            foreach (var attributeInit in group)
            {
                if (!uniqueNames.Add(attributeInit.attributeName.name))
                {
                    Debug.LogWarning($"Duplicate attribute name detected: {attributeInit.attributeName.name}");
                }
            }
        }

        public void OnEnable()
        {
            group = group.OrderBy(attr => attr.attributeName.name).ToList();
            group = group.OrderBy(ga => ga.name).ToList();
        }
    }
}