# Unity Gameplay Ability System (GAS)

## 개요

Gameplay Ability System (GAS)는 Unity에서 캐릭터의 능력(스킬), 효과, 속성을 체계적으로 관리할 수 있는 모듈형 프레임워크입니다.
이 프로젝트는 리그 오브 레전드의 블리츠크랭크 **"로켓 그랩"** 과 유사한 스킬을 Unity GAS를 기반으로 구현합니다.

## 주요 기능

- **능력(Abilities)**: 캐릭터가 수행할 수 있는 행동, 스킬 등을 정의하고 관리.
- **효과(Effects)**: 능력이 캐릭터에게 미치는 영향을 처리.
- **속성(Attributes)**: 캐릭터의 상태(예: 체력, 마나, 공격력 등)를 동적으로 관리.
- **태그(Tags)**: 능력 및 효과에 대한 조건과 필터링 지원.
- **큐(Cues)**: 효과 발생 시 시각적 및 청각적 피드백 제공.

---

### 🎯 과제 필수 요구 사항

#### 1. 스킬 시스템 구현

- Excel, CSV 파일 또는 ScriptableObject에서 스킬 데이터를 읽어와 스킬을 사용할 수 있는 시스템을 구축합니다.
- Skill은 여러개의 Effect를 가지고 있습니다.
- Effect는 자원 소모, 스킬의 기능, 기타 효과 등 모든 동작이 컴포넌트화 되어있는 모듈을 뜻합니다
- Skill이 시전되면 Effect들 이 실행됩니다.

=> **구현 내용** :

- ScriptableObject를 통해서 Skill Ability 데이터를 불러와서 스킬을 사용합니다.
- 각 Skill Ability에 Effects SO 를 통해서 Scriptable Object를 통해 구현된 Gameplay Effect 효과를 적용할 수 있습니다.
- Effect를 통해서 적의 체력 및 공격력 등을 감소 시킬 수 있습니다.
- Skill이 시전되면 Effect들 이 실행됩니다.

=> **테스트**

> 1, 플레이어를 WASD키를 입력하여 움직일 수 있습니다.  
> 2, 마우스 좌클릭을 통해서 발사체를 발사할 수 있습니다.  
> 3, 적이 발사체의 피격이 피격 당시의 플레이어의 위치로 이동합니다.  
> 4, 적의 체력이 다 되면 적은 1초후에 파괴됩니다.

> 추가 : 왼쪽 쉬프트 키를 입력하면 플레이어가 대쉬합니다.

#### 2. 스킬 기능 구현

- 특정 키 입력 시 스킬을 발동하여 적을 플레이어 앞으로 끌어오는 기능을 개발합니다.

=> 구현 내용 :
PlayerController 에 내장되어 있는 각 Input Action에 맞는 키를 눌렀을 시 해당 Ability가 실행됩니다.

> 블리츠크랭크 "로켓 그랩"과 유사한 스킬은 마우스 좌클릭시 발사체가 나가고 발사체에 적이 피격 되었을 시 플레이어 위치로 이동합니다.

### ✨ 추가 요구 사항

#### 1. 기획 작업 편의성 고려

- **데이터 관리 도구**:
  - **스킬 데이터 관리 툴** 제작.
  - 게임 디자이너가 스킬 데이터를 직관적으로 편집하고 저장할 수 있는 UI 제공.

=> 구현 내용 :
현재는 Scriptable Object를 통해서 각 Ability 및 Effect, Cue 등의 데이터를 추가, 수정, 제거가 가능합니다

> 추후 Odin Inspector와 같은 Editor 편집기를 통해서 더욱 직관적이게 보일 수 있도록 수정할 수 있습니다.

#### 2. VFX, SFX, Indicator 추가

- **스킬 효과 강화**:
  - 스킬 발동 시 **시각적 효과(VFX)** 추가.
  - 스킬 실행 음향 효과(**SFX**) 적용.
  - **사거리 표시(Indicator)** 기능 구현.

=> 구현 내용
발사체 발동시 Gameplay Cue를 통해서 인스턴스를 생성하고 사운드 및 시각적 효과 를 재생합니다.

> 1, 추후 ObjectPooling을 적용하여 최적화를 진행할 수 있습니다.  
> 2, Unity Addressable과 연동하여 Resource 관리를 진행할 수 있습니다.

#### 3. 기타 스킬 관련 추가 구현

- **스킬 쿨다운 관리**.
- 적 AI가 스킬에 반응하도록 기능 추가.

=> 구현 내용
Ability에 Duration Value를 통해서 스킬 쿨다운을 관리할 수 있습니다.

---

> 사거리 표시(Indicator), 적 AI가 스킬에 반응하는 기능은 시간이 없어서 구현하지 못했습니다.

---

## 사용한 라이브러리

#### LitMotion

- LitMotion은 Unity에서 애니메이션, 트윈(Tween) 효과를 간편하게 구현할 수 있도록 돕는 경량화된 애니메이션 라이브러리입니다.
- UniTask와 연동하여 유지적으로 비동기 처리 및 예외 처리가 가능합니다.

#### UniTask

- 할당 없는 비동기 처리가 가능한 비동기 라이브러리입니다.
- 현 프로젝트에서는 LitMotion의 예외 처리만을 위해 사용되었습니다.

---

## 핵심 개념

### 1. **Ability System Component**

모든 능력, 효과, 속성을 관리하는 시스템의 핵심 컴포넌트입니다.

- 능력 활성화 및 해제.
- 효과 적용 및 제거.
- 속성 값 변경 및 이벤트 처리.
- 태그 관리.

#### 예제:

```csharp
public class AbilitySystemComponent : MonoBehaviour
{
    public async void TryActivateAbility(GameplayAbility ga, AbilitySystemComponent target,
        string activationGUID = null)
    {
        ga.source = this;
        ga.target = target;
        ga.activationGUID = activationGUID;

        OnGameplayAbilityTryActivated?.Invoke(ga, activationGUID);

        if (ga.isActive)
        {
            ga.DeactivateAbility(ga.activationGUID);
            return;
        }

        await InputBuffering(ga, target, activationGUID);

        if (!ga.CanActivateAbility(this, target, activationGUID, true)) return;

        ga.CommitAbility(this, target, ga.activationGUID);
    }
}
```

### 2. **능력 (GameplayAbility)**

캐릭터가 수행할 수 있는 특정 행동이나 스킬을 정의합니다.

주요 특징:

- 능력 실행 조건.
- 능력 활성화 및 비활성화 로직.
- 능력과 연결된 효과.

**예제**:

```csharp
public class Projectile : GameplayAbility
{
    public override void ActivateAbility(AbilitySystemComponent source, AbilitySystemComponent target, string activationGUID)
    {
        base.ActivateAbility(source, target, activationGUID);

        if (projectilePrefab == null)
            projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        else
            projectile = GameObject.Instantiate(projectilePrefab);

        projectile.name = "projectile";
        projectile.transform.position = source.transform.position;
        projectile.transform.rotation = source.transform.rotation;

        var rb = projectile.AddComponent<Rigidbody>();
        rb.drag = 0;
        rb.useGravity = false;
        rb.velocity = GetProjectileDirection();

        var projectileComponent = projectile.AddComponent<Projectile>();
        projectileComponent.OnHit += (hitAsc) => {
            // Debug.Log($"ProjectileAbility hitAsc.name {hitAsc.name}");
            effects.ForEach(ge => hitAsc.ApplyGameplayEffect(source, hitAsc, ge, activationGUID));
        };
        projectileComponent.source = source;

        base.DeactivateAbility(activationGUID);
    }
}
```

### 3. **효과(GameplayEffect)**

캐릭터의 속성을 변경하거나 추가적인 동작을 수행하는 로직을 정의합니다.

주요 특징:

- 지속 시간(Duration) 설정.
- 속성(Attribute) 수정.
- 태그(Tag) 부여 및 제거.

**예제**:

```csharp
public class GameplayEffect_KnockBackToPlayer : GameplayEffect
{
    public float speed; // 이동 속도
    public override void ApplyEffect(AbilitySystemComponent source, AbilitySystemComponent target)
    {
        base.ApplyEffect(source, target);
        // 에러 검사를 통해 소스와 타겟이 모두 유효한지 확인
        if (target == null || source == null)
        {
            Debug.LogWarning("MovementGameplayEffect: Source, Target is null!");
            return;
        }

        Transform targetTransform = source.transform;
        Transform currentTransform = target.transform;

        // 이동 거리 계산
        float distance = Vector3.Distance(currentTransform.position, targetTransform.position);

        // 이동 지속 시간 동적 계산 (속도 기반)
        float calculatedDuration = Mathf.Clamp(durationValue, 0.1f, distance / speed);

        // LitMotion을 활용한 부드러운 이동
        var motionHandle = LMotion.Create(currentTransform.position, targetTransform.position, calculatedDuration)
            .WithEase(Ease.OutQuad)
            .WithScheduler(MotionScheduler.FixedUpdate)
            .WithOnComplete(() =>
            {
                if (target != null) // 타겟이 여전히 유효한 경우에만 완료 로직 실행
                {
                    Debug.Log("Movement Complete!");
                    OnMovementComplete(source, target);
                }
            }).BindToPosition(currentTransform)
            .AddTo(target);
    }
}
```

### 4. **속성(GameplayAttribute)**

캐릭터의 상태(예: 체력, 마나, 이동 속도 등)를 정의하고 관리합니다.

주요 특징:

- 기본 값(Base Value) 및 현재 값(Current Value) 관리.
- 속성 변경 시 이벤트 발생.

**예제**:

```csharp
using System;
using GameplayAbilitySystem;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.SOs;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        private Rigidbody rb;

        private AbilitySystemComponent _abilitySystemComponent;

        public AbilitySystemComponent AbilitySystemComponent =>
            _abilitySystemComponent ??= GetComponent<AbilitySystemComponent>();

        public AttributeName _health;
        public AttributeName _maxHealth;
        private void OnHealthChanged(AttributeName attributeName, float oldValue, float newValue, GameplayEffect ge)
        {
            if (attributeName == _health)
            {
                // 체력이 0이하면 1초뒤에 파괴
                if (newValue <= 0)
                {
                    Destroy(gameObject,1);
                }
            }
        }

        private void OnEnable()
        {
            AbilitySystemComponent.OnAttributeChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            AbilitySystemComponent.OnAttributeChanged -= OnHealthChanged;
        }
    }
}
```

### 5. **태그 (GameplayTag)**

능력과 효과가 특정 조건에 따라 적용되거나 제거되도록 제어합니다.

주요 특징:

- 효과 중복 방지.
- 능력 실행 조건 필터링.
- 특정 태그에 따라 능력 활성화 조건 설정.
  **예제**:

```csharp
using UnityEngine;

namespace GameplayAbilitySystem.SOs
{
    /// <summary>
    /// GameplayTag 클래스는 게임 내 태그(Tag)를 정의하는 ScriptableObject입니다.
    /// 특정 상태나 카테고리를 나타내기 위해 사용됩니다.
    /// </summary>
    [CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Tag",fileName = "New Gameplay Tag")]
    public class GameplayTag : ScriptableObject
    {
        // 이 클래스는 현재 속성이나 메서드를 가지지 않지만,
        // 게임 내에서 태그를 구별하기 위한 기본 구조로 사용됩니다.
        // 예를 들어, "Fire", "Stunned", "Invincible"과 같은 태그를 정의할 수 있습니다
    }
}
```

### 6. **큐 (GameplayCue)**

GameplayCue는 효과 발생 시 플레이어에게 피드백(시각적, 청각적 효과)을 제공합니다.

주요 특징:

- VFX(시각적 효과), SFX(사운드 효과) 처리.
- 효과 시작, 중단 시 발생.
  **예제**:

```csharp
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
```
