namespace MonsterAttack
{
    internal class GameLoop
    {
        private IAttackStrategy attackStrategy = new StandardAttackStrategy();

        private List<MonsterClass> possibleMonsterClasses = new List<MonsterClass>();

        private uint attackRounds;

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
      
        private void InitGame()
        {
            InitAttackRounds();
            InitializePossibleMonsters();
        }
        
        private void InitializePossibleMonsters()
        {
            possibleMonsterClasses.Clear();
            foreach (var monsterClass in (MonsterClass[])Enum.GetValues(typeof(MonsterClass)))
            {
                possibleMonsterClasses.Add(monsterClass);   
            }
        }

        private void InitAttackRounds()
        {
            attackRounds = 0;
        }
        
        private void RemoveChoiceFromPossibleMonsterClasses(MonsterClass monsterClass)
        {
            possibleMonsterClasses.Remove(monsterClass);
        }

        private Monster GetMonster()
        {           
            Monster monster = new Monster(attackStrategy);
           
            monster.R = GetMonsterClass();
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

        private void CheckIfFightIsPossible(Monster firstOppenent, Monster secondOppenent)
        {
            if (!this.attackStrategy.isFightPossible(firstOppenent, secondOppenent))
            {
                throw new ImpossibleFightException(Utils.ImpossibleFight);
            }
        }

        private void PrintPossibleMonsterClasses()
        {
            bool isFirstMonsterClass = true;
            Console.Write("Welche Rasse soll das Monster haben (");
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
            Console.Write(message + " ");
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
