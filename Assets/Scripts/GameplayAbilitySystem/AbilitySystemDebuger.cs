using System;
using System.Linq;
using UnityEngine;

namespace GameplayAbilitySystem
{
    public class AbilitySystemDebuger : MonoBehaviour, IAbilitySystemInterface
    {
        private AbilitySystemComponent _abilitySystemComponent;

        public AbilitySystemComponent AbilitySystemComponent =>
            _abilitySystemComponent ??= GetComponent<AbilitySystemComponent>();

        public Vector2 scrollPosition; // 스크롤뷰를 위한 변수

        private bool showDebugUI = false; // 디버그 UI 토글 상태

        // 디버그 UI의 버튼 위치를 관리하는 정적 속성
        private static int toggleButtonCount = 0; // 현재 생성된 토글 버튼 개수
        private static readonly int buttonWidth = 200;
        private static readonly int buttonHeight = 30;

        // 현재 버튼 인덱스 (해당 디버거의 고유 버튼 위치를 설정)
        private int toggleButtonIndex;

        private void Awake()
        {
            // 인스턴스가 생성될 때 고유 버튼 위치를 지정
            toggleButtonIndex = toggleButtonCount++;
        }


        public AbilitySystemComponent GetAbilitySystemComponent()
        {
            return AbilitySystemComponent;
        }

        private void OnGUI()
        {
            // 토글 버튼 위치 설정
            var toggleButtonX = 10;
            var toggleButtonY = 10 + toggleButtonIndex * (buttonHeight + 5);

            // 토글 버튼 생성
            showDebugUI = GUI.Toggle(
                new Rect(toggleButtonX, toggleButtonY, buttonWidth, buttonHeight),
                showDebugUI,
                $"{gameObject.name} Debug"
            );

            // 디버그 UI가 비활성화 상태면 반환
            if (!showDebugUI) return;

            // 디버그 UI 영역
            GUILayout.BeginArea(new Rect(220, 10, 400, Screen.height - 20), $"{gameObject.name} Ability System",
                GUI.skin.window);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            if (AbilitySystemComponent == null)
            {
                GUILayout.Label("Ability System not assigned.");
                GUILayout.EndScrollView();
                GUILayout.EndArea();
                return;
            }

            // Abilities Section
            GUILayout.Label("Abilities", GUI.skin.box);
            if (AbilitySystemComponent.grantedGameplayAbilities.Any())
            {
                foreach (var ability in AbilitySystemComponent.grantedGameplayAbilities)
                {
                    GUILayout.Label($"- {ability.name}");
                    GUILayout.Label($"  Active: {ability.isActive}");
                    GUILayout.Label($"  Cooldown: {ability.cooldown}");
                    GUILayout.Label($"  Activation GUID: {ability.activationGUID}");
                }
            }
            else
            {
                GUILayout.Label("No abilities granted.");
            }

            GUILayout.Space(10);

            // Gameplay Effects Section
            GUILayout.Label("Gameplay Effects", GUI.skin.box);
            if (AbilitySystemComponent.appliedGameplayEffects.Any())
            {
                foreach (var effect in AbilitySystemComponent.appliedGameplayEffects)
                {
                    GUILayout.Label($"- {effect.name}");
                    GUILayout.Label($"  Duration: {effect.durationValue}");
                    GUILayout.Label($"  Source: {effect.source?.name}");
                    GUILayout.Label($"  Target: {effect.target?.name}");
                }
            }
            else
            {
                GUILayout.Label("No active effects.");
            }

            GUILayout.Space(10);

            // Gameplay Tags Section
            GUILayout.Label("Gameplay Tags", GUI.skin.box);
            if (AbilitySystemComponent.tags.Any())
            {
                foreach (var tag in AbilitySystemComponent.tags)
                {
                    GUILayout.Label($"- {tag.name}");
                }
            }
            else
            {
                GUILayout.Label("No tags.");
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}