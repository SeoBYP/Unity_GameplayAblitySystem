using System;
using System.Collections.Generic;
using System.Linq;
using GameplayAbilitySystem.SOs;
using GameplayAbilitySystem.Utils;
using UnityEngine;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// GameplayTag와 연결된 Cue 정보를 관리하는 클래스
    /// </summary>
    public class GameplayTagsWithCue
    {
        /// <summary>
        /// 태그와 연결된 Cue의 이름
        /// </summary>
        public string name = "";

        /// <summary>
        /// 연결된 GameplayTag
        /// </summary>
        public GameplayTag tag;

        /// <summary>
        /// 연결된 GameplayCue
        /// </summary>
        public GameplayCue cue;
    }

    /// <summary>
    /// Cue를 관리하는 라이브러리 싱글톤 ScriptableObject 클래스입니다.
    /// </summary>
    public class CuesLibrary : SingleTonScriptableObject<CuesLibrary>
    {
        /// <summary>
        /// Cue 라이브러리 리스트입니다.
        /// 각 태그와 연결된 Cue 정보를 저장합니다.
        /// </summary>
        public List<GameplayTagsWithCue> cuesLibrary = new List<GameplayTagsWithCue>();

        /// <summary>
        /// 주어진 태그에 해당하는 GameplayCue 리스트를 생성하여 반환합니다.
        /// </summary>
        /// <param name="tag">GameplayTag</param>
        /// <returns>생성된 GameplayCue 리스트</returns>
        public List<GameplayCue> CreateCues(GameplayTag tag)
        {
            // 태그에 연결된 원본 Cue들을 찾음
            List<GameplayCue> originalCues = cuesLibrary
                .Where(tagWithCue => tagWithCue.tag == tag)
                .Select(tagWithCue => tagWithCue.cue)
                .ToList();

            // 원본 Cue 리스트를 복사
            List<GameplayCue> copyCues = new List<GameplayCue>();
            foreach (GameplayCue cue in originalCues)
            {
                if (cue == null) return null; // Cue가 null이면 null 반환
                GameplayCue copy = new GameplayCue()
                {
                    prefab = cue.prefab, // Cue의 프리팹 복사
                    offset = cue.offset, // Cue의 위치 오프셋 복사
                    tag = tag // 태그 복사
                };
                copyCues.Add(copy);
            }


            return copyCues;
        }

        /// <summary>
        /// Unity 에디터에서 라이브러리가 변경될 때 태그 이름을 동기화
        /// </summary>
        private void OnValidate()
        {
            cuesLibrary.ForEach(cue => cue.name = cue.tag.name);
        }
    }
}