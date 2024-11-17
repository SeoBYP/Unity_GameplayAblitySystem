namespace GameplayAbilitySystem.Enums
{
    /// <summary>
    /// 능력 활성화 실패 원인을 나타내는 열거형
    /// </summary>
    public enum EActivationFailure
    {
        /// <summary>
        /// 능력이 이미 활성화된 상태인 경우
        /// </summary>
        ALREADY_ACTIVE,

        /// <summary>
        /// 능력 사용에 필요한 비용을 충족하지 못한 경우
        /// </summary>
        COST,

        /// <summary>
        /// 능력이 쿨다운 중인 경우
        /// </summary>
        COOLDOWN,

        /// <summary>
        /// 출처(소스)의 태그 조건을 충족하지 못한 경우
        /// </summary>
        TAGS_SOURCE_FAILED,

        /// <summary>
        /// 대상의 태그 조건을 충족하지 못한 경우
        /// </summary>
        TAGS_TARGET_FAILED,

        /// <summary>
        /// 특정 태그에 의해 능력이 차단된 경우
        /// </summary>
        TAGS_BLOCKED,

        /// <summary>
        /// 그 외의 다른 이유로 실패한 경우
        /// </summary>
        OTHER,
    }
}