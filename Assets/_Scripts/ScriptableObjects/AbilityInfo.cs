using UnityEngine;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability", order = 52)]

    public class AbilityInfo : ScriptableObject
    {
        public string abilityName;
        public Sprite abilityIcon;
        public float  abilityMinDmg;
        public float  abilityMaxDmg;
        public float  spellRange;
    }
}
