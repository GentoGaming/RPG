using _Scripts.NPC_Scripts;
using _Scripts.ScriptableObjects;
using _Scripts.UI_And_Animation;
using UnityEngine;

namespace _Scripts.Abilities_Factory
{
    public class IceMagicAbility : Ability
    {
        public override string Name => "Cast Ice Frost";

        public override void Process(GameObject selectedNpc, GameObject targetNpc, AbilityInfo abilityInfo)
        {
            float damage = Random.Range(abilityInfo.abilityMinDmg, abilityInfo.abilityMaxDmg);
            Npc enemyUnitNpc = targetNpc.GetComponent<Npc>();
            enemyUnitNpc.TakeDamage(damage);
            AnimatingNpc.Instance.AnimatingAttackJab(selectedNpc);
            StaffAnim.Instance.PlayAnim(targetNpc.transform.position);
        }
    } 
}
