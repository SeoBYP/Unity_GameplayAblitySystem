using System;
using GameplayAbilitySystem.Utils;
using UnityEngine;

namespace GameplayAbilitySystem.SOs
{
    /// <summary>
    /// AttributeNameLibrary 클래스는 AttributeName 객체들을 관리하는 싱글톤 라이브러리입니다.
    /// ScriptableObject로 구현되어 다양한 속성(AttributeName)들을 효율적으로 참조 및 관리할 수 있습니다.
    /// </summary>
    [CreateAssetMenu(fileName = "AttributeNameLibrary", menuName = "Gameplay Ability System/AttributeNameLibrary")]
    [Serializable]
    public class AttributeNameLibrary : SingletonScriptableObjectLibrary<AttributeNameLibrary,AttributeName>
    { 
        // AttributeNameLibrary는 SingletonScriptableObjectLibrary<AttributeNameLibrary, AttributeName>을 상속받아,
        // AttributeName 객체를 저장하고 검색할 수 있는 기능을 제공하는 클래스입니다.
        // 이 클래스를 통해 AttributeName 객체들을 효율적으로 관리하고 접근할 수 있습니다.
    }
}