using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using GameplayAbilitySystem.Attributes;
using GameplayAbilitySystem.Enums;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace GameplayAbilitySystem.GameplayAbilities
{
    /// <summary>
    /// GameplayAbility 클래스는 게임 내에서 사용되는 능력을 정의합니다.
    /// 예를 들어, 스킬, 주문, 패시브 효과, 상호작용 등 다양한 행동을 구현할 수 있습니다.
    /// </summary>
    [Serializable]
    public class GameplayAbility
    {
        /// <summary>
        /// 능력의 이름
        /// </summary>
        public string name;

        /// <summary>
        /// 능력의 쿨다운을 관리하는 GameplayEffect
        /// </summary>
        public GameplayEffect cooldown = null;

        /// <summary>
        /// 능력 사용에 필요한 비용을 정의하는 GameplayEffect
        /// </summary>
        public GameplayEffect cost = null;

        /// <summary>
        /// 능력이 적용하는 효과 목록
        /// </summary>
        public List<GameplayEffect> effects = new List<GameplayEffect>();

        /// <summary>
        /// ScriptableObject로 저장된 효과 목록
        /// </summary>
        public List<GameplayEffectSO> effectsSO = new List<GameplayEffectSO>();

        /// <summary>
        /// 능력에 연결된 태그들 (e.g., 취소 태그, 블록 태그 등)
        /// </summary>
        [SerializeReference] public AbilityTags abiltyTags = new AbilityTags();

        /// <summary>
        /// 능력의 소스, 타겟, 소유자 AbilitySystemComponent
        /// </summary>
        public AbilitySystemComponent source, target, owner;

        /// <summary>
        /// 능력과 연결된 Cue 태그 목록
        /// </summary>
        [SerializeReference] public List<GameplayTag> cuesTags = new List<GameplayTag>();

        /// <summary>
        /// 고유 식별자
        /// </summary>
        [ReadOnly] public string guid;

        /// <summary>
        /// 클래스 이름 (직렬화 및 디버깅용)
        /// </summary>
        [ReadOnly] public string className;

        /// <summary>
        /// 능력 레벨
        /// </summary>
        public float level;

        /// <summary>
        /// 능력이 활성화 상태인지 여부
        /// </summary>
        public bool isActive;

        /// <summary>
        /// 능력이 마지막으로 활성화된 시간
        /// </summary>
        private float timeActivated;

        /// <summary>
        /// 활성화 고유 식별자
        /// </summary>
        public string activationGUID;

        /// <summary>
        /// 새로운 능력 인스턴스를 생성합니다.
        /// </summary>
        /// <param name="Owner">능력을 소유한 AbilitySystemComponent</param>
        /// <returns>생성된 GameplayAbility 인스턴스</returns>
        public virtual GameplayAbility Instantiate(AbilitySystemComponent Owner)
        {
            this.owner = Owner;
            Type classType = this.GetType();
            GameplayAbility gaCopy = (GameplayAbility)Activator.CreateInstance(classType);
            gaCopy.guid = Guid.NewGuid().ToString();
            gaCopy.className = this.GetType().FullName;

            // 효과 복사
            gaCopy.effects = this.effects.Select(fx => fx.Instantiate()).ToList();

            // ScriptableObject 기반 효과 추가
            foreach (var effectSo in effectsSO)
            {
                gaCopy.effects.Add(effectSo.ge.Instantiate());
            }

            gaCopy.effectsSO.Clear();
            // 속성 복사
            gaCopy.name = this.name;
            gaCopy.abiltyTags = this.abiltyTags;
            gaCopy.level = this.level;
            gaCopy.isActive = this.isActive;
            gaCopy.cuesTags = this.cuesTags;
            // 쿨다운 효과 복사
            if (cooldown != null)
            {
                gaCopy.CreateCoolDownGE(this.cooldown.durationValue);
                gaCopy.cooldown.gameplayEffectTags = cooldown.gameplayEffectTags;
            }

            // 비용 효과 복사
            if (cost != null)
            {
                gaCopy.CreateCostGE(cost.modifiers, cost.durationType, cost.durationValue);
                gaCopy.cost.gameplayEffectTags = cost.gameplayEffectTags;
            }

            // 태그 초기화
            gaCopy.abiltyTags = abiltyTags;
            if (!abiltyTags.initialized)
            {
                gaCopy.abiltyTags.FillTags(gaCopy);
                gaCopy.abiltyTags.ClearStrings();
            }

            return gaCopy;
        }

        /// <summary>
        /// Additional network serialization for inherited classes
        /// </summary>
        public virtual void SerializeAdditionalData() { }
        /// <summary>
        /// Additional network serialization for inherited classes
        /// </summary>
        public virtual void DeserializeAdditionalData() { }
        
        /// <summary>
        /// 능력을 활성화하기 전 준비 작업을 수행합니다.
        /// </summary>
        public virtual void PreActivate(AbilitySystemComponent source, AbilitySystemComponent target,
            string activationGUID)
        {
            isActive = true;
            timeActivated = Time.time;

            this.source = source;
            this.target = target;
            source.OnGameplayAbilityPreActivated?.Invoke(this, activationGUID);
        }

        /// <summary>
        /// 능력을 활성화합니다. 비용 및 쿨다운을 적용하며 관련 태그를 처리합니다.
        /// </summary>
        public virtual void ActivateAbility(AbilitySystemComponent source, AbilitySystemComponent target,
            string activationGUID)
        {
            Debug.Log($"Activating Ability : {this.name}");
            // Apply Cooldown
            if (cooldown != null && cooldown.durationValue > 0)
            {
                source.ApplyGameplayEffect(source, source, cooldown, activationGUID);
            }

            //Apply cost
            if (cost != null && cost.modifiers.Count > 0)
            {
                source.ApplyGameplayEffect(source, source, cost, activationGUID);
            }

            // 능력 태그 처리
            foreach (var tag in abiltyTags.CancelAbilitiesWithTags)
            {
                foreach (var ga in source.grantedGameplayAbilities)
                {
                    if (ga.isActive && ga.abiltyTags.ActivationOwnedTags.Contains(tag))
                    {
                        ga.DeactivateAbility(activationGUID);
                    }
                }
            }
        }

        /// <summary>
        /// 능력을 활성화한 후 후속 작업을 처리합니다.
        /// </summary>
        public virtual void PostActivate(AbilitySystemComponent source, AbilitySystemComponent target,
            string activationGUID)
        {
            if (source.invokeEventsGA) source.OnGameplayAbilityActivated?.Invoke(this, activationGUID);
        }

        /// <summary>
        /// 능력 활성화 프로세스를 전체적으로 실행합니다.
        /// </summary>
        public virtual void CommitAbility(AbilitySystemComponent source, AbilitySystemComponent target,
            string activationGUID)
        {
            PreActivate(source, target, activationGUID);
            ActivateAbility(source, target, activationGUID);
            PostActivate(source, target, activationGUID);
        }

        /// <summary>
        /// 능력을 비활성화합니다.
        /// </summary>
        public virtual void DeactivateAbility(string activationGUID = null)
        {
            if (!isActive) return;
            isActive = false;
            if (source.invokeEventsGA) source.OnGameplayAbilityDeactivated?.Invoke(this, activationGUID);
        }

        /// <summary>
        /// 남은 쿨다운 시간을 반환합니다.
        /// </summary>
        public float GetCooldownRemainingTime()
        {
            if (this.cooldown == null) return 0;
            return Math.Clamp((timeActivated + cooldown.durationValue) - Time.time, 0, 100000f);
        }

        /// <summary>
        /// 쿨다운 효과를 생성합니다.
        /// </summary>
        public GameplayEffect CreateCoolDownGE(float durationValue, GameplayTag coolDownTag = null,
            string cooldownName = "CoolDown")
        {
            cooldown = new GameplayEffect()
            {
                durationType = EGameplayEffectDurationType.Duration,
                name = cooldownName + " " + name,
                durationValue = durationValue,
            };
            if (coolDownTag != null)
            {
                cooldown.gameplayEffectTags = new GameplayEffectTags()
                {
                    GrantedTags = new List<GameplayTag>() { coolDownTag }
                };
            }

            return cooldown;
        }

        /// <summary>
        /// 비용 효과를 생성합니다.
        /// </summary>
        public GameplayEffect CreateCostGE(List<Modifier> modifiers,
            EGameplayEffectDurationType durationType = EGameplayEffectDurationType.Instant,
            float duration = 0f, GameplayTag costTag = null, string costName = "Cost")
        {
            var createdCost = new GameplayEffect()
            {
                durationType = durationType,
                name = costName + " " + name,
                durationValue = duration,
                gameplayEffectTags = new GameplayEffectTags()
                {
                    GrantedTags = new List<GameplayTag>() { costTag }
                }
            };
            if (costTag != null)
            {
                createdCost.gameplayEffectTags = new GameplayEffectTags()
                {
                    GrantedTags = new List<GameplayTag>() { costTag }
                };
            }

            createdCost.modifiers = modifiers;
            cost = createdCost;
            return createdCost;
        }

        /// <summary>
        /// 능력이 활성화 가능한지 확인합니다.
        /// </summary>
        public virtual bool CanActivateAbility(AbilitySystemComponent src, AbilitySystemComponent target,
            string activationGUI, bool sendFailedEvent)
        {
            // Check ALREADY_ACTIVE
            if (this.isActive)
            {
                if (src.logging || target.logging) Debug.Log($"{this.name} is already active.");
                if (sendFailedEvent)
                    src.OnGameplayAbilityFailedActivation?.Invoke(this, activationGUID,
                        EActivationFailure.ALREADY_ACTIVE);
                return false;
            }

            // Check coolDown
            if (GetCooldownRemainingTime() > 0)
            {
                if (src.logging || target.logging)
                    Debug.Log($"{this.name} is on Cooldown. Time Remaining: {GetCooldownRemainingTime()}");
                if (sendFailedEvent)
                    src.OnGameplayAbilityFailedActivation?.Invoke(this, activationGUID, EActivationFailure.COOLDOWN);
                return false;
            }

            // Check Cost
            if (!CheckCost(src))
            {
                if (sendFailedEvent)
                    src.OnGameplayAbilityFailedActivation?.Invoke(this, activationGUID, EActivationFailure.COST);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 비용이 충족 가능한지 확인합니다.
        /// </summary>
        public virtual bool CheckCost(AbilitySystemComponent src)
        {
            if (this.cost == null)
                return true;
            List<AttributeName> costAttributes = new List<AttributeName>();
            this.cost.modifiers.ForEach(modifier => costAttributes.Add(modifier.attributeName));

            List<AttributeName> presentAttributes = new List<AttributeName>();
            src.attributes.ForEach(attribute => presentAttributes.Add(attribute.attributeName));

            foreach (var costAttribute in costAttributes)
            {
                if (presentAttributes.Contains(costAttribute) == false)
                {
                    if (src.logging || target.logging)
                        Debug.Log($"ASC presentAttributes DOES NOT contain costAttributeName: {costAttribute}");
                    return false;
                }
            }

            foreach (var costAttributeName in costAttributes)
            {
                GameplayAttribute presentAttribute = src.attributes
                    .Find((presentAttribute) => presentAttribute.attributeName == costAttributeName);
                Modifier costModifer =
                    this.cost.modifiers.Find((costAttribute) => costAttribute.attributeName == costAttributeName);

                if (presentAttribute.baseValue < -costModifer.GetValue())
                {
                    if (src.logging || target.logging)
                        Debug.Log(
                            $"CANT PAY GA COST - ASC {presentAttribute.attributeName} {presentAttribute.baseValue} cannot pay {costAttributeName} {costModifer.GetValue()}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 이 능력이 활성화되면 차단되는 태그 목록을 반환합니다.
        /// </summary>
        public List<GameplayTag> GetBlockedAbilitiesTags(AbilitySystemComponent src)
        {
            var blockedAblityTags = new List<GameplayTag>();
            foreach (var ga in src.grantedGameplayAbilities)
            {
                if (ga.isActive)
                {
                    blockedAblityTags.AddRange(ga.abiltyTags.BlockAbilitiesWithTags);
                }
            }

            return blockedAblityTags;
        }
    }
}