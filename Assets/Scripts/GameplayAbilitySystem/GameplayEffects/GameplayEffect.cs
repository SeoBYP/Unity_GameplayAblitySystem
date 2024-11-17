using System;
using System.Collections.Generic;
using GameplayAbilitySystem.Enums;
using GameplayAbilitySystem.SOs;
using UnityEditor;
using UnityEngine;

namespace GameplayAbilitySystem.GameplayEffects
{
    [Serializable]
    public class GameplayEffect
    {
        /// <summary>
        /// 효과 이름
        /// </summary>
        public string name;

        /// <summary>
        /// 효과 설명
        /// </summary>
        public string description;

        /// <summary>
        /// 효과 지속 시간 유형(즉시, 지속, 무한 등)
        /// </summary>
        public EGameplayEffectDurationType durationType;

        /// <summary>
        /// 효과 지속 시간(단위 : 초)
        /// </summary>
        public float durationValue = 0f;

        /// <summary>
        /// 효과가 반복되는 주기
        /// </summary>
        public float period = 0f;

        /// <summary>
        /// 효과 지속이 만료되었는지 여부를 나타내는 플래그
        /// </summary>
        public bool periodicExpired = false;

        /// <summary>
        /// 해당 효과가 적용할 속성 연산 리스트
        /// </summary>
        [SerializeReference] public List<Modifier> modifiers = new List<Modifier>();

        /// <summary>
        /// 효과를 제공하는 주체 
        /// </summary>
        public AbilitySystemComponent source;

        /// <summary>
        /// 효과를 받는 대상
        /// </summary>
        public AbilitySystemComponent target;

        /// <summary>
        /// 이 효과에 부여된 태그를 나타내는 객체
        /// </summary>
        public GameplayEffectTags gameplayEffectTags = new GameplayEffectTags();

        /// <summary>
        /// 효과가 발생할 때 재생되는 VFX, SFX 등의 목록들
        /// </summary>
        [SerializeReference] public List<GameplayTag> cuesTags = new List<GameplayTag>();

        /// <summary>
        /// 효과의 레벨
        /// </summary>
        public float level = 1f;

        /// <summary>
        /// 효과가 적용될 확률
        /// </summary>
        public float chanceToApply = 1f;

        /// <summary>
        /// 효과를 고유하게 식별할 GUID
        /// </summary>
        public string guid;

        /// <summary>
        /// 적용된 인스턴스에 대한 고유한 GUID
        /// </summary>
        public string applicationGUID;

        /// <summary>
        /// GameplayEffect의 새로운 인스턴스를 생성하여 반환합니다.
        /// 원본 오브젝트에 대한 참조를 끊고, 각 속성 값을 복사하여 새로운 객체를 만듭니다.
        /// </summary>
        /// <returns></returns>
        public GameplayEffect Instantiate()
        {
            GameplayEffect geCopy = (GameplayEffect)Activator.CreateInstance(this.GetType()); // 파생 클래스의 타입으로 인스턴스 생성
            geCopy.guid = Guid.NewGuid().ToString();
            geCopy.applicationGUID = applicationGUID;

            geCopy.name = name;
            geCopy.description = description;
            geCopy.durationType = durationType;
            geCopy.durationValue = durationValue;
            geCopy.period = period;
            geCopy.modifiers = modifiers;


            geCopy.cuesTags = cuesTags;
            geCopy.level = level;
            geCopy.chanceToApply = chanceToApply;

            geCopy.target = target;
            geCopy.source = source;

            // 태그를 새로운 인스턴스로 복사합니다.
            geCopy.gameplayEffectTags = gameplayEffectTags;
            if (!gameplayEffectTags.initialized)
            {
                geCopy.gameplayEffectTags.FillTags(geCopy); // 인스턴스화 후 ScriptableObjects를 채웁니다.
                geCopy.gameplayEffectTags.ClearStrings(); // 디자인 문제로 인해 문자열 초기화가 필요합니다.
            }

            return geCopy;
        }

        // 추가: 효과 처리를 위한 메서드 (필요시 오버라이드 가능)
        public virtual void ApplyEffect(AbilitySystemComponent source, AbilitySystemComponent target)
        {
            Debug.Log($"ApplyEffect: {this.name}");
        }

        public virtual void RemoveEffect(AbilitySystemComponent source, AbilitySystemComponent target)
        {
            if (target == null) return;

            foreach (var modifier in modifiers)
            {
                var attribute = target.attributes.Find(a => a.attributeName == modifier.attributeName);
                if (attribute != null)
                {
                    // 수정 값 제거
                    attribute.RemoveModifier(modifier.GetValue(this), this);
                    Debug.Log($"{target.name}: Removed modifier {modifier.GetValue(this)} from {modifier.attributeName.name}");
                }
            }
        }
    }
}