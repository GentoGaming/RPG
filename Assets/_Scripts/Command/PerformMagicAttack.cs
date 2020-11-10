using _Scripts.NPC_Scripts;
using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Command
{
    public class PerformMagicAttack : Command
    {
        private readonly GameObject _unitControlled;
        private readonly GameObject _unitTarget;
        private readonly AbilityInfo _abilityName;
        private Npc _npc;


        public PerformMagicAttack(GameObject unitControlled, GameObject unitTarget, AbilityInfo abilityName) : base(
            unitControlled, unitTarget, abilityName)
        {
            _unitControlled = unitControlled;
            _unitTarget = unitTarget;
            _abilityName = abilityName;
        }

        public override void Execute()
        {
            _npc = _unitControlled.GetComponent<Npc>();
            _npc.EnemyTarget = _unitTarget;
            _npc.MovingToTargetAttack = false;
            _npc.IsPerformingATask = true;
            _npc.CastingMagicAttack = true;
            _npc.AbilityName = _abilityName;
        }

        public override void ExecuteReturnCommand()
        {
        }


        public override GameObject GetEnemyUnit()
        {
            return _unitTarget;
        }

        public override bool IsTaskStillRunning()
        {
            return _npc.IsPerformingATask;
        }


        public override GameObject GetCommandUnit()
        {
            return _unitControlled;
        }
    }
}