using System;
using GameplayAbilitySystem.Attributes;
using GameplayAbilitySystem.GameplayAbilities;
using UnityEngine;

namespace GameplayAbilitySystem.SOs.GroupData
{
    /// <summary>
    /// Ability System Component 데이터를 그룹화하여 관리하는 ScriptableObject.
    /// 이 클래스는 캐릭터의 Attribute, AttributeProcessor, GameplayAbility 등을 초기화하고 구성하는 데 사용됩니다.
    /// </summary>
    [CreateAssetMenu(menuName = "Gameplay Ability System/DataGroup/AbilitySystemComponent", fileName = "GroupASC_")]
    public class GroupASC : PrefixedScriptableObject
    {
        /// <summary>
        /// 그룹화된 Attribute 데이터.
        /// </summary>
        public GroupAttribute attributes;

        /// <summary>
        /// 그룹화된 AttributeProcessor 데이터.
        /// </summary>
        public GroupAttributeProcessor attributeProcessors;

        /// <summary>
        /// 그룹화된 GameplayAbility 데이터.
        /// </summary>
        public GroupGameplayAbility gameplayAbilities;

        /// <summary>
        /// AbilitySystemComponent에 Attribute 데이터를 추가합니다.
        /// </summary>
        /// <param name="asc">초기화할 AbilitySystemComponent 인스턴스</param>
        public void AddAttribute(AbilitySystemComponent asc)
        {
            // 현재 Attribute 리스트를 초기화
            asc.attributes.Clear();

            // Attribute 데이터가 없는 경우 경고 메시지를 출력하고 반환
            if (attributes == null)
            {
                Debug.LogWarning(asc.name + " has no attributes.");
                return;
            }

            // Attribute 데이터를 AbilitySystemComponent에 추가
            foreach (var init in attributes.group)
            {
                asc.attributes.Add(new GameplayAttribute()
                {
                    attributeName = init.attributeName,
                    name = init.name,
                    baseValue = init.baseValue,
                });
            }
        }

        /// <summary>
        /// AbilitySystemComponent에 AttributeProcessor 데이터를 추가합니다.
        /// </summary>
        /// <param name="asc">초기화할 AbilitySystemComponent 인스턴스</param>
        public void AddAttributePorcessors(AbilitySystemComponent asc)
        {
            // 현재 AttributeProcessor 리스트를 초기화
            asc.attributeProcessors.Clear();

            // AttributeProcessor 데이터가 없는 경우 경고 메시지를 출력하고 반환
            if (attributeProcessors == null)
            {
                Debug.LogWarning(asc.name + " has no attribute processors.");
                return;
            }

            // AttributeProcessor 데이터를 AbilitySystemComponent에 추가
            foreach (var attributeProcessor in attributeProcessors.group)
            {
                // AttributeProcessor 타입을 확인하고 새 인스턴스를 생성
                Type processorType = attributeProcessor.GetType();
                GameplayAttributeProcessor newProcessor = Activator.CreateInstance(processorType) as GameplayAttributeProcessor;

                // JSON 직렬화를 이용해 기존 데이터를 복사
                JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(attributeProcessor), newProcessor);

                // 새로운 AttributeProcessor를 리스트에 추가
                asc.attributeProcessors.Add(newProcessor);
            }
        }

        /// <summary>
        /// AbilitySystemComponent에 GameplayAbility 데이터를 추가합니다.
        /// </summary>
        /// <param name="asc">초기화할 AbilitySystemComponent 인스턴스</param>
        public void GrantAbilities(AbilitySystemComponent asc)
        {
            // 현재 GameplayAbility 리스트를 초기화
            asc.grantedGameplayAbilities.Clear();

            // GameplayAbility 데이터가 없는 경우 경고 메시지를 출력하고 반환
            if (gameplayAbilities == null)
            {
                Debug.LogWarning(asc.name + " has no gameplay abilities.");
                return;
            }

            // GameplayAbility 데이터를 AbilitySystemComponent에 추가
            foreach (var gameplayAbilitySo in gameplayAbilities.group)
            {
                if (gameplayAbilitySo == null)
                {
                    Debug.LogWarning(asc.name + " has no gameplay abilities.");
                    continue;
                }

                // GameplayAbilitySO 타입 확인 후 AbilitySystemComponent에 추가
                if (gameplayAbilitySo is GameplayAbilitySO abilitySo)
                {
                    GameplayAbility ga = abilitySo.ga;
                    asc.GrantAbility(ga);
                }
                else
                {
                    Debug.LogError($"Invalid type in the initialGameplayAbilitiesSO list.");
                }
            }
        }

        /// <summary>
        /// ScriptableObject 활성화 시 호출됩니다. Attribute 데이터 유효성을 확인합니다.
        /// </summary>
        public void OnEnable()
        {
            if (attributes == null) Debug.LogError("NULL attributes in " + name);
        }

        /// <summary>
        /// Unity 에디터에서 값이 변경되었을 때 호출되는 메서드로, 기본 ScriptableObject 검증 로직을 수행합니다.
        /// </summary>
        public override void OnValidate()
        {
            base.OnValidate();
        }
    }
}
