using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace GameplayAbilitySystem.SOs.GroupData
{
    [Serializable]
    public abstract class PrefixedScriptableObject : ScriptableObject
    {
        [Conditional("UNITY_EDITOR")]
        public virtual void OnValidate()
        {
            if (!string.IsNullOrEmpty(name))
            {
                string prefix = GetType().ToString().Replace("GAS.", "") + "_";
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(GetInstanceID());
                string assetName = Path.GetFileNameWithoutExtension(assetPath);
                if (!assetName.Contains(prefix))
                {
                    UnityEditor.AssetDatabase.RenameAsset(assetPath, prefix + assetName);
                    UnityEditor.AssetDatabase.SaveAssets();
                }
            }
        }
    }
}