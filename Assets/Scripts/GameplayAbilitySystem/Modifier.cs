using System;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;
using Unity.Collections;
using UnityEngine;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// Modifier 클래스는 게임 내 속성(Attribute)에 변화를 주는 기본적인 수정자 클래스입니다.
    /// </summary>
    [Serializable]
    public class Modifier
    {
        /// <summary>
        /// Modifier의 이름 (클래스 이름으로 초기화됨)
        /// </summary>
        [SerializeField] [ReadOnly] public string name;
        /// <summary>
        /// Modifier가 영향을 줄 속성의 이름
        /// </summary>
        [SerializeReference] public AttributeName attributeName;
        /// <summary>
        /// 직렬화된 속성 이름으로, 저장 시 attributeName의 이름을 문자열로 저장합니다.
        /// </summary>
        [HideInInspector] public string attributeNameSerialized = "";

        /// <summary>
        ///  Modifier 생성자: name을 클래스 이름으로 초기화
        /// </summary>
        public Modifier()
        {
            name = GetType().Name;
        }

        /// <summary>
        /// Modifier가 적용할 값 계산을 위한 가상 메서드로, 자식 클래스에서 재정의됩니다.
        /// </summary>
        /// <param name="ge"></param>
        /// <returns></returns>
        public virtual float GetValue(GameplayEffect ge = null)
        {
            return 0;
        }
        
        
        /// <summary>
        /// attributeName을 직렬화된 문자열에 저장하여 이후 복원에 사용할 수 있도록 하는 함수입니다.
        /// </summary>
        public virtual void FillString()
        {
            attributeNameSerialized = attributeName.name;
        }

        /// <summary>
        /// attributeNameSerialized에 저장된 문자열을 바탕으로 AttributeName 인스턴스를 복원합니다.
        /// </summary>
        public virtual void FillModifier()
        {
            attributeName = AttributeNameLibrary.Instance.GetByName(attributeNameSerialized);
        }
    }

    /// <summary>
    /// BaseModifier 클래스는 Modifier를 상속받아 고정된 수치 값을 반환하는 기본 수정자 클래스입니다.
    /// </summary>
    [Serializable]
    public class BasicModifier : Modifier
    {
        /// <summary>
        /// 수정할 값 (고정된 값)
        /// </summary>
        public float value;

        /// <summary>
        /// 고정된 값을 반환하는 GetValue 메서드
        /// </summary>
        /// <param name="ge"></param>
        /// <returns></returns>
        public override float GetValue(GameplayEffect ge = null)
        {
            return value;
        }
    }

}