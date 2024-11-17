using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
using Unity.VisualScripting;

namespace GameplayAbilitySystem.GameplayEffects
{
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
                    Debug.Log("Movement Complete!");
                    OnMovementComplete(source, target);
                }).BindToPosition(currentTransform);
        }
        // 이동 완료 후 추가 작업을 정의할 수 있는 메서드
        private void OnMovementComplete(AbilitySystemComponent source, AbilitySystemComponent target)
        {
            // 이동이 완료된 후 필요한 로직 추가
            Debug.Log($"{target.name} has reached the destination set by {source.name}.");
        }
    }
}