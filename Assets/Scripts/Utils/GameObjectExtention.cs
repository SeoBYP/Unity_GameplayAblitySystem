using UnityEngine;
using System.Collections;

namespace GameplayAbilitySystem.Utils
{
    public static class GameObjectExtention
    {
        public static T GetComponentAround<T>(this GameObject obj) where T : Component
        {
            T comp = obj.GetComponent<T>();
            if (comp == null)
                comp = obj.GetComponentInParent<T>();
            if(comp == null)
                comp = obj.GetComponentInChildren<T>();
            return comp;
        }
    }
}