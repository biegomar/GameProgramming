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
        private readonly Random randomGenerator;

        private readonly char[,] room;

        private Coordinate playerPosition;

        private readonly Coordinate keyPosition;
        private readonly Coordinate doorPosition;
        private readonly Coordinate dimension;

        private const int origRow = 13; //logo size
        private const int origCol = 0;

        private const char playerIcon = 'ß';
        private const char doorIcon = '#';
        private const char keyIcon = '§';
        private const char sideWallIcon = '|';
        private const char topWallIcon = '-';
        private const char groundIcon = '.';

        private const string youWin = "█▓▒▒░░░You win!░░░▒▒▓█";

        private bool isKeyCollected = false;

        private ConsoleColor defaultColor = Console.ForegroundColor;

        internal static bool IsRoomDimensionValid(Coordinate dimension)
        {
            int maxDimensionX = Console.WindowWidth-2;
            int maxDimensionY = Console.WindowHeight-(origRow+2); //take care of the logo size.

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
            this.randomGenerator = new Random();

            this.dimension = dimension;
            this.playerPosition = this.GetPlayerPosition();
            this.doorPosition = this.GetDoorPosition();
            this.keyPosition = this.GetKeyPosition();

            this.room = this.GenerateRoom();                        
        }

        internal void NextStep(ConsoleKeyInfo input)
        {            
            if (input.Key == ConsoleKey.Q)
            {
                throw new QuitException("Ok, das Spiel wird beendet.");
            }

            this.SetNewPlayerPosition(input);

            this.CheckIfYouWin();
            
            this.CheckIfKeyIsCollected();

            this.SetItemColorAndPrint(this.doorPosition, doorIcon);
        }      

        internal void DrawInitialPlayGround()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(origCol, origRow);            

            for (int row = 0; row < this.dimension.Y; row++)
            {
                for (int column = 0; column < this.dimension.X; column++)
                {
                    this.SetItemColor(column, row);
                    Console.Write(room[column, row]);
                    this.ResetItemColor();
                }
                Console.WriteLine();
            }
        }

        private void SetItemColor(int column, int row)
        {           
            ConsoleColor doorColor = this.isKeyCollected ? ConsoleColor.Green : ConsoleColor.Red;

            Console.ForegroundColor = room[column, row] switch
            {
                playerIcon => ConsoleColor.Yellow,
                doorIcon => doorColor,
                keyIcon => ConsoleColor.Blue,
                _ => defaultColor                
            };                        
        }

        private void ResetItemColor()
        {
            Console.ForegroundColor = defaultColor;
        }

        internal void CleanUp()
        {
            Console.CursorVisible = true;
        }

        private void SetNewPlayerPosition(ConsoleKeyInfo input)
        {
            Coordinate oldPlayerPosition = new Coordinate(this.playerPosition.X, this.playerPosition.Y);
            this.playerPosition = this.CalculateNewPlayerPositionOnValidRules(input.Key, this.playerPosition);

            SetItemPositionAndColorAndPrint(oldPlayerPosition, groundIcon);          

            SetItemPositionAndColorAndPrint(this.playerPosition, playerIcon);

            Console.SetCursorPosition(origCol, origRow + this.dimension.Y + 1);
        }

        private void SetItemPositionAndColorAndPrint(Coordinate itemPosition, char itemIcon)
        {
            Console.SetCursorPosition(origCol + itemPosition.X, origRow + itemPosition.Y);
            room[itemPosition.X, itemPosition.Y] = itemIcon;
            Console.Write(itemIcon);
            
            this.SetItemColorAndPrint(itemPosition, itemIcon);
        }

        private void SetItemColorAndPrint(Coordinate itemPosition, char itemIcon)
        {
            this.SetItemColor(itemPosition.X, itemPosition.Y);
            Console.SetCursorPosition(origCol + itemPosition.X, origRow + itemPosition.Y);
            Console.Write(itemIcon);
            this.ResetItemColor();
        }

        private void CheckIfKeyIsCollected()
        {
            this.isKeyCollected = 
                this.playerPosition.X == this.keyPosition.X 
                && this.playerPosition.Y == this.keyPosition.Y
                || this.isKeyCollected;
        }

        private void CheckIfYouWin()
        {
            if (this.isKeyCollected 
                && this.playerPosition.X == this.doorPosition.X
                && this.playerPosition.Y == this.doorPosition.Y)
            {
                throw new WinException(youWin);
            }
        }

        private Coordinate CalculateNewPlayerPositionOnValidRules(ConsoleKey inputKey, Coordinate playerPosition)
        {
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
                        if (newPosition.X == this.dimension.X - 1)
                        {
                            Console.Beep();
                            return playerPosition;
                        }
                        return newPosition;
                    }                    
                case ConsoleKey.DownArrow:
                    {
                        var newPosition = new Coordinate(playerPosition.X, playerPosition.Y + 1);
                        if (newPosition.Y == this.dimension.Y - 1)
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

        private Coordinate GetPlayerPosition()
        {
            int playerPositionX = this.randomGenerator.Next(1, this.dimension.X - 1);
            int playerPositionY = this.randomGenerator.Next(1, this.dimension.Y - 1);

            return new Coordinate(playerPositionX, playerPositionY);
        }

        private Coordinate GetDoorPosition()
        {
            //choose a side (0 = up, 1 = down, 2 = left, 3 = right)
            int side = this.randomGenerator.Next(0,4);
            
            if (side is 0 or 1)
            {
                int doorPositionX = this.randomGenerator.Next(1, this.dimension.X - 1);

                if (side == 0)
                {
                    return new Coordinate(doorPositionX, 0);
                }

                return new Coordinate(doorPositionX, this.dimension.Y - 1);
            }
            else
            {
                int doorPositionY = this.randomGenerator.Next(1, this.dimension.Y - 1);

                if (side == 2)
                {
                    return new Coordinate(0, doorPositionY);
                }

                return new Coordinate(this.dimension.X - 1, doorPositionY);
            }            
        }

        private Coordinate GetKeyPosition()
        {
            int keyPositionX;
            int keyPositionY;

            //player and key should not be placed at the same position.
            do
            {
                keyPositionX = this.randomGenerator.Next(1, this.dimension.X - 1);
                keyPositionY = this.randomGenerator.Next(1, this.dimension.Y - 1);
            } while (keyPositionX == this.playerPosition.X && keyPositionY == this.playerPosition.Y);

            return new Coordinate(keyPositionX, keyPositionY);
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
                return playerIcon;
            }
            else if (column == this.doorPosition.X && row == this.doorPosition.Y)
            {
                return doorIcon;
            }
            else if (column == 0 || column == dimension.X - 1)
            {
                return sideWallIcon;
            }
            else if (row == 0 || row == dimension.Y - 1)
            {
                return topWallIcon;
            }            
            else if (column == this.keyPosition.X && row == this.keyPosition.Y)
            {
                return keyIcon;
            }

            return groundIcon;
        }
    }
}
