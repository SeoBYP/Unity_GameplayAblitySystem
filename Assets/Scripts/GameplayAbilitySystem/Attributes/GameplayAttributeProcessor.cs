using System;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;
using Unity.Collections;
using UnityEngine;

namespace GameplayAbilitySystem.Attributes
{
    /// <summary>
    /// AttributeProcessor 클래스는 속성 처리 프로세스를 정의하는 기본 클래스입니다.
    /// </summary>
    [Serializable]
    public class GameplayAttributeProcessor
    {
        /// <summary>
        /// 프로세서 이름 (읽기 전용)
        /// </summary>
        [ReadOnly] public string name;

        /// <summary>
        /// 속성의 변경 전 처리 메서드 (필요에 따라 재정의)
        /// </summary>
        /// <param name="attribute">처리할 속성</param>
        /// <param name="ge">속성을 처리할 Effect</param>
        /// <param name="asc">처리될 속성의 Owner</param>
        public virtual void PreProcess(GameplayAttribute attribute, GameplayEffect ge, AbilitySystemComponent asc)
        {
        }

        /// <summary>
        /// 속성의 변경 후 처리 메서드 (필요에 따라 재정의)
        /// </summary>
        /// <param name="attribute">처리할 속성</param>
        /// <param name="oldValue">속성이 처리되기 전 값</param>
        /// <param name="newValue">속성이 처리된 후 값</param>
        /// <param name="asc">처리될 속성의 Owner</param>
        public virtual void PostProcessed(AttributeName name, float oldValue, float newValue, GameplayEffect ge) {
            // Debug.Log($"PostProcess: {name}, oldValue {oldValue} newValue {newValue} ge.name {ge.name}");
        }

    }

    /// <summary>
    /// Clamper 클래스는 특정 속성의 값을 최소/최대 범위로 제한하는 프로세서입니다.
    /// </summary>
    [Serializable]
    public class Clamper : GameplayAttributeProcessor
    {
        /// <summary>
        /// 속성의 최소값과 최대값
        /// </summary>
        public float min, max;
        /// <summary>
        /// 클램프할 대상 속성
        /// </summary>
        public AttributeName clampedAttributeName;

        /// <summary>
        /// 속성 변경 전 값이 최소/최대 범위를 벗어나는지 확인하여 제한하는 메서드
        /// </summary>
        /// <param name="attribute">처리할 속성</param>
        /// <param name="ge">속성을 처리할 Effect</param>
        /// <param name="asc">처리될 속성의 Owner</param>
        public override void PreProcess(GameplayAttribute attribute, GameplayEffect ge, AbilitySystemComponent asc)
        {
            //대상 속성이 clampedAttributeName과 일치할 때만 적용
            if (attribute.attributeName == clampedAttributeName)
            {
                //partialValue가 최소값보다 작으면 최소값으로, 최대값보다 크면 최대값으로 설정
                if (attribute.partialValue < min) attribute.partialValue = min;
                if (attribute.partialValue > max) attribute.partialValue = max;
            }
        }
    }

    /// <summary>
    /// ClamperMaxAttributeValue 클래스는 특정 속성을 다른 속성의 최대값으로 제한하는 프로세서입니다.
    /// </summary>
    [Serializable]
    public class ClamperMaxGameplayAttributeValue : GameplayAttributeProcessor
    {
        /// <summary>
        /// 최대값을 가져올 대상 속성과 클램프할 대상 속성
        /// </summary>
        public AttributeName max;
        public AttributeName clampedAttributeName;

        /// <summary>
        /// 최대값으로 사용할 속성
        /// </summary>
        [HideInInspector] public GameplayAttribute clamper = null;

        /// <summary>
        /// 속성 변경 전 값이 최대 속성을 넘지 않도록 제한하는 메서드
        /// </summary>
        /// <param name="attribute">처리할 속성</param>
        /// <param name="ge">속성을 처리할 Effect</param>
        /// <param name="asc">처리될 속성의 Owner</param>
        public override void PreProcess(GameplayAttribute attribute, GameplayEffect ge, AbilitySystemComponent asc)
        {
            // 대상 속성이 clampedAttributeName과 일치할 때만 적용
            if (attribute.attributeName == clampedAttributeName)
            {
                // clamper가 초기화되지 않았거나 null인 경우, 최대값 속성을 gameplayAttributesDictionary에서 가져옴
                if (clamper == null || clamper.attributeName == null)
                    asc.gameplayAttributesDictionary.TryGetValue(max.name, out clamper);
                // partialValue가 최대 속성값을 넘으면 최대 속성값으로 제한
                if (attribute.partialValue > clamper.GetValue()) attribute.partialValue = clamper.GetValue();
            }
        }
    }

    /// <summary>
    ///  ClamperMinAttributeValue 클래스는 특정 속성을 다른 속성의 최소값으로 제한하는 프로세서입니다.
    /// </summary>
    [Serializable]
    public class ClamperMinGameplayAttributeValue : GameplayAttributeProcessor
    {
        /// <summary>
        /// 최소값을 가져올 대상 속성과 클램프할 대상 속성
        /// </summary>
        public AttributeName min;
        public AttributeName clampedAttributeName;

        /// <summary>
        /// 최소값으로 사용할 속성
        /// </summary>
        [HideInInspector] public GameplayAttribute clamper = null;

        /// <summary>
        /// 속성 변경 전 값이 최소 속성보다 작지 않도록 제한하는 메서드
        /// </summary>
        /// <param name="attribute">처리할 속성</param>
        /// <param name="ge">속성을 처리할 Effect</param>
        /// <param name="asc">처리될 속성의 Owner</param>
        public override void PreProcess(GameplayAttribute attribute, GameplayEffect ge, AbilitySystemComponent asc)
        {
            // 대상 속성이 clampedAttributeName과 일치할 때만 적용
            if (attribute.attributeName == clampedAttributeName)
            {
                // clamper가 초기화되지 않았거나 null인 경우, 최소값 속성을 gameplayAttributesDictionary에서 가져옴
                if (clamper == null || clamper.attributeName == null)
                    asc.gameplayAttributesDictionary.TryGetValue(min.name, out clamper);
                // partialValue가 최소 속성값보다 작으면 최소 속성값으로 제한
                if (attribute.partialValue < clamper.GetValue()) attribute.partialValue = clamper.GetValue();
            }
        }
    }
}