using System.Collections.Generic;
using GameplayAbilitySystem.GameplayEffects;

namespace GameplayAbilitySystem.Attributes
{
    //Only for Stats attributes
    [System.Serializable]
    public class AttributeModifier
    {
        public float value;

        public void Value(List<Modifier> modifiers, GameplayEffect ge)
        {
            foreach (var modifier in modifiers)
            {
                value += modifier.GetValue(ge);
                break;
            }
        }

        public void Clear()
        {
            value = 0;
        }
    }
}