using _Scripts.Managers;
using _Scripts.NPC_Scripts;
using _Scripts.UI_And_Animation;
using UnityEngine;

namespace _Scripts.States
{
    public class AttackState : State
    {
        private readonly Npc _npc;
        private Vector3 _enemyPos;
        private float _step;
        private Vector3 _unitControlledPos;
        private float speed = 6f;

        public AttackState(Npc npc, GameObject activeNpc, Animator anim) : base(activeNpc, anim)
        {
            StageName = InStage.Attack;
            ActiveNpc = activeNpc;
            Anim = anim;
            _npc = npc;
        
        }

        protected override void Enter()
        {
            GameEnvironment.Instance.PlayWalkSound();
        
            if (_npc.IsReturning)
            {
                if (ActiveNpc.transform.localScale.x > 0 && _npc.npcInfo.isTeamPlayer)
                {
                    ActiveNpc.transform.localScale = Vector3.Scale( ActiveNpc.transform.localScale, new Vector3(-1, 1, 1) );
                }else if (ActiveNpc.transform.localScale.x < 0 && !_npc.npcInfo.isTeamPlayer)
                {
                    ActiveNpc.transform.localScale = Vector3.Scale( ActiveNpc.transform.localScale, new Vector3(-1, 1, 1) );
                }
            }

            AnimatingNpc.Instance.AnimatingIsWalking(ActiveNpc, true);
            base.Enter();
        }


        protected override void Update()
        {
            if (_npc.IsReturning)
            {
                _enemyPos = _npc.PreviousPosition;
                MoveTowardsTarget(ActiveNpc.transform, _enemyPos, 0.01f);
            }
            else
            {
                if (_npc.npcInfo.isRangedUnit)
                {
                    NextStage = new CastingState(_npc, ActiveNpc, Anim);
                    StageEvent = InEvent.Exit;
                }
                else
                {
                    _enemyPos = _npc.EnemyTarget.transform.position;
                    MoveTowardsTarget(ActiveNpc.transform, _enemyPos, 0.01f);
                }
            }
        }

        private void MoveTowardsTarget(Transform unitControlled, Vector3 targetPosition,float distance2)
        {

        
            if (_npc.npcInfo.isRangedUnit)
            {
                NextStage = new CastingState( _npc,  ActiveNpc,  Anim);
                StageEvent = InEvent.Exit;
            }
            else
            {
                //first, check to see if we're close enough to the target
                if (Vector3.Distance(unitControlled.position, targetPosition) > _npc.npcInfo.hasAbilities[0].spellRange)
                {
                    Vector3 directionOfTravel = targetPosition - unitControlled.position;
                    directionOfTravel.Normalize();

                    unitControlled.Translate(
                        (directionOfTravel.x * speed * Time.deltaTime),
                        (directionOfTravel.y * speed * Time.deltaTime),
                        (directionOfTravel.z * speed * Time.deltaTime),
                        Space.World);
                
                }else if ( Vector3.Distance(unitControlled.position,  new Vector3(unitControlled.position.x,targetPosition.y,unitControlled.position.z)) > distance2)
                {
                    Vector3 directionOfTravel = targetPosition - unitControlled.position;
                    unitControlled.Translate((0), (directionOfTravel.y * speed * 2 * Time.deltaTime), (0), Space.World);
                }
                else
                {
                    if (_npc.IsReturning)
                    {
                        NextStage = new IdleState(_npc, ActiveNpc, Anim);
                        StageEvent = InEvent.Exit;
                    }
                    else
                    {

                        NextStage = new CastingState(_npc, ActiveNpc, Anim);
                        StageEvent = InEvent.Exit;
                    }
                }

            }
        }

        protected override void Exit()
        {
            GameEnvironment.Instance.StopSound();
            // We go to attack State
            AnimatingNpc.Instance.AnimatingIsWalking(ActiveNpc, false);
            AnimatingNpc.Instance.AnimatingIdleAndActive(ActiveNpc, false); 
            base.Exit();
        }
    }
}
