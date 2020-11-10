using UnityEngine;

namespace _Scripts.States
{
    public class State
    {
        protected enum InStage
        {
            Idle,
            Attack,
            Casting,
            Defensive
        };

        protected GameObject ActiveNpc;
        protected Animator Anim;
        protected bool FirstTimeUpdate = true;

        protected State NextStage;
        protected InEvent StageEvent;

        protected InStage StageName;


        protected State(GameObject activeNpc, Animator anim)
        {
            StageEvent = InEvent.Enter;
            ActiveNpc = activeNpc;
            Anim = anim;
        }

        public bool IsOurTeam { set; get; }


        protected virtual void Enter()
        {
            StageEvent = InEvent.Update;
        }

        protected virtual void Update()
        {
            StageEvent = InEvent.Update;
        }

        protected virtual void Exit()
        {
            StageEvent = InEvent.Exit;

        }

        public State Process()
        {
            if (StageEvent == InEvent.Enter)
            {
                Enter();
            }
            if (StageEvent == InEvent.Update)
            {
                Update();
            }
        
            if (StageEvent == InEvent.Exit)
            {
                Exit();
                return NextStage;
            }

            return this;
        }

        protected enum InEvent
        {
            Enter,
            Update,
            Exit
        };
    }
}
