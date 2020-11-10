using System;
using System.Collections;
using _Scripts.Command;
using _Scripts.Managers;
using _Scripts.ScriptableObjects;
using _Scripts.States;
using _Scripts.UI_And_Animation;
using DamagePopups;
using UnityEngine;

namespace _Scripts.NPC_Scripts
{
    public class Npc : MonoBehaviour
    {
        public NpcInfo npcInfo;
        private Animator _animator;
        private AnimatingNpc _animInstance;
        private State _currentState;
        private float _health;
        private GameEnvironment _instance;
        private Vector3 _tempPos;

        [NonSerialized] public AbilityInfo AbilityName;
        [NonSerialized] public bool CastingMagicAttack;
        [NonSerialized] public GameObject EnemyTarget;
        [NonSerialized] public bool IsDead;
        [NonSerialized] public bool IsDefensive;
        [NonSerialized] public bool IsPerformingATask;
        [NonSerialized] public bool IsReturning;


        [NonSerialized] public bool MovingToTargetAttack;

        [NonSerialized] public Vector3 PreviousPosition;

        void Start()
        {
            _instance = GameEnvironment.Instance;
            _animInstance = AnimatingNpc.Instance;
            _animator = GetComponentInChildren<Animator>();
            _currentState = new IdleState(this, this.gameObject, _animator);
            PreviousPosition = transform.position;
            _health = npcInfo.health;
        }

        private void Update()
        {
            _currentState = _currentState.Process();
        }

        private void OnMouseExit()
        {
            _animInstance.AnimatingIdleAndActive(gameObject, false);

            if (_instance.WaitingToPickTarget)
            {
                _instance.ChangeCursor((int) CursorT.Hand);
                return;
            }

            _instance.ChangeCursor((int) CursorT.Basic);
            _instance.OnMouseOverNpcAction(gameObject, npcInfo, false, _health);
        }

        private void OnMouseOver()
        {
            if (npcInfo.isTeamPlayer && !CommandInvoker.IsAlreadyQueued(gameObject))
            {
                _animInstance.AnimatingIdleAndActive(gameObject, true);
                _instance.ChangeCursor((int) CursorT.Select);
            }

            _instance.OnMouseOverNpcAction(gameObject, npcInfo, true, _health);
        }


        private void OnMouseUp()
        {
            if (CommandInvoker.IsAlreadyQueued(gameObject) && npcInfo.isTeamPlayer) return;
            _instance.OnMouseSelectNpcAction(gameObject, npcInfo, true, _health);
            _instance.ChangeCursor((int) CursorT.Basic);
        }

        public void SetNpcToIdle()
        {
            IsReturning = false;
            IsPerformingATask = false;
            MovingToTargetAttack = false;
            CastingMagicAttack = false;
            //IsDefensive = false;
            AbilityName = null;
        }

        public void TakeDamage(float dmgTakes)
        {
            if (IsDefensive)
            {
                dmgTakes =  dmgTakes - (dmgTakes * 70) / 100;
            }
            
            dmgTakes = dmgTakes - (dmgTakes * npcInfo.armor) / 100;
            _health -= (int) dmgTakes;
            _tempPos = transform.position + new Vector3(-1, 4, 0);
            if (_health < 1)
            {
                IsDead = true;
                Destroy(GetComponent<CapsuleCollider2D>());
            }

            Invoke(nameof(TakeAHit), 0.4f);

            StartCoroutine(ShowDmg(dmgTakes));
        }

        private void TakeAHit()
        {
            if (_health < 1)
            {
                AnimatingNpc.Instance.AnimatingDefensive(gameObject, false);

                AnimatingNpc.Instance.AnimatingDieBack(gameObject);
            }
            else
                AnimatingNpc.AnimatingDamage(gameObject);
        }


        IEnumerator ShowDmg(float dmgTakes)
        {
            yield return new WaitForSeconds(0.5f);
            DamagePopup.Create(_tempPos, (int) dmgTakes);
        }
    }
}