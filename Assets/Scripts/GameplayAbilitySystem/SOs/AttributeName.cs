using System;
using GameplayAbilitySystem.Enums;
using UnityEngine;

namespace GameplayAbilitySystem.SOs
{


    /// <summary>
    /// AttributeName 클래스는 속성의 이름과 유형을 저장하는 ScriptableObject로, 
    /// 다양한 속성을 정의하고 관리하는 데 사용됩니다.
    /// </summary>
    [CreateAssetMenu(fileName = "Attribute Name", menuName = "Gameplay Ability System/Attribute Name")]
    [Serializable]
    public class AttributeName : ScriptableObject
    {
        /// <summary>
        /// 속성의 유형을 나타내는 필드 (Stat 또는 Resource)
        /// </summary>
        public EAttributeType attributeType;
    }
}