using _Scripts.NPC_Scripts;
using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Command
{
    public class PerformAnAttack : Command
    {
        private readonly GameObject _unitControlled;
        private readonly GameObject _unitTarget;
        private readonly AbilityInfo _abilityName;
        private Npc _npc;

        public PerformAnAttack(GameObject unitControlled, GameObject unitTarget, AbilityInfo abilityName) : base(unitControlled, unitTarget, abilityName)
        {
            _unitControlled = unitControlled;
            _unitTarget = unitTarget;
            _abilityName = abilityName;
        }


        private void PrepareTask()
        {
            _npc = _unitControlled.GetComponent<Npc>();
            _npc.EnemyTarget = _unitTarget;
            _npc.MovingToTargetAttack = true;
            _npc.IsPerformingATask = true;
            _npc.AbilityName = _abilityName;
        }

        public override void Execute()
        {
            PrepareTask();
        }

        public override void ExecuteReturnCommand()
        {

            _npc.IsReturning = true;
            PrepareTask();

        }


        public override GameObject GetEnemyUnit()
        {
            return _unitTarget;
        }

        public override bool IsTaskStillRunning()
        {
            return  _npc.IsPerformingATask ;

        }

        public override GameObject GetCommandUnit()
        {
            return _unitControlled;
        }
    }
}
