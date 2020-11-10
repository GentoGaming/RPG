using _Scripts.NPC_Scripts;
using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Command
{
    public class GoIntoDefensiveMode : Command
    {
        private readonly GameObject _unitControlled;
        private AbilityInfo _abilityName;
        private Npc _npc;
        private readonly GameObject _unitTarget;

        public GoIntoDefensiveMode(GameObject unitControlled, GameObject unitTarget, AbilityInfo abilityName) : base(unitControlled, unitTarget, abilityName)
        {
            _unitControlled = unitControlled;
            _unitTarget = unitTarget;
            _abilityName = abilityName;
        }

        private void PrepareTask()
        {
            _npc = _unitControlled.GetComponent<Npc>();
            _npc.IsPerformingATask = true;
            _npc.IsDefensive = true;
            _npc.IsReturning = false;
        }

        public override void Execute()
        {
            PrepareTask();
        }

        public override void ExecuteReturnCommand()
        {
            _npc.IsPerformingATask = false;

            PrepareTask();

        }


        public override GameObject GetEnemyUnit()
        {
            return _unitTarget;
        }

        public override bool IsTaskStillRunning()
        {
            return  _npc.IsPerformingATask;

        }

        public override GameObject GetCommandUnit()
        {
            return _unitControlled;
        }
    }
}
