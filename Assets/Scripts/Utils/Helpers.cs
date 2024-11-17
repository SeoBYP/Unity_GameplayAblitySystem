using System.Collections.Generic;
using System.Linq;
using GameplayAbilitySystem.Attributes;
using GameplayAbilitySystem.Enums;
using GameplayAbilitySystem.GameplayEffects;
using UnityEngine.EventSystems;

namespace GameplayAbilitySystem.Utils
{
    /// <summary>
    /// Helpers 클래스는 다양한 보조 함수들을 제공하여, Modifier 및 AttributeProcessor 등을 쉽게 추가하고 관리할 수 있도록 도와줍니다.
    /// </summary>
    public class Helpers
    {
        /// <summary>
        /// 지정된 Modifier 타입의 Modifier를 생성하여, 해당 GameplayEffect의 Modifier 리스트에 추가하는 함수입니다.
        /// </summary>
        /// <param name="modifierType"></param>
        /// <param name="ge"></param>
        public static void AddModifier(EModifierType modifierType, GameplayEffect ge)
        {
            ge.modifiers.Add(CreateModifier(modifierType));
        }

        /// <summary>
        /// 기존 Modifier를 지정된 Modifier 타입의 새로운 Modifier로 교체하는 함수입니다.
        /// </summary>
        /// <param name="modifierType"></param>
        /// <param name="modifier"></param>
        public static void ChangeModifier(EModifierType modifierType, Modifier modifier)
        {
            modifier = CreateModifier(modifierType);
        }

        /// <summary>
        /// 지정된 Modifier 타입의 Modifier 인스턴스를 생성하는 함수
        /// </summary>
        /// <param name="modifierType"></param>
        /// <returns></returns>
        public static Modifier CreateModifier(EModifierType modifierType)
        {
            return modifierType switch
            {
                EModifierType.SimpleModifier => new BasicModifier(),
                _ => new BasicModifier(),
            };
        }

        /// <summary>
        /// 지정된 AttributeProcessor 타입의 AttributeProcessor 인스턴스를 생성하여 리스트에 추가하는 함수
        /// </summary>
        /// <param name="attributeProcessorType"></param>
        /// <param name="list"></param>
        public static void AddAttributeProcessor(EAttributeProcessorType attributeProcessorType,
            List<GameplayAttributeProcessor> list)
        {
            switch (attributeProcessorType)
            {
                // Clamper 타입 프로세서를 생성하고, 기본값을 설정하여 리스트에 추가
                case EAttributeProcessorType.Clamper:
                    list.Add(new Clamper(){min = 0,max = 1000,clampedAttributeName = null});
                    break;
                // ClamperMaxAttributeValue 타입 프로세서를 생성하고, 기본값을 설정하여 리스트에 추가
                case EAttributeProcessorType.ClamperMaxAttributeValue:
                    list.Add(new ClamperMaxGameplayAttributeValue() {max = null,clampedAttributeName = null});
                    break;
                // ClamperMinAttributeValue 타입 프로세서를 생성하고, 기본값을 설정하여 리스트에 추가
                case EAttributeProcessorType.ClamperMinAttributeValue:
                    list.Add(new ClamperMinGameplayAttributeValue(){min = null,clampedAttributeName = null});
                    break;
            }
        }

        /// <summary>
        /// 문자열 배열을 받아 리스트로 변환 후, 쉼표로 구분된 문자열로 반환하는 함수
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static string StringFromList(IEnumerable<string> stringArray)
        {
            return StringFromList(stringArray.ToList());
        }

        /// <summary>
        /// 문자열 리스트를 쉼표로 구분된 문자열 형식으로 반환하는 함수
        /// </summary>
        /// <param name="stringList"></param>
        /// <returns></returns>
        public static string StringFromList(List<string> stringList)
        {
            return $"[{string.Join(", ", stringList)}]";
        }
    }
}