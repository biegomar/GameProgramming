using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonsterAttack
{
    internal static class GameLoop
    {
        private static IAttackStrategy attackStrategy = new StandardAttackStrategy();

        internal static void Run()
        {
            // Outer loop - let me easily "play another round"
            do
            {
                Monster firstOppenent = GetMonster();
                Console.WriteLine();
                Monster secondOppenent = GetMonster();

                (firstOppenent, secondOppenent) = SortMontersBySpeed(firstOppenent, secondOppenent);

                Console.WriteLine();
                Console.WriteLine("------ Auf gehts! -------");

                try
                {
                    // Core game loop!
                    do
                    {
                        firstOppenent.Attack(secondOppenent);
                        secondOppenent.Attack(firstOppenent);
                    } while (true);
                }
                catch (KillException ex)
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

        private static Monster GetMonster()
        {           
            Monster monster = new Monster(attackStrategy);
           
            monster.R = GetMonsterRace();
            monster.HP = GetPropertyValueForMonster(Utils.HealthPoints);
            monster.AP = GetPropertyValueForMonster(Utils.AttackPoints);
            monster.DP = GetPropertyValueForMonster(Utils.DefensePoints);
            monster.S = GetPropertyValueForMonster(Utils.SpeedPoints);           

            return monster;
        } 

        private static (Monster fasterMonster, Monster slowerMonster) SortMontersBySpeed(Monster firstOppenent, Monster secondOppenent)
        {
            if (firstOppenent.S >= secondOppenent.S) 
            {
                return (firstOppenent, secondOppenent);
            }

            return (secondOppenent,  firstOppenent);
        }

        private static Race GetMonsterRace() 
        {
            int race;
            Console.Write(Utils.MonsterRace);
            while (!int.TryParse(Console.ReadLine(), out race) || race < 1 || race > 3)
            {
                Console.WriteLine(Utils.ErrorMessageUnknownRace);
                Console.Write(Utils.MonsterRace);
            }

            return (Race)race;
        }
        
        private static float GetPropertyValueForMonster(string message)
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

        private static bool WantToContinueGame(string message)
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
