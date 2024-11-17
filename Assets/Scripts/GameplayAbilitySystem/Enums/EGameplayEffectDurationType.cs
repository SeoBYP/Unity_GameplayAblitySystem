using System;

namespace GameplayAbilitySystem.Enums
{
    /// <summary>
    /// GameplayEffect의 지속 시간 유형을 정의하는 열거형
    /// </summary>
    [Serializable]
    public enum EGameplayEffectDurationType
    {
        /// <summary>
        /// 즉시 효과가 적용되며, 지속 시간이 없는 타입
        /// </summary>
        Instant,

        /// <summary>
        /// 무한히 지속되는 효과
        /// </summary>
        Infinite,

        /// <summary>
        /// 특정 시간 동안 지속되는 효과
        /// </summary>
        Duration,
    }
}