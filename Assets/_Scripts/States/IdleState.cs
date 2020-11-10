using _Scripts.NPC_Scripts;
using _Scripts.UI_And_Animation;
using UnityEngine;

namespace _Scripts.States
{
    public class IdleState : State
    {
        private readonly Npc _npc;

        public IdleState(Npc npc, GameObject activeNpc, Animator anim) : base(activeNpc, anim)
        {
            ActiveNpc = activeNpc;
            StageName = InStage.Idle;
            Anim = anim;
            _npc = npc;
        }


        protected override void Enter()
        {
            if (ActiveNpc.transform.localScale.x < 0 && _npc.npcInfo.isTeamPlayer)
            {
                ActiveNpc.transform.localScale = Vector3.Scale( ActiveNpc.transform.localScale, new Vector3(-1, 1, 1) );
            }else if (ActiveNpc.transform.localScale.x > 0 && !_npc.npcInfo.isTeamPlayer)
            {
                ActiveNpc.transform.localScale = Vector3.Scale( ActiveNpc.transform.localScale, new Vector3(-1, 1, 1) );
            }
        
            AnimatingNpc.Instance.AnimatingIdleAndActive(ActiveNpc, false);
            _npc.SetNpcToIdle();
            base.Enter();
        }

        protected override void Update()
        {
            if (_npc.IsDefensive)
            {
                NextStage = new DefensiveState( _npc,  ActiveNpc,  Anim);
                StageEvent = InEvent.Exit;
            }else if (_npc.IsPerformingATask && _npc.CastingMagicAttack)
            {
                NextStage = new AttackState( _npc,  ActiveNpc,  Anim);
                StageEvent = InEvent.Exit;
            }else if (_npc.IsPerformingATask &&_npc.MovingToTargetAttack)
            {
                NextStage = new AttackState( _npc,  ActiveNpc,  Anim);
                StageEvent = InEvent.Exit;
            }
        }
        
    }
}
 
