using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Abilities_Factory
{
    public class DefenceAbility : Ability
    {
        public override string Name => "Defence";

        public override void Process(GameObject selectedNpc, GameObject targetNpc, AbilityInfo abilityInfo)
        {
        }
    }
}