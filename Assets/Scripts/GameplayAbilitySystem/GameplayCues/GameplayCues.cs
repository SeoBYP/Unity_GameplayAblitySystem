using System;
using System.Threading.Tasks;
using GameplayAbilitySystem.SOs;
using UnityEngine;

namespace GameplayAbilitySystem
{
    /// <summary>
    /// GameplayCue 클래스는 게임 내에서 특정 이벤트나 효과를 시각적, 청각적으로 표시하기 위해 사용되는 Cue를 관리합니다.
    /// </summary>
    [Serializable]
    public class GameplayCue
    {
        /// <summary>
        /// Cue에 사용될 프리팹 오브젝트
        /// </summary>
        public GameObject prefab;
        /// <summary>
        /// 인스턴스화된 Cue 오브젝트
        /// </summary>
        public GameObject instance;
        /// <summary>
        /// Cue와 연결된 태그
        /// </summary>
        public GameplayTag tag;
        /// <summary>
        /// Cue의 위치 오프셋 (x, y, z)
        /// </summary>
        public Vector3 offset;

        /// <summary>
        /// Cue 적용에 대한 추가 데이터
        /// </summary>
        public GameplayCueApplicationData applicationData;

        /// <summary>
        /// Cue를 추가합니다. 즉시 제거 옵션이 활성화된 경우, Cue를 추가한 직후 제거합니다.
        /// </summary>
        /// <param name="asc">능력 시스템 컴포넌트</param>
        /// <param name="instantDestroy">Cue를 즉시 제거할지 여부</param>
        /// <param name="appData">추가 데이터</param>
        public virtual void AddCue(AbilitySystemComponent asc, bool instantDestroy, GameplayCueApplicationData appData)
        {
            if (prefab == null)
            {
                Debug.Log($"AddCue with NULL Prefab");
                return;
            }
            
            applicationData = appData;
            PlaceCue(asc); // Cue를 특정 위치에 배치
            if (instantDestroy)
            {
                RemoveCue(asc); // 즉시 제거
            }
        }
        /// <summary>
        /// Cue를 제거합니다. 제거 시 3초 딜레이를 추가로 적용합니다.
        /// </summary>
        /// <param name="asc">능력 시스템 컴포넌트</param>
        public virtual async void RemoveCue(AbilitySystemComponent asc)
        {
            // Cue 인스턴스가 존재하면 "OnDestroySoon" 메시지를 보냄
            if(instance != null) instance.SendMessage("OnDestroySoon", SendMessageOptions.DontRequireReceiver);
            
            // 3초 대기
            await Task.Delay(3_000);

            // ASC의 Cue 리스트에서 제거
            asc.instancedCues.Remove(this);

            // 인스턴스가 존재하면 제거
            if(instance == null) return;
            GameObject.Destroy(instance);
        }

        /// <summary>
        /// Cue를 능력 시스템 컴포넌트의 위치에 배치합니다.
        /// </summary>
        /// <param name="asc">능력 시스템 컴포넌트</param>
        public void PlaceCue(AbilitySystemComponent asc)
        {
            // Cue 프리팹 인스턴스화
            instance = GameObject.Instantiate(prefab);
            instance.name = "cueInstance_" + prefab.name;

            // Cue의 부모를 ASC로 설정하고 위치를 계산하여 배치
            instance.transform.SetParent(asc.transform);
            instance.transform.position = asc.transform.position + asc.transform.forward * offset.z
                                                                 + asc.transform.right * offset.x + asc.transform.up * offset.y;

            // ASC의 Cue 리스트에 추가
            asc.instancedCues.Add(this);
        }
    }
}