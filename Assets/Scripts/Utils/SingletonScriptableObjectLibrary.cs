using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace GameplayAbilitySystem.Utils
{
    /// <summary>
    /// SingletonScriptableObjectLibrary<T,TS> 클래스는 특정 타입의 오브젝트(S)를 관리하는 싱글톤 라이브러리입니다.
    /// ScriptableObject로 구현되어 자주 참조되는 오브젝트들을 효율적으로 관리할 수 있도록 합니다.
    /// </summary>
    /// <typeparam name="T">인스턴스 타입입니다.</typeparam>
    /// <typeparam name="TS">관리할 오브젝트의 타입입니다.</typeparam>
    public class SingletonScriptableObjectLibrary<T,TS> : SingleTonScriptableObject<T> where T : ScriptableObject where TS : Object
    {
        /// <summary>
        /// 자동으로 TS 오브젝트를 갱신하는 지에 대한 여부
        /// </summary>
        public bool AutoRefresh = false;
        /// <summary>
        /// TS 오브젝트를 이름을 기준으로 Key값으로 Maping하기 위한 Dictionary입니다. 
        /// </summary>
        public Dictionary<string, TS> itemDictionary = new Dictionary<string, TS>();
        
        /// <summary>
        /// TS 오브젝트 리스트입니다.
        /// </summary>
        public List<TS> itemList = new List<TS>();
        
        /// <summary>
        /// TS 오브젝트를 가져올 폴더 경로입니다.
        /// </summary>
        public string folder = "";

        /// <summary>
        /// 싱글톤이 활성화될 때 호출되는 메서드로, AutoRefresh 설정에 따라 Refresh 시도합니다.
        /// </summary>
        public override void OnEnable()
        {
            base.OnEnable();
            TryRefresh();
        }

        /// <summary>
        /// 컨텍스트 메뉴에서 호출할 수 있는 메서드로, AutoRefresh가 true일 때만 Refresh를 실행합니다.
        /// </summary>
        [ContextMenu("Try Refresh")]
        public void TryRefresh()
        {
            if (AutoRefresh == false)
            {
                return;
            }

            Refresh();
        }

        /// <summary>
        /// Resources 폴더에서 지정된 타입(S)의 모든 오브젝트를 불러와 리스트와 딕셔너리를 갱신합니다.
        /// </summary>
        protected virtual void Refresh()
        {
            // 지정된 폴더에서 TS 타입의 오브젝트를 모두 로드합니다.
            UnityEngine.Object[] assets = Resources.LoadAll(folder, typeof(TS));
            // itemList를 새로 갱신하며 이름순으로 정렬합니다.
            itemList = assets.OfType<TS>().ToList();
            itemList = itemList.OrderBy(x => x.name).ToList();
            // itemDictionary를 새로 갱신하며 이름을 키로 설정합니다.
            itemDictionary = itemList.ToDictionary(x=>x.name, x=>x);
        }

        /// <summary>
        /// 아이템 이름으로 해당 오브젝트를 딕셔너리에서 찾는 메서드
        /// </summary>
        /// <param name="name">찾을 아이템의 이름입니다.</param>
        /// <returns></returns>
        public TS GetByName(string name)
        {
            // itemList와 itemDictionary의 요소 수가 맞지 않으면 딕셔너리를 갱신합니다.
            if(itemList.Count != itemDictionary.Count)
                itemDictionary = itemList.ToDictionary(x => x.name, x => x);

            // 이름이 일치하는 아이템이 있으면 반환, 없으면 오류 메시지 출력
            if (itemDictionary.TryGetValue(name, out var foundItem))
                return foundItem;
            
            Debug.LogError($"{typeof(T).Name} 라이브러리에서 '{name}'은 존재하지 않습니다.");
            return null;
        }
    }
}