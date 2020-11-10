using _Scripts.Managers;
using _Scripts.NPC_Scripts;
using _Scripts.ScriptableObjects;
using _Scripts.UI_And_Animation;
using UnityEngine;

namespace _Scripts.Abilities_Factory
{
    public class TwoHandsAttackAbility : Ability
    {
        public override string Name => "Two Hands Attack";

        public override void Process(GameObject selectedNpc, GameObject targetNpc, AbilityInfo abilityInfo )
        {
            float damage = Random.Range(abilityInfo.abilityMinDmg, abilityInfo.abilityMaxDmg);
            Npc enemyUnitNpc = targetNpc.GetComponent<Npc>();
            enemyUnitNpc.TakeDamage(damage);
            AnimatingNpc.Instance.AnimatingAttackSlash(selectedNpc);
            GameEnvironment.Instance.PlaySlashSound();

        }
    }  
}
