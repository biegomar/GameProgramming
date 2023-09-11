using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    internal sealed class PlayGround
    {
        private char[,] room;
        private Coordinate playerPosition;
        private Coordinate keyPosition;
        private Coordinate doorPosition;
        private Coordinate dimension;
        private int origRow;
        private int origCol;       

        internal static bool IsRoomDimensionValid(Coordinate dimension)
        {
            int maxDimensionX = 50;
            int maxDimensionY = 25;

            if (dimension.X <= 0 || dimension.Y <= 0)
            {
                Console.WriteLine("Der Raum benötigt eine Breite und eine Höhe > 0.");
                return false;
            }
            else if (dimension.X > maxDimensionX || dimension.Y > maxDimensionY)
            {
                Console.WriteLine($"Der Raum sollte die maximale Breite ({maxDimensionX}) oder die maximale Höhe ({maxDimensionY}) nicht überschreiten.");
                return false;
            }

            return true;
        }

        internal PlayGround(Coordinate dimension)
        {
            this.dimension = dimension;
            this.playerPosition = this.GetPlayerPosition();
            this.doorPosition = this.GetDoorPosition();
            this.keyPosition = this.GetKeyPosition();

            this.room = this.GenerateRoom();

            origRow = Console.CursorTop + 1;
            origCol = Console.CursorLeft;

            this.DrawPlayGround();
        }

        internal void NextStep(ConsoleKeyInfo input)
        {            
            if (input.Key == ConsoleKey.Q)
            {
                throw new QuitException("Ok, das Spiel wird beendet.");
            }

            this.playerPosition = this.CalculateNewPlayerPositionOnValidRules(input.Key, this.playerPosition);
            this.DrawPlayGround();
        }

        private Coordinate CalculateNewPlayerPositionOnValidRules(ConsoleKey inputKey, Coordinate playerPosition)
        {
            int numberOfRows = room.GetLength(1);
            int numberOfColumns = room.GetLength(0);

            switch (inputKey)
            {                
                case ConsoleKey.LeftArrow:
                    {
                        var newPosition = new Coordinate(playerPosition.X - 1, playerPosition.Y);
                        if (newPosition.X == 0)
                        {
                            Console.Beep();
                            return playerPosition;
                        }
                        return newPosition;
                    }                    
                case ConsoleKey.UpArrow:
                    {
                        var newPosition = new Coordinate(playerPosition.X, playerPosition.Y - 1);
                        if (newPosition.Y == 0)
                        {
                            Console.Beep();
                            return playerPosition;
                        }
                        return newPosition;
                    }                    
                case ConsoleKey.RightArrow:
                    {
                        var newPosition = new Coordinate(playerPosition.X + 1, playerPosition.Y);
                        if (newPosition.X == numberOfColumns - 1)
                        {
                            Console.Beep();
                            return playerPosition;
                        }
                        return newPosition;
                    }                    
                case ConsoleKey.DownArrow:
                    {
                        var newPosition = new Coordinate(playerPosition.X, playerPosition.Y + 1);
                        if (newPosition.Y == numberOfRows - 1)
                        {
                            Console.Beep();
                            return playerPosition;
                        }
                        return newPosition;
                    }                                                  
                default:
                    break;
            }

            return playerPosition;
        }

        private void DrawPlayGround()
        {
            UpdatePlayGround();

            Console.SetCursorPosition(origCol, origRow);

            int numberOfRows = room.GetLength(1);
            int numberOfColumns = room.GetLength(0);

            for (int row = 0; row < numberOfRows; row++)
            {
                for (int column = 0; column < numberOfColumns; column++)
                {
                    Console.Write(room[column, row]);
                }
                Console.WriteLine();
            }           
        }

        private void UpdatePlayGround()
        {
            int numberOfRows = room.GetLength(1);
            int numberOfColumns = room.GetLength(0);

            for (int row = 0; row < numberOfRows; row++)
            {
                for (int column = 0; column < numberOfColumns; column++)
                {
                    room[column, row] = GetRoomPositionContent(column, row);                    
                }                
            }
        }

        private Coordinate GetPlayerPosition()
        {
            return new Coordinate(1, 1);
        }

        private Coordinate GetDoorPosition()
        {
            return new Coordinate(0, 0);
        }

        private Coordinate GetKeyPosition()
        {
            return new Coordinate(2, 2);
        }

        private char[,] GenerateRoom()
        {
            var roomDefinition = new char[dimension.X, dimension.Y];

            for (int row = 0; row < dimension.Y; row++)
            {
                for (int column = 0; column < dimension.X; column++)
                {
                    roomDefinition[column, row] = GetRoomPositionContent(column, row);
                }
            }

            return roomDefinition;
        }

        private char GetRoomPositionContent(int column, int row)
        {
            if (column == this.playerPosition.X && row == this.playerPosition.Y)
            {
                return 'ß';
            }
            else if (column == this.doorPosition.X && row == this.doorPosition.Y)
            {
                return 'O';
            }
            else if (column == 0 || column == dimension.X - 1)
            {
                return '|';
            }
            else if (row == 0 || row == dimension.Y - 1)
            {
                return '-';
            }            
            else if (column == this.keyPosition.X && row == this.keyPosition.Y)
            {
                return '§';
            }

            return '.';
        }
    }
}
