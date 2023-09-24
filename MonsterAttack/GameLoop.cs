using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonsterAttack
{
    internal class GameLoop
    {
        private IAttackStrategy attackStrategy = new StandardAttackStrategy();

        private List<Race> possibleMonsters = new List<Race>();

        private uint attackRounds;

        internal void Run()
        {            
            // Outer loop - let me easily "play another round"
            do
            {

                Init();

                Monster firstOppenent = GetMonster();
                RemoveChoiceFromPossibleMonsters(firstOppenent.R);                
                
                Monster secondOppenent = GetMonster();
                
                (firstOppenent, secondOppenent) = SortMonstersBySpeed(firstOppenent, secondOppenent);                

                PrintStart();

                try
                {
                    CheckIfFightIsPossible(firstOppenent, secondOppenent);

                    // Core game loop!
                    do
                    {
                        attackRounds++;
                        firstOppenent.Attack(secondOppenent);
                        secondOppenent.Attack(firstOppenent);
                    } while (true);
                }
                catch (KillException ex)
                {
                    PrintNumberOfAttackRounds();

                    if (!WantToContinueGame(ex.Message))
                    {
                        Environment.Exit(0);
                    }
                }
                catch (ImpossibleFightException ex)
                {
                    if (!WantToContinueGame(ex.Message))
                    {
                        Environment.Exit(0);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(0);
                }

            } while (true);
        }
      
        private void Init()
        {
            InitAttacksRounds();
            InitializePossibleMonsters();
        }
        
        private void InitializePossibleMonsters()
        {
            possibleMonsters.Clear();
            foreach (var race in (Race[])Enum.GetValues(typeof(Race)))
            {
                possibleMonsters.Add(race);   
            }
        }

        private void InitAttacksRounds()
        {
            attackRounds = 0;
        }
        
        private void RemoveChoiceFromPossibleMonsters(Race race)
        {
            possibleMonsters.Remove(race);
        }

        private Monster GetMonster()
        {           
            Monster monster = new Monster(attackStrategy);
           
            monster.R = GetMonsterRace();
            monster.HP = GetPropertyValueForMonster(Utils.HealthPoints);
            monster.AP = GetPropertyValueForMonster(Utils.AttackPoints);
            monster.DP = GetPropertyValueForMonster(Utils.DefensePoints);
            monster.S = GetPropertyValueForMonster(Utils.SpeedPoints);           

            return monster;
        } 

        private (Monster fasterMonster, Monster slowerMonster) SortMonstersBySpeed(Monster firstOppenent, Monster secondOppenent)
        {
            if (firstOppenent.S >= secondOppenent.S) 
            {
                return (firstOppenent, secondOppenent);
            }

            return (secondOppenent,  firstOppenent);
        }

        private Race GetMonsterRace() 
        {
            Race race;
            PrintPossibleMonsters();           
            while (!Enum.TryParse(Console.ReadLine(), out race) || !possibleMonsters.Contains(race))
            {
                Console.WriteLine(Utils.ErrorMessageUnknownRace);
                PrintPossibleMonsters();
            }

            return race;
        }

        private void CheckIfFightIsPossible(Monster firstOppenent, Monster secondOppenent)
        {
            if (!this.attackStrategy.isFightPossible(firstOppenent, secondOppenent))
            {
                throw new ImpossibleFightException(Utils.ImpossibleFight);
            }
        }

        private void PrintPossibleMonsters()
        {
            bool firstRace = true;
            Console.Write("Welche Rasse soll das Monster haben (");
            foreach (var race in possibleMonsters)
            {
                if (firstRace)
                {
                    Console.Write($"{(int)race} - {race}");
                    firstRace = false;
                }
                else
                {
                    Console.Write($", {(int)race} - {race}");
                }
                
            }
            Console.Write(")? ");
        }
        
        private void PrintNumberOfAttackRounds()
        {
            Console.WriteLine(Utils.AttackRounds, this.attackRounds);
        }

        private static void PrintStart()
        {
            Console.WriteLine();
            Console.WriteLine(Utils.StartMessage);
        }

        private float GetPropertyValueForMonster(string message)
        {
            float result;
            Console.Write(message);
            while (!float.TryParse(Console.ReadLine(), out result) || result <= 0f)
            {
                Console.WriteLine(Utils.ErrorMessageNumberIsZero);
                Console.Write(message);
            }
            
            return result;
        }

        private bool WantToContinueGame(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine(Utils.WantToContinueGame);
            do 
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key is ConsoleKey.Y or ConsoleKey.J)
                {
                    Console.WriteLine();
                    return true;
                }

                if (key == ConsoleKey.N)
                {
                    return false;
                }

                Utils.BeepOnWrongEntry();
            } while (true);
        }
    }
}
