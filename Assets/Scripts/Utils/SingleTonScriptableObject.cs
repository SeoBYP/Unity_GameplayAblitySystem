using System.Linq;
using UnityEngine;

namespace GameplayAbilitySystem.Utils
{
    /// <summary>
    /// SingletonScriptableObject<T> 클래스는 ScriptableObject를 싱글톤으로 관리하기 위한 유틸리티 클래스입니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleTonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        /// <summary>
        /// Singleton 인스턴스를 저장할 정적 변수
        /// </summary>
        private static T _instance;

        /// <summary>
        /// Singleton 인스턴스를 가져오는 프로퍼티
        /// </summary>
        public static T Instance
        {
            get
            {
                // _instance가 null인 경우, Resources 폴더에서 T에 해당하는 
                if (_instance == null)
                {
                    // Resouces 폴더에서 모든 오브젝트를 가져옵니다. 
                    var objs = Resources.LoadAll("").ToList();
                    // 가져온 오브젝트들에서 T에 해당하는 값을 가져온다.
                    var obj = objs.FirstOrDefault(x => x.GetType() == typeof(T));

                    // 찾은 오브젝트를 _instance로 캐스팅합니다.
                    _instance = obj as T;

                    // 그럼에서 _instance가 없다면 Resources 폴더에 T에 해당하는 오브젝트가 없습니다.
                    if (_instance == null)
                    {
                        Debug.LogError($"SingletonScriptableObject<{typeof(T).Name}> 을 Resources 폴더에서 찾을 수 없습니다.");
                    }
                }
                // _instance를 반환합니다.
                return _instance;
            }
        }

        /// <summary>
        /// ScriptableObject가 활성화될 때 호출되는 메서드입니다.
        /// </summary>
        public virtual void OnEnable()
        {
            // _instance가 null인 경우, 현재 인스턴스를 _instance로 설정하고, 씬 전환 시 파괴되지 않도록 합니다.
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this);
            }
            else
            {
                // _instance가 이미 존재하는 경우, 현재 인스턴스를 파괴하여 중복 생성을 방지합니다.
                Destroy(this);
            }
        }
    }
}