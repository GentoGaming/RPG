using _Scripts.Managers;
using _Scripts.NPC_Scripts;
using _Scripts.ScriptableObjects;
using _Scripts.UI_And_Animation;
using UnityEngine;

namespace _Scripts.Abilities_Factory
{
    public class SingleHandAttackAbility : Ability
    {
        public override string Name => "Single Hand Attack";

        public override void Process(GameObject selectedNpc, GameObject targetNpc, AbilityInfo abilityInfo)
    {
        float damage = Random.Range(abilityInfo.abilityMinDmg, abilityInfo.abilityMaxDmg);
        Npc enemyUnitNpc = targetNpc.GetComponent<Npc>();
        enemyUnitNpc.TakeDamage(damage);
        AnimatingNpc.Instance.AnimatingAttackJab(selectedNpc);
        GameEnvironment.Instance.PlaySlashSound();
    }
    }
}
