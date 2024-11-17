using GameplayAbilitySystem.GameplayAbilities;
using GameplayAbilitySystem.GameplayEffects;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// GameplayCue가 적용될 때 필요한 데이터 구조를 정의하는 클래스
    /// </summary>
    public class GameplayCueApplicationData
    {
        /// <summary>
        /// Cue를 트리거한 GameplayAbility
        /// </summary>
        public GameplayAbility ga;

        /// <summary>
        /// Cue를 트리거한 GameplayEffect
        /// </summary>
        public GameplayEffect ge;

        /// <summary>
        /// Cue를 트리거한 주체(소스) AbilitySystemComponent
        /// </summary>
        public AbilitySystemComponent src;

        /// <summary>
        /// Cue가 적용된 대상 AbilitySystemComponent
        /// </summary>
        public AbilitySystemComponent tgt;

        /// <summary>
        /// Cue의 원본 이름(Ability 또는 Effect의 이름)
        /// </summary>
        public string originName;

        /// <summary>
        /// GameplayCueApplicationData 생성자
        /// </summary>
        /// <param name="ga">GameplayAbility 객체</param>
        /// <param name="ge">GameplayEffect 객체</param>
        /// <param name="src">소스 AbilitySystemComponent</param>
        /// <param name="tgt">대상 AbilitySystemComponent</param>
        public GameplayCueApplicationData(GameplayAbility ga, GameplayEffect ge, AbilitySystemComponent src,
            AbilitySystemComponent tgt)
        {
            this.ga = ga; // Cue를 트리거한 Ability
            this.ge = ge; // Cue를 트리거한 Effect
            this.src = src; // 소스 AbilitySystemComponent
            this.tgt = tgt; // 대상 AbilitySystemComponent
            // 원본 이름을 Ability 이름 또는 Effect 이름으로 설정
            originName = ga == null ? ge.name : ga.name;
        }

        /// <summary>
        /// 주어진 Ability 또는 Effect가 이 데이터의 원본인지 확인
        /// </summary>
        /// <param name="gaToCheck">확인할 GameplayAbility</param>
        /// <param name="geToCheck">확인할 GameplayEffect</param>
        /// <returns>원본이 일치하면 true, 아니면 false</returns>
        public bool IsOrigin(GameplayAbility gaToCheck, GameplayEffect geToCheck)
        {
            // 이 데이터의 Ability나 Effect가 주어진 값과 동일하면 true 반환
            if (gaToCheck == ga) return true;
            if (geToCheck == ge) return true;
            return false;
        }
    }
}
