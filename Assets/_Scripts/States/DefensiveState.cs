using _Scripts.NPC_Scripts;
using _Scripts.UI_And_Animation;
using UnityEngine;

namespace _Scripts.States
{
    public class DefensiveState : State
    {
        private readonly Npc _npc;
        private Vector3 _enemyPos;
        private float _step;
        private Vector3 _unitControlledPos;

        public DefensiveState(Npc npc, GameObject activeNpc, Animator anim) : base(activeNpc, anim)
        {
            StageName = InStage.Defensive;
            ActiveNpc = activeNpc;
            Anim = anim;
            _npc = npc;
        
        }

        protected override void Enter()
        {
            AnimatingNpc.Instance.AnimatingDefensive(ActiveNpc, true);
            _npc.IsPerformingATask = false;
            _npc.IsReturning = false;
            base.Enter();
        }

        protected override void Update()
        {
            _npc.IsPerformingATask = false;
            if (!_npc.IsDefensive)
            {
                NextStage = new IdleState(_npc, ActiveNpc, Anim);
                StageEvent = InEvent.Exit;
            }
        
        }


        protected override void Exit()
        {
            AnimatingNpc.Instance.AnimatingDefensive(ActiveNpc, false);
            base.Exit();
        }
    }
}
