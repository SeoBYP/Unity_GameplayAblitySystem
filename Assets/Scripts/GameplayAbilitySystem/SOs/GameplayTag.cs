using UnityEngine;

namespace GameplayAbilitySystem.SOs
{
    /// <summary>
    /// GameplayTag 클래스는 게임 내 태그(Tag)를 정의하는 ScriptableObject입니다.
    /// 특정 상태나 카테고리를 나타내기 위해 사용됩니다.
    /// </summary>
    [CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Tag",fileName = "New Gameplay Tag")]
    public class GameplayTag : ScriptableObject
    {
        // 이 클래스는 현재 속성이나 메서드를 가지지 않지만,
        // 게임 내에서 태그를 구별하기 위한 기본 구조로 사용됩니다.
        // 예를 들어, "Fire", "Stunned", "Invincible"과 같은 태그를 정의할 수 있습니다
    }
}