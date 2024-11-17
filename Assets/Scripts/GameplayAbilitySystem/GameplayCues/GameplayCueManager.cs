using System.Collections.Generic;
using GameplayAbilitySystem.Enums;
using GameplayAbilitySystem.GameplayAbilities;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// GameplayCue를 관리하는 클래스.
    /// 능력 시스템 컴포넌트에 효과가 적용될 때 적절한 Cue를 트리거합니다.
    /// </summary>
    public static class GameplayCueManager
    {
        /// <summary>
        /// AbilitySystemComponent를 등록하여 효과 및 능력의 적용/제거 이벤트에 따라 Cue를 처리합니다.
        /// </summary>
        /// <param name="asc">등록할 AbilitySystemComponent</param>
        public static void Register(AbilitySystemComponent asc)
        {
            // GameplayEffect가 적용될 때 Cue를 처리
            asc.OnGameplayEffectApplied += (ge) =>
            {
                if (ge.cuesTags?.Count > 0)
                    OnApplyCue(ge.cuesTags, asc, ge.durationType == EGameplayEffectDurationType.Instant, null, ge);
            };
            // GameplayEffect가 제거될 때 Cue를 제거
            asc.OnGameplayEffectRemoved += (ge) =>
            {
                if (ge.cuesTags?.Count > 0)
                {
                    OnRemoveCue(ge.cuesTags, asc, null, ge);
                }
            };
            // GameplayAbility가 활성화될 때 Cue를 처리
            asc.OnGameplayAbilityActivated += (ga, activationGUID) =>
            {
                if (ga.cuesTags?.Count > 0)
                {
                    OnApplyCue(ga.cuesTags, asc, !ga.isActive, ga, null);
                }
            };
            // GameplayAbility가 비활성화될 때 Cue를 제거
            asc.OnGameplayAbilityDeactivated += (ga, activationGUID) =>
            {
                if (ga.cuesTags?.Count > 0)
                {
                    OnRemoveCue(ga.cuesTags, asc, ga, null);
                }
            };
        }
        /// <summary>
        /// 주어진 Cue 태그를 기반으로 Cue를 생성하고 적용합니다.
        /// </summary>
        /// <param name="cueTags">적용할 Cue 태그 목록</param>
        /// <param name="asc">AbilitySystemComponent</param>
        /// <param name="instantDestroy">즉시 제거 여부</param>
        /// <param name="ga">GameplayAbility 객체</param>
        /// <param name="ge">GameplayEffect 객체</param>
        static void OnApplyCue(List<GameplayTag> cueTags, AbilitySystemComponent asc, bool instantDestory,
            GameplayAbility ga, GameplayEffect ge)
        {
            foreach (GameplayTag cueTag in cueTags)
            {
                // Cue 태그에 해당하는 Cue 인스턴스를 생성
                List<GameplayCue> instancedCues = CuesLibrary.Instance.CreateCues(cueTag);
                foreach (var instancedCue in instancedCues)
                {
                    if(instancedCue == null) return;
                    // 생성된 Cue를 AbilitySystemComponent에 추가
                    instancedCue.AddCue(asc,instantDestory,new GameplayCueApplicationData(ga,ge,asc,null));
                }
            }   
        }

        /// <summary>
        /// 주어진 Cue 태그를 기반으로 Cue를 제거합니다.
        /// </summary>
        /// <param name="cueTags">제거할 Cue 태그 목록</param>
        /// <param name="asc">AbilitySystemComponent</param>
        /// <param name="ga">GameplayAbility 객체</param>
        /// <param name="ge">GameplayEffect 객체</param>
        static void OnRemoveCue(List<GameplayTag> cueTags, AbilitySystemComponent asc, GameplayAbility ga,
            GameplayEffect ge)
        {
            // ASC의 인스턴스화된 Cue 리스트에서 제거할 Cue를 찾음
            for (int i = 0; i < asc.instancedCues.Count; i++)
            {
                // 태그가 일치하고, 원본 데이터가 동일한 Cue를 제거
                if (cueTags.Contains(asc.instancedCues[i].tag) && asc.instancedCues[i].applicationData.IsOrigin(ga, ge))
                {
                    asc.instancedCues[i].RemoveCue(asc);
                }
            }
        }
        
    }
}