using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using _Scripts.NPC_Scripts;
using UnityEngine;

namespace _Scripts.Command
{
    public static class CommandInvoker
    {
        private static Queue<Command> _commands = new Queue<Command>();

        public static int NumberOfCommands()
        {
            return _commands.Count;
        }

        public static Queue<Command> CommandsToExecute()
        {
            return _commands;
        }

        public static bool IsAlreadyQueued(GameObject npc)
        {
            foreach (Command t in _commands)
            {
                if (t.GetCommandUnit() ==  npc)
                {
                    return true;
                }
            }
            
            return false;
        }

        public static void AddCommand(Command command)
        {
            _commands.Enqueue(command);
        }

        public static void Clear()
        {
            _commands.Clear();
            _commands = new Queue<Command>();
        }


        public static IEnumerator ExecuteOurCommands()
        {
            foreach (Command t in _commands.ToList())
            {
                if ( t.GetEnemyUnit() != null && t.GetEnemyUnit().GetComponent<Npc>().IsDead)
                {
                    continue;
                }

                t.Execute();
                yield return new WaitUntil(() => t.IsTaskStillRunning() == false);
                t.ExecuteReturnCommand();
                yield return new WaitUntil(() => t.IsTaskStillRunning() == false);
            }
            Clear();
            GameEnvironment.FixTurn(false);
        }
        


        public static IEnumerator ExecuteEnemyCommands1()
        {

            foreach (Npc enemy in GameEnvironment.EnemyTeamNpc)
            {
                if(enemy.IsDead)continue;
                
                enemy.MovingToTargetAttack = true;
                enemy.AbilityName = enemy.npcInfo.hasAbilities[0];
                GameObject target = GameEnvironment.EnemyTargetUnit();
                if (target == null)
                {
                    Clear();
                    GameEnvironment.FixTurn(true);
                    break;
                }
                AddCommand(new PerformAnAttack(enemy.gameObject, target,
                    enemy.npcInfo.hasAbilities[0]));
                Command cmd = new PerformAnAttack(enemy.gameObject, target,
                    enemy.npcInfo.hasAbilities[0]);
                cmd.Execute();
                
                yield return new WaitUntil(() => cmd.IsTaskStillRunning() == false);
                cmd.ExecuteReturnCommand();
                yield return new WaitUntil(() => cmd.IsTaskStillRunning() == false);
            }
            
            Clear();
            GameEnvironment.FixTurn(true);
            GameEnvironment.ourTurnGUI.SetActive(true);

        }
    }
}