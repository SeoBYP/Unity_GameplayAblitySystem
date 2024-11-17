using System;
using System.Diagnostics;
using GameplayAbilitySystem.Enums;
using GameplayAbilitySystem.GameplayEffects;
using GameplayAbilitySystem.Utils;
using UnityEngine;

namespace GameplayAbilitySystem.SOs
{
    [CreateAssetMenu(menuName = "Gameplay Ability System/Gameplay Effect/Gameplay Effect Base",fileName = "GE_")]
    [Serializable]
    public class GameplayEffectSO : ScriptableObject
    {
        public GameplayEffect ge;

        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(name))
            {
                string prefix = "GE_";
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
                string assetName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                if (!assetName.Contains(prefix))
                {
                    UnityEditor.AssetDatabase.RenameAsset(assetPath, prefix + assetName);
                    UnityEditor.AssetDatabase.SaveAssets();
                }
                ge.name = assetName.Replace(prefix, "");
            }
        }

        [ContextMenu("ADD MODIFIER WITH TYPE")]
        public void ADD_MODIFIER_VIA_EDITOR(EModifierType modifierType)
        {
            Helpers.AddModifier(modifierType,this.ge);
        }
    }
}