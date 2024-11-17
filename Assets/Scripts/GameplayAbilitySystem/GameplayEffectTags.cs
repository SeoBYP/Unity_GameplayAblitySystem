using System;
using System.Collections.Generic;
using System.Linq;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;
using UnityEngine;

namespace GameplayAbilitySystem
{
    [Serializable]
    public class GameplayEffectTags
    {
        /// <summary>
        /// 이 효과가 적용된 대상 ASC에 추가로 부여되는 태그 목록입니다.
        /// 효과가 제거될 때 ASC에서도 해당 태그가 제거됩니다.
        /// 이 기능은 지속(Duration) 및 무한(Infinite) 효과에서만 작동합니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> GrantedTags = new List<GameplayTag>();
        /// <summary>
        /// 이 효과를 설명하는 태그 목록입니다.
        /// 태그 자체는 아무 기능도 수행하지 않으며, 단순히 효과를 설명하는 역할만 합니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> DescriptionTags = new List<GameplayTag>();
        
        /// <summary>
        /// 효과기 적용이 되면 이후에도 지속적으로 활성화 되기 위해서 필요한 태그 목록입니다.
        /// 일단 효과가 적용되면, 이 태그들은 효과가 계속 유지될지 여부를 결정합니다.
        /// 이 기능은 지속(Duration) 및 무한(Infinite) 효과에서만 작동합니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> OngoingTagRequirementsRequired = new List<GameplayTag>();
        /// <summary>
        /// 효과기 적용이 되면 이후에도 지속적으로 효과가 활성화될 수 없도록 금지하는 태그 목록입니다.
        /// 일단 효과가 적용되면, 이 태그들은 효과가 계속 유지될지 여부를 결정합니다.
        /// 이 기능은 지속(Duration) 및 무한(Infinite) 효과에서만 작동합니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> OngoingTagRequirementsForbidden = new List<GameplayTag>();
        
        /// <summary>
        ///효과가 적용되기 위한 필수 태그 목록입니다.
        /// => 이 태그들이 모두 존재하지 않으면 효과가 적용되지 않습니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> ApplicationTagRequirementsRequired = new List<GameplayTag>();
        /// <summary>
        /// 효과가 적용되지 않도록 하는 태그 목록입니다.
        /// => 이 태그 중 하나라도 존재하면 효과가 적용되지 않습니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> ApplicationTagRequirementsForbidden = new List<GameplayTag>();
        
        /// <summary>
        /// 이 효과가 적용될 때, 제거될 태그 목록입니다.
        /// => 현재 효과가 성공적으로 적용될 때, 대상의 Granted Tags에 이 태그 목록이 포함된 모든 GameplayEffect를 대상에서 제거합니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> RemoveGameplayEffectsWithTag = new List<GameplayTag>();
        
        // 네트워킹 및 직렬화를 위한 플래그 및 문자열 리스트
        [HideInInspector] public bool initialized = false;
        [HideInInspector] public List<string> string_GrantedTags = new List<string>();
        [HideInInspector] public List<string> string_DescriptionTags = new List<string>();

        [HideInInspector] public List<string> string_OngoingTagRequirementsRequired = new List<string>();
        [HideInInspector] public List<string> string_OngoingTagRequirementsForbidden = new List<string>();

        [HideInInspector] public List<string> string_ApplicationTagRequirementsRequired = new List<string>();
        [HideInInspector] public List<string> string_ApplicationTagRequirementsForbidden = new List<string>();

        [HideInInspector] public List<string> string_RemovalTagRequirementsRequired = new List<string>();
        [HideInInspector] public List<string> string_RemovalTagRequirementsForbidden = new List<string>();

        [HideInInspector] public List<string> string_RemoveGameplayEffectsWithTag = new List<string>();

        [HideInInspector] public List<string> string_CueTags = new List<string>();

        /// <summary>
        /// 저장된 문자열 리스트를 바탕으로 각 태그 리스트를 초기화합니다.
        /// </summary>
        public void FillTags(GameplayEffect ge)
        {
            initialized = true;
            GrantedTags = GrantedTags.Union(GameplayTagLibrary.Instance.GetByNames(string_GrantedTags)).ToList();
            DescriptionTags = DescriptionTags.Union(GameplayTagLibrary.Instance.GetByNames(string_DescriptionTags)).ToList();
            OngoingTagRequirementsRequired = OngoingTagRequirementsRequired.Union(GameplayTagLibrary.Instance.GetByNames(string_OngoingTagRequirementsRequired)).ToList();
            OngoingTagRequirementsForbidden = OngoingTagRequirementsForbidden.Union(GameplayTagLibrary.Instance.GetByNames(string_OngoingTagRequirementsForbidden)).ToList();
            ApplicationTagRequirementsRequired = ApplicationTagRequirementsRequired.Union(GameplayTagLibrary.Instance.GetByNames(string_ApplicationTagRequirementsRequired)).ToList();
            ApplicationTagRequirementsForbidden = ApplicationTagRequirementsForbidden.Union(GameplayTagLibrary.Instance.GetByNames(string_ApplicationTagRequirementsForbidden)).ToList();
            RemoveGameplayEffectsWithTag = RemoveGameplayEffectsWithTag
                .Union(GameplayTagLibrary.Instance.GetByNames(string_RemoveGameplayEffectsWithTag)).ToList();
            
            ge.cuesTags = ge.cuesTags.Union(GameplayTagLibrary.Instance.GetByNames(string_CueTags)).ToList();
        }

        /// <summary>
        /// 현재 태그 리스트의 태그 이름을 문자열 리스트로 변환하여 저장합니다.
        /// </summary>
        public void FillStrings(GameplayEffect ge)
        {
            string_GrantedTags = GrantedTags.Select(tag => tag.name).ToList();
            string_DescriptionTags = DescriptionTags.Select(tag => tag.name).ToList();
            string_OngoingTagRequirementsRequired = OngoingTagRequirementsRequired.Select(tag => tag.name).ToList();
            string_OngoingTagRequirementsForbidden = OngoingTagRequirementsForbidden.Select(tag => tag.name).ToList();
            string_ApplicationTagRequirementsRequired = ApplicationTagRequirementsRequired.Select(tag => tag.name).ToList();
            string_ApplicationTagRequirementsForbidden = ApplicationTagRequirementsForbidden.Select(tag => tag.name).ToList();
            string_RemoveGameplayEffectsWithTag = RemoveGameplayEffectsWithTag.Select(tag => tag.name).ToList();
            
            string_CueTags = ge.cuesTags.Select(tag => tag.name).ToList();
        }

        /// <summary>
        /// 태그 리스트를 모두 초기화(비우기)합니다.
        /// </summary>
        public void ClearTags(GameplayEffect ge)
        {
            GrantedTags.Clear();
            DescriptionTags.Clear();
            OngoingTagRequirementsRequired.Clear();
            OngoingTagRequirementsForbidden.Clear();
            ApplicationTagRequirementsRequired.Clear();
            ApplicationTagRequirementsForbidden.Clear();
            RemoveGameplayEffectsWithTag.Clear();
            
            ge.cuesTags.Clear();
        }

        /// <summary>
        /// 문자열 리스트를 모두 초기화(비우기)합니다.
        /// </summary>
        public void ClearStrings()
        {
            string_GrantedTags.Clear();
            string_DescriptionTags.Clear();
            string_OngoingTagRequirementsRequired.Clear();
            string_OngoingTagRequirementsForbidden.Clear();
            string_ApplicationTagRequirementsRequired.Clear();
            string_ApplicationTagRequirementsForbidden.Clear();
            string_RemoveGameplayEffectsWithTag.Clear();
            
            string_CueTags.Clear();
        }
    }
}