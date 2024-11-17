using System;
using System.Collections.Generic;
using System.Linq;
using GameplayAbilitySystem.Utils;
using UnityEngine;

namespace GameplayAbilitySystem.SOs
{
    /// <summary>
    /// GameplayTags 클래스는 GameplayTagLibrary의 정적 참조를 저장하기 위한 유틸리티입니다.
    /// </summary>
    public static class GameplayTags
    {
        /// <summary>
        /// GameplayTagLibrary에 대한 정적 참조
        /// </summary>
        public static GameplayTagLibrary Library;
    }
    
    /// <summary>
    /// ameplayTagLibrary 클래스는 여러 GameplayTag 객체들을 관리하고 검색할 수 있는 싱글톤 라이브러리입니다.
    /// 다양한 태그를 리스트 및 딕셔너리 형태로 관리하며, 특정 태그를 이름 또는 인덱스로 검색할 수 있는 기능을 제공합니다.
    /// </summary>
    [CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Tag Library", fileName = "GameplayTagLibrary")]
    [Serializable]
    public class GameplayTagLibrary : SingletonScriptableObjectLibrary<GameplayTagLibrary,GameplayTag>
    {
        /// <summary>
        /// 태그 이름 리스트를 통해 해당하는 태그들을 검색하여 반환하는 메서드
        /// </summary>
        /// <param name="tagNames"></param>
        /// <returns></returns>
        public List<GameplayTag> GetByNames(List<string> tagNames)
        {
            // itemList에서 태그 이름이 tagNames 리스트에 포함된 태그들을 찾아 리스트로 반환
            List<GameplayTag> foundTag = itemList.Where(tag => tagNames.Contains(tag.name)).ToList();
            return foundTag;
        }

        /// <summary>
        /// 인덱스를 통해 해당 인덱스의 태그를 가져오는 메서드
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GameplayTag GetByIndex(int index)
        {
            return itemList[index];
        }

        /// <summary>
        /// ContextMenu를 통해 에디터에서 호출할 수 있으며, 정적 참조를 로그로 출력합니다.
        /// </summary>
        [ContextMenu("Log Static Reference")]
        public void LogStaticReference()
        {
            Debug.Log($"static ref : {GameplayTags.Library}");
        }

        /// <summary>
        /// 현재 객체를 JSON 문자열로 직렬화하여 로그에 출력하는 메서드
        /// </summary>
        /// <returns></returns>
        public bool SerializeString()
        {
            string s = JsonUtility.ToJson(this,true);
            Debug.Log(s);
            return true;
        }

        /// <summary>
        /// 특정 태그(child)가 다른 태그(parent)의 하위 태그인지 확인하는 메서드
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool IsParent(GameplayTag child, GameplayTag parent)
        {
            // child 태그 이름에 parent 태그 이름이 포함되어 있으면 true를 반환
            if (child.name.Contains(parent.name)) return true;
            return false;
        }
    }
}