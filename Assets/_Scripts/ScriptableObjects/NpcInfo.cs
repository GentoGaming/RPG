using UnityEngine;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NPC", menuName = "ScriptableObjects/NpcInfo", order = 1)]

    public class NpcInfo : ScriptableObject
    {
        public string unitName;
        public float health;
        public float armor;
        public AbilityInfo[] hasAbilities;
        public Sprite armorTexture;
        public bool isTeamPlayer;
        public bool isRangedUnit;
    }
}
