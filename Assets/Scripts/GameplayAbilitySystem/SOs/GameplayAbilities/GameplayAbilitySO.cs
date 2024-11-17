using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameplayAbilitySystem.GameplayAbilities;
using UnityEngine;

namespace GameplayAbilitySystem.SOs
{
    [Serializable]
    public class GameplayAbilitySO : ScriptableObject
    {
        [SerializeReference] public GameplayAbility ga;

        [ContextMenu("Create Cost GE")]
        public void CreateCostGE()
        {
            ga.CreateCostGE(new List<Modifier> { new BasicModifier() });
        }

        [ContextMenu("Create CoolDown GE")]
        public void CreateCoolDownGE()
        {
            ga.CreateCoolDownGE(1);
        }

        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(name))
            {
                string prefix = "GA_";
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
                string assetName = Path.GetFileNameWithoutExtension(assetPath);
                if (!assetName.StartsWith(prefix))
                {
                    UnityEditor.AssetDatabase.RenameAsset(assetPath, prefix + assetName);
                    UnityEditor.AssetDatabase.SaveAssets();
                }

                ga.name = assetName.Replace(prefix, "");
            }
        }
    }
}