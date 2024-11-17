using System;
using GameplayAbilitySystem.Enums;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;
using Unity.VisualScripting;
using UnityEngine;

namespace GameplayAbilitySystem.Attributes
{
    /// <summary>
    /// 게임 내 특정 속성을 나타내는 클래스 (예: Health, Mana, Speed 등).
    /// 해당 속성은 기본값과 현재값, 그리고 수정자 값을 포함하며, 효과(GameplayEffect)에 의해 변경될 수 있습니다.
    /// </summary>
    public class GameplayAttribute
    {
        /// <summary>
        /// 속성의 이름 (예: "Health", "Mana" 등)
        /// </summary>
        public string name;

        /// <summary>
        /// 속성의 유형을 나타내는 열거형 (예: 스탯(Stat), 자원(Resource) 등)
        /// </summary>
        public AttributeName attributeName;

        /// <summary>
        /// 속성의 기본값 (초기 상태의 값)
        /// </summary>
        public float baseValue;

        /// <summary>
        /// 속성의 현재값 (효과 및 수정자가 적용된 이후의 값)
        /// </summary>
        public float currentValue;

        /// <summary>
        /// 속성에 적용된 수정값을 관리하는 객체
        /// </summary>
        public AttributeModifier modification = new();
        
        /// <summary>
        /// 속성이 변경된 이후 호출되는 이벤트.
        /// 속성 이름, 이전 값, 새로운 값, 적용된 효과를 전달합니다.
        /// </summary>
        public Action<AttributeName, float, float, GameplayEffect> OnPostAttributeChanged;

        /// <summary>
        /// 속성이 변경되기 전에 호출되는 이벤트.
        /// 변경될 속성 및 관련 효과 정보를 전달합니다.
        /// </summary>
        public Action<GameplayAttribute, GameplayEffect> OnPreAttributeChange;

        /// <summary>
        /// 속성의 이전값 (변경 전 값)
        /// </summary>
        private float oldValue;

        /// <summary>
        /// 속성 값의 부분적인 변화.
        /// (예: 현재 값에 누적된 변경값을 계산할 때 사용)
        /// </summary>
        public float partialValue;

        /// <summary>
        /// 전달받은 GameplayEffect의 수정자를 기반으로 속성 값을 변경합니다.
        /// </summary>
        /// <param name="gameplayEffect">속성 값 변경에 영향을 미치는 GameplayEffect</param>
        public void ApplyModifiers(GameplayEffect gameplayEffect)
        {
            oldValue = currentValue;

            // 현재 수정 값 계산
            partialValue = baseValue + modification.value;
            
            // 값이 변경되었을 경우, 사전 변경 이벤트 호출
            if (oldValue != partialValue)
                OnPreAttributeChange?.Invoke(this, gameplayEffect);

            // 변경된 값 저장
            currentValue = partialValue;

            // 변경 후 이벤트 호출
            if (oldValue != currentValue && attributeName.attributeType == EAttributeType.STAT)
            {
                OnPostAttributeChanged?.Invoke(attributeName, oldValue, currentValue, gameplayEffect);
            }
        }

        /// <summary>
        /// 전달받은 수정 값을 제거하여 속성을 원래 상태로 되돌립니다.
        /// </summary>
        /// <param name="modifierValue">제거할 수정 값</param>
        /// <param name="gameplayEffect">효과 정보를 포함하는 GameplayEffect</param>
        public void RemoveModifier(float modifierValue, GameplayEffect gameplayEffect)
        {
            oldValue = currentValue;

            // 수정 값을 제거
            modification.value -= modifierValue;

            // 값 재계산
            partialValue = baseValue + modification.value;

            // 사전 변경 이벤트 호출
            if (!Mathf.Approximately(oldValue, partialValue))
                OnPreAttributeChange?.Invoke(this, gameplayEffect);

            // 현재값 갱신
            currentValue = partialValue;

            // 변경 후 이벤트 호출
            if (!Mathf.Approximately(oldValue, currentValue) && attributeName.attributeType == EAttributeType.STAT)
            {
                OnPostAttributeChanged?.Invoke(attributeName, oldValue, currentValue, gameplayEffect);
            }
        }

        /// <summary>
        /// 전달받은 Modifier 값을 자원(Resource)에 적용합니다.
        /// </summary>
        /// <param name="modifier">적용할 Modifier 객체</param>
        /// <param name="ge">GameplayEffect 정보</param>
        public void ApplyModifierAsResource(Modifier modifier, GameplayEffect ge)
        {
            oldValue = baseValue;
            partialValue = baseValue;

            // 수정 값 적용
            partialValue += modifier.GetValue(ge);

            // 사전 변경 이벤트 호출
            if (!Mathf.Approximately(oldValue, partialValue))
                OnPreAttributeChange?.Invoke(this, ge);

            // 자원 값 업데이트
            baseValue = partialValue;

            // 변경 후 이벤트 호출
            if (!Mathf.Approximately(oldValue, baseValue) && attributeName.attributeType == EAttributeType.RESOURCE)
            {
                OnPostAttributeChanged?.Invoke(attributeName, oldValue, baseValue, ge);
            }
        }

        /// <summary>
        /// 현재 속성 값을 반환합니다.
        /// 스탯의 경우 currentValue, 자원의 경우 baseValue를 반환합니다.
        /// </summary>
        /// <returns>속성 값</returns>
        public float GetValue()
        {
            if (attributeName.attributeType == EAttributeType.STAT) return currentValue;
            else return baseValue;
        }
    }
}
