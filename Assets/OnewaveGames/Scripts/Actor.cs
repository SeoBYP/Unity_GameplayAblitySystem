using System.Collections;
using System.Collections.Generic;
using GameplayAbilitySystem;
using UnityEngine;

public class Actor : MonoBehaviour ,IAbilitySystemInterface
{
    [SerializeField] private AbilitySystemComponent abilitysystem;
    public void ApplySkill(Actor target)
    {
        
    }

    public AbilitySystemComponent GetAbilitySystemComponent()
    {
        return abilitysystem;
    }
}
