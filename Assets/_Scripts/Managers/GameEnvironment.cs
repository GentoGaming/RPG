using System;
using System.Collections.Generic;
using _Scripts.Command;
using _Scripts.NPC_Scripts;
using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Managers
{
    public enum CursorT
    {
        Basic = 0,
        Select = 1,
        Hand,
    }

    public sealed class GameEnvironment : MonoBehaviour
    {
        private static GameEnvironment _instance;
        [NonSerialized] public static bool IsExecutingCommands;
        [NonSerialized] public static bool IsOurTurn = true;
        public static readonly List<Npc> TeamPlayersNpc = new List<Npc>();
        public static readonly List<Npc> EnemyTeamNpc = new List<Npc>();
        public Camera mainCamera;
        public static GameObject ourTurnGUI;
        public bool WaitingToPickTarget;
        public List<Texture2D> cursors;
        public AudioClip footStepSound;
        public AudioClip slashSound;
        public AudioClip electric;
        public GameObject gameResult;
        private AbilityInfo _latestAbility;
        private NpcInfo _npcInfo;
        private GameObject _npcSelected;
        [NonSerialized] public AudioSource AudioSource;

        public Action<GameObject, NpcInfo, bool, float> OnMouseOverNpcAction = delegate { };
        public Action<GameObject, NpcInfo, bool, float> OnMouseSelectNpcAction = delegate { };
        public Action<GameObject, NpcInfo, bool, float> OnSelfTargetSpell = delegate { };
        public static GameEnvironment Instance => _instance;
        public static bool isGameOver;
        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
            OnMouseSelectNpcAction += CommandLogic;

            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            GameObject team = GameObject.Find("TeamPlayer");
            GameObject enemies = GameObject.Find("EnemyTeam");
            ourTurnGUI = GameObject.Find("Your Turn"); 
            FillListWithChildren(team, TeamPlayersNpc);
            FillListWithChildren(enemies, EnemyTeamNpc);
        }


        private void Update()
        {
            if(!IsExecutingCommands) CheckResult();

            if (!IsOurTurn && !IsExecutingCommands)
            {
                IsExecutingCommands = true;
                StartCoroutine(CommandInvoker.ExecuteEnemyCommands1());
            }


            if (IsOurTurn && !IsExecutingCommands &&
                CommandInvoker.NumberOfCommands() == GetAliveHeroesCount(TeamPlayersNpc))
            {
                WaitingToPickTarget = false;
                ourTurnGUI.SetActive(false);
                IsExecutingCommands = true;
                StartCoroutine(CommandInvoker.ExecuteOurCommands());
            }
        }

        
        private void OnDisable()
        {
            // ReSharper disable once DelegateSubtraction
            if (Instance.OnMouseOverNpcAction != null) Instance.OnMouseOverNpcAction -= CommandLogic;
        }

        private void CheckResult()
        {
            if (GetAliveHeroesCount(TeamPlayersNpc) == 0)
            {
                gameResult.transform.GetChild(0).transform.gameObject.SetActive(true);
                isGameOver = true;
                ourTurnGUI.SetActive(false);
                return;
            }
            else if (GetAliveHeroesCount(EnemyTeamNpc) == 0)
            {
                gameResult.transform.GetChild(1).transform.gameObject.SetActive(true);
                isGameOver = true;
                ourTurnGUI.SetActive(false);
                return;
            }
        }


        private int GetAliveHeroesCount(List<Npc> list)
        {
            int counter = 0;
            foreach (var unit in list)
            {
                if (!unit.IsDead) counter++;
            }

            return counter;
        }


        public static GameObject EnemyTargetUnit()
        {
            GameObject target = null;
            float armor = 101f;
            foreach (Npc unit in TeamPlayersNpc)
            {
                if (!unit.IsDead && unit.npcInfo.armor < armor)
                {
                    armor = unit.npcInfo.armor;
                    target = unit.gameObject;
                }
            }

            return target;
        }

        public static void ResetAll(List<Npc> list, bool condition)
        {
            foreach (Npc unit in list)
            {
                unit.SetNpcToIdle();
                if (condition)
                {
                    unit.IsDefensive = false;
                }
            }
        }

        public void AttackButtonClicked()
        {
            WaitingToPickTarget = true;
            _latestAbility = _npcInfo.hasAbilities[0];
        }


        public void DefenceButtonClicked()
        {
            _latestAbility = _npcInfo.hasAbilities[1];
            CommandInvoker.AddCommand(new GoIntoDefensiveMode(_npcSelected, null, _latestAbility));
            OnSelfTargetSpell(gameObject, null, false, 1);
        }

        private void ResetCommand()
        {
            WaitingToPickTarget = false;
            _npcSelected = null;
            _npcInfo = null;
        }

        private void CommandLogic(GameObject npc, NpcInfo npcInfo, bool condition, float health)
        {
            if (npcInfo.isTeamPlayer)
            {
                _npcSelected = npc;
                _npcInfo = npcInfo;
            }
            else if (!npcInfo.isTeamPlayer && WaitingToPickTarget && !CommandInvoker.IsAlreadyQueued(_npcSelected))
            {
                if (_npcSelected.GetComponent<Npc>().npcInfo.isRangedUnit)
                {
                    CommandInvoker.AddCommand(new PerformMagicAttack(_npcSelected, npc, _latestAbility));
                }
                else
                {
                    CommandInvoker.AddCommand(new PerformAnAttack(_npcSelected, npc, _latestAbility));
                }

                ResetCommand();
            }
        }


        public void ChangeCursor(int cursorIcon)
        {
            Cursor.SetCursor(cursors[cursorIcon], Vector2.zero, CursorMode.Auto);
        }


        private void FillListWithChildren(GameObject parentObj, List<Npc> listNpc)
        {
            foreach (Transform child in parentObj.transform)
            {
                listNpc.Add(child.gameObject.GetComponent<Npc>());
            }
        }

        public static void FixTurn(bool condition)
        {
            IsOurTurn = condition;
            IsExecutingCommands = false;
            if (condition)
            {
                ResetAll(TeamPlayersNpc, true);
                ResetAll(EnemyTeamNpc, true);
            }
            else
            {
                ResetAll(TeamPlayersNpc, false);
                ResetAll(EnemyTeamNpc, false);
            }
        }

        public void PlayWalkSound()
        {
            StopSound();
            AudioSource.clip = footStepSound;
            AudioSource.loop = true;
            AudioSource.Play();
        }

        public void PlaySlashSound()
        {
            StopSound();
            AudioSource.clip = slashSound;
            AudioSource.loop = false;
            AudioSource.PlayDelayed(0.5f);
        }

        public void PlayElectricSound()
        {
            StopSound();
            AudioSource.clip = electric;
            AudioSource.loop = false;
            AudioSource.Play();
        }

        public void StopSound()
        {
            AudioSource.Stop();
        }
    }
}