using GameplayAbilitySystem.Enums;
using GameplayAbilitySystem.SOs;
using UnityEditor;
using UnityEngine;

namespace Editor.GameplayAbilitySystem
{
    [CustomEditor(typeof(GameplayEffectSO))]
    [CanEditMultipleObjects]
    public class GameplayEffectSOEditor : UnityEditor.Editor
    {
        // 선택된 ModifierType을 저장하는 변수
        public EModifierType selectedModifierType;
        
        public override void OnInspectorGUI()
        {
            // Update serialized object
            serializedObject.Update();

            // 기본 Inspector UI 표시
            DrawDefaultInspector();

            // GameplayEffectSO를 대상으로 캐스팅
            GameplayEffectSO gameplayEffectSO = (GameplayEffectSO)target;

            selectedModifierType = (EModifierType)EditorGUILayout.EnumPopup("Modifier Type", selectedModifierType);
            
            // 버튼 생성
            if (GUILayout.Button("Add Modifier via Editor"))
            {
                // ADD_MODIFIER_VIA_EDITOR 함수 호출
                gameplayEffectSO.ADD_MODIFIER_VIA_EDITOR(selectedModifierType);

                // 변경 사항 저장
                EditorUtility.SetDirty(gameplayEffectSO);
            }

            // Apply modified properties
            serializedObject.ApplyModifiedProperties();
        }
    }
}