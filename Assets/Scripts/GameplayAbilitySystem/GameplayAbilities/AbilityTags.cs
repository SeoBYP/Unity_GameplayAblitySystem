using System.Collections.Generic;
using System.Linq;
using GameplayAbilitySystem.SOs;
using Unity.Collections;
using UnityEngine;

namespace GameplayAbilitySystem.GameplayAbilities
{
    /// <summary>
    /// Gameplay Abilities와 관련된 태그 관리 클래스입니다.
    /// </summary>
    public class AbilityTags
    {
        /// <summary>
        /// 이 능력이 활성화되거나 실행 중일 때 능력 소유자에게 부여되는 태그 목록입니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> ActivationOwnedTags = new List<GameplayTag>();
        /// <summary>
        /// 이 능력을 설명하는 태그 목록. 기능적인 역할 없이 단순히 설명 목적으로 사용되는 태그 목록입니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> DescriptionTags  = new List<GameplayTag>();
        
        /// <summary>
        /// 이 능력이 실행 중일 때, 현재 능력을 발동한 캐릭터 취소할 태그 목록입니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> CancelAbilitiesWithTags  = new List<GameplayTag>();
        
        /// <summary>
        /// 이 능력이 실행 중일 때, 현재 능력을 발동한 캐릭터에서 실행을 막을 태그 목록입니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> BlockAbilitiesWithTags  = new List<GameplayTag>();
        
        /// <summary>
        /// 능력을 활성화하기 위해서 소스 ASC에 반드시 있어야 하는 태그 목록입니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> SourceTagsRequired = new List<GameplayTag>();
        /// <summary>
        /// 능력을 활성화하기 위해서 소스 ASC에 없어야 할 태그 목록입니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> SourceTagsForbidden = new List<GameplayTag>();
        
        /// <summary>
        /// 능력을 활성화하려면 대상에 반드시 있어야 하는 태그 목록입니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> TargetTagsRequired = new List<GameplayTag>();
        /// <summary>
        /// 능력을 활성화하려면 대상에 반드시 없어야 하는 태그 목록입니다.
        /// </summary>
        [SerializeField] public List<GameplayTag> TargetTagsForbidden = new List<GameplayTag>();
        
        /// <summary>
        /// 초기화 상태 플래그
        /// </summary>
        [HideInInspector] public bool initialized = false;
        
        /// <summary>
        /// 문자열 기반 태그(네트워킹 및 직렬화 목적)
        /// </summary>
        [ReadOnly][HideInInspector] public List<string> stringActivationOwnedTags = new List<string>();
        [ReadOnly][HideInInspector] public List<string> stringDescriptionTags = new List<string>();
        [ReadOnly][HideInInspector] public List<string> stringCancelAbilitiesWithTags = new List<string>();
        [ReadOnly][HideInInspector] public List<string> stringBlockAbilitiesWithTags = new List<string>();
        [ReadOnly][HideInInspector] public List<string> stringSourceTagsRequired = new List<string>();
        [ReadOnly][HideInInspector] public List<string> stringSourceTagsForbidden = new List<string>();
        [ReadOnly][HideInInspector] public List<string> stringTargetTagsRequired = new List<string>();
        [ReadOnly][HideInInspector] public List<string> stringTargetTagsForbidden = new List<string>();
        [ReadOnly][HideInInspector] public List<string> string_CueTags = new List<string>();

        
        /// <summary>
        /// 저장된 문자열 리스트를 바탕으로 각 태그 리스트를 초기화합니다.
        /// </summary>
        public void FillTags(GameplayAbility ga)
        {
            initialized = true;
            ActivationOwnedTags = ActivationOwnedTags.Union(GameplayTagLibrary.Instance.GetByNames(stringActivationOwnedTags)).ToList();
            DescriptionTags = DescriptionTags.Union(GameplayTagLibrary.Instance.GetByNames(stringDescriptionTags)).ToList();
            CancelAbilitiesWithTags = CancelAbilitiesWithTags.Union(GameplayTagLibrary.Instance.GetByNames(stringCancelAbilitiesWithTags)).ToList();
            BlockAbilitiesWithTags = BlockAbilitiesWithTags.Union(GameplayTagLibrary.Instance.GetByNames(stringBlockAbilitiesWithTags)).ToList();

            SourceTagsRequired = SourceTagsRequired.Union(GameplayTagLibrary.Instance.GetByNames(stringSourceTagsRequired)).ToList();
            SourceTagsForbidden = SourceTagsForbidden.Union(GameplayTagLibrary.Instance.GetByNames(stringSourceTagsForbidden)).ToList();
            TargetTagsRequired = TargetTagsRequired.Union(GameplayTagLibrary.Instance.GetByNames(stringTargetTagsRequired)).ToList();
            TargetTagsForbidden = TargetTagsForbidden.Union(GameplayTagLibrary.Instance.GetByNames(stringTargetTagsForbidden)).ToList();

            ga.cuesTags = ga.cuesTags.Union(GameplayTagLibrary.Instance.GetByNames(string_CueTags)).ToList();
        }

        /// <summary>
        /// 현재 태그 리스트의 태그 이름을 문자열 리스트로 변환하여 저장합니다.
        /// </summary>
        public void FillStrings(GameplayAbility ga)
        {
            stringActivationOwnedTags = ActivationOwnedTags.Select(tag => tag.name).ToList();
            stringDescriptionTags = DescriptionTags.Select(tag => tag.name).ToList();
            stringCancelAbilitiesWithTags = CancelAbilitiesWithTags.Select(tag => tag.name).ToList();
            stringBlockAbilitiesWithTags = BlockAbilitiesWithTags.Select(tag => tag.name).ToList();
            stringSourceTagsRequired = SourceTagsRequired.Select(tag => tag.name).ToList();
            stringSourceTagsForbidden = SourceTagsForbidden.Select(tag => tag.name).ToList();
            stringTargetTagsRequired = TargetTagsRequired.Select(tag => tag.name).ToList();
            stringTargetTagsForbidden = TargetTagsForbidden.Select(tag => tag.name).ToList();

            string_CueTags = ga.cuesTags.Select(tag => tag.name).ToList();
        }
        
        /// <summary>
        /// 태그 리스트를 모두 초기화(비우기)합니다.
        /// </summary>
        public void ClearTags(GameplayAbility ga)
        {
            ActivationOwnedTags.Clear();
            DescriptionTags.Clear();
            CancelAbilitiesWithTags.Clear();
            BlockAbilitiesWithTags.Clear();
            SourceTagsRequired.Clear();
            SourceTagsForbidden.Clear();
            TargetTagsRequired.Clear();
            TargetTagsForbidden.Clear();

            ga.cuesTags.Clear();
        }

        /// <summary>
        /// 문자열 리스트를 모두 초기화(비우기)합니다.
        /// </summary>
        public void ClearStrings()
        {
            stringActivationOwnedTags.Clear();
            stringDescriptionTags.Clear();
            stringCancelAbilitiesWithTags.Clear();
            stringBlockAbilitiesWithTags.Clear();
            stringSourceTagsRequired.Clear();
            stringSourceTagsForbidden.Clear();
            stringTargetTagsRequired.Clear();
            stringTargetTagsForbidden.Clear();

            string_CueTags.Clear();
        }
    }
}