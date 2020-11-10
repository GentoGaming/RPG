using System;
using System.Threading.Tasks;
using _Scripts.Abilities_Factory;
using _Scripts.NPC_Scripts;
using _Scripts.UI_And_Animation;
using UnityEngine;

namespace _Scripts.States
{
    public class CastingState : State
    {
        private readonly Npc _npc;
        private Vector3 _enemyPos;
        private float _step;
        private Vector3 _unitControlledPos;

        public CastingState(Npc npc, GameObject activeNpc, Animator anim) : base(activeNpc, anim)
        {
            StageName = InStage.Casting;
            ActiveNpc = activeNpc;
            Anim = anim;
            _npc = npc;
        }

        protected override void Enter()
        {
            string abName = _npc.AbilityName == null ? "Single Hand Attack" : _npc.AbilityName.abilityName;
            Ability abilityToCast = AbilityFactory.GetAbility(abName);
            abilityToCast.Process(ActiveNpc, _npc.EnemyTarget, _npc.AbilityName);
            base.Enter();
            Task.Delay(new TimeSpan(0, 0, 2)).ContinueWith(o => { WaitAndExit(); });
        }

        void WaitAndExit()
        {
            NextStage = new IdleState(_npc, ActiveNpc, Anim);
            StageEvent = InEvent.Exit;
        }


        protected override void Exit()
        {
            // We go to attack State
            AnimatingNpc.Instance.AnimatingIdleAndActive(ActiveNpc, false);
            base.Exit();
        }
    }
}