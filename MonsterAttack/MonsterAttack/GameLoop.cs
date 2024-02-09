using System.IO;

namespace MonsterAttack
{
    /// <summary>
    /// Central GameLoop.
    /// </summary>
    internal class GameLoop
    {
        /// <summary>
        /// The attack strategy for all monsters.
        /// Only one is implemented so far, so no dynamic mechanism (e.g. DI-Container) is needed.
        /// </summary>
        private IAttackStrategy attackStrategy = new StandardAttackStrategy();

        private List<MonsterClass> possibleMonsterClasses = new();

        private uint attackRounds;

        /// <summary>
        /// Real entry point of the game.
        /// </summary>
        internal void Run()
        {            
            // Outer loop - let me easily "play another round"
            do
            {

                InitGame();

                Monster firstOppenent = GetMonster();
                RemoveChoiceFromPossibleMonsterClasses(firstOppenent.R);                
                
                Monster secondOppenent = GetMonster();
                
                (firstOppenent, secondOppenent) = SortMonstersBySpeed(firstOppenent, secondOppenent);                

                Utils.PrintStart(firstOppenent, secondOppenent);

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
      
        /// <summary>
        /// Initialize the game settings.
        /// </summary>
        private void InitGame()
        {
            InitAttackRounds();
            InitializePossibleMonsters();
        }
        
        /// <summary>
        /// Prepare the list of possible monster classes to use in the game.
        /// </summary>
        private void InitializePossibleMonsters()
        {
            possibleMonsterClasses = Enum.GetValues(typeof(MonsterClass)).Cast<MonsterClass>().ToList();
        }

        /// <summary>
        /// Initialize the attack round counter.
        /// </summary>
        private void InitAttackRounds()
        {
            attackRounds = 0;
        }
        
        /// <summary>
        /// Reduce the monster class choice by the specified value.
        /// </summary>
        /// <param name="monsterClass"></param>
        private void RemoveChoiceFromPossibleMonsterClasses(MonsterClass monsterClass)
        {
            possibleMonsterClasses.Remove(monsterClass);
        }

        /// <summary>
        /// Gets a new monster.
        /// </summary>
        /// <returns>The new monster.</returns>
        private Monster GetMonster()
        {           
            Monster monster = new Monster(attackStrategy);
           
            monster.R = GetMonsterClass();
            monster.HP = GetPropertyValueForMonster(Utils.HealthPoints);
            monster.AP = GetPropertyValueForMonster(Utils.AttackPoints);
            monster.DP = GetPropertyValueForMonster(Utils.DefensePoints);
            monster.S = GetPropertyValueForMonster(Utils.SpeedPoints);
            monster.Avatar = Utils.GetAvatarForMonsterClass(monster.R);

            return monster;
        } 

        /// <summary>
        /// Sort the two monsters according to their speed.
        /// </summary>
        /// <param name="firstOpponent">The first monster.</param>
        /// <param name="secondOpponent">The second monster.</param>
        /// <returns>A tuple in sorted order.</returns>
        private (Monster fasterMonster, Monster slowerMonster) SortMonstersBySpeed(Monster firstOpponent, Monster secondOpponent)
        {
            if (firstOpponent.S >= secondOpponent.S) 
            {
                return (firstOpponent, secondOpponent);
            }

            return (secondOpponent,  firstOpponent);
        }

        /// <summary>
        /// Asks the player for a specific monster property.
        /// </summary>
        /// <param name="message">The message to display to the player.</param>
        /// <returns>The players choice.</returns>
        private float GetPropertyValueForMonster(string message)
        {
            float result;
            Console.Write(message + " ");
            while (!float.TryParse(Console.ReadLine(), out result) || result <= 0f)
            {
                Console.WriteLine(Utils.ErrorMessageNumberIsZero);
                Console.Write(message);
            }
            
            return result;
        }    
        
        /// <summary>
        /// Asks the player for a monster class.
        /// </summary>
        /// <returns>The players choice.</returns>
        private MonsterClass GetMonsterClass() 
        {
            MonsterClass monsterClass;
            PrintPossibleMonsterClasses();           
            while (!Enum.TryParse(Console.ReadLine(), out monsterClass) || !possibleMonsterClasses.Contains(monsterClass))
            {
                Console.WriteLine(Utils.ErrorMessageEnterValidNumber);
                PrintPossibleMonsterClasses();
            }

            return monsterClass;
        }
        
        /// <summary>
        /// Checks if the fight is possible.
        /// The call is delegated to the attack strategy.
        /// </summary>
        /// <param name="firstOppenent">The first monster.</param>
        /// <param name="secondOppenent">The second monster.</param>
        /// <exception cref="ImpossibleFightException">Is thrown if the fight is impossible.</exception>
        private void CheckIfFightIsPossible(Monster firstOppenent, Monster secondOppenent)
        {
            if (!this.attackStrategy.IsFightPossible(firstOppenent, secondOppenent))
            {
                throw new ImpossibleFightException(Utils.ImpossibleFight);
            }
        }

        /// <summary>
        /// Prints all available monster classes for the player to choose from.
        /// </summary>
        private void PrintPossibleMonsterClasses()
        {
            bool isFirstMonsterClass = true;
            Console.Write($"{Utils.WhichRace} (");
            foreach (var monsterClass in possibleMonsterClasses)
            {
                if (isFirstMonsterClass)
                {
                    Console.Write($"{(int)monsterClass} - {monsterClass}");
                    isFirstMonsterClass = false;
                }
                else
                {
                    Console.Write($", {(int)monsterClass} - {monsterClass}");
                }
                
            }
            Console.Write(")? ");
        }
        
        /// <summary>
        /// Prints the actual number of attack rounds.
        /// </summary>
        private void PrintNumberOfAttackRounds()
        {
            Console.WriteLine(Utils.AttackRounds, this.attackRounds);
        }        
        
        /// <summary>
        /// Ask the player whether he wants to continue the game.
        /// </summary>
        /// <param name="message">The message to display to the player.</param>
        /// <returns>True if the player wants to continue, else false.</returns>
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
