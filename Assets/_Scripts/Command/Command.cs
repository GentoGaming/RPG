using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Command
{
    public abstract  class Command
    {
        private AbilityInfo _abilityName;
        private GameObject _unitControlled;
        private GameObject _unitTarget;

        protected Command(GameObject unitControlled, GameObject unitTarget, AbilityInfo  abilityName)
        {
            _unitControlled = unitControlled;
            _unitTarget = unitTarget;
            _abilityName = abilityName;
        }

        public abstract void Execute();

        public abstract void ExecuteReturnCommand();

        public abstract GameObject GetCommandUnit();

        public abstract GameObject GetEnemyUnit();

        public abstract bool IsTaskStillRunning();
    }
}
