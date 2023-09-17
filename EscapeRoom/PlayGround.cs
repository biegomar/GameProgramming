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

        private bool isKeyCollected = false;

        private ConsoleColor defaultColor = Console.ForegroundColor;

        internal static bool IsRoomDimensionValid(Coordinate dimension)
        {
            int maxDimensionX = Console.WindowWidth-2;
            int maxDimensionY = Console.WindowHeight-(Utils.OriginalRowPosition+2); //take care of the logo size.

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
            randomGenerator = new Random();

            this.dimension = dimension;
            playerPosition = GetPlayerPosition();
            doorPosition = GetDoorPosition();
            keyPosition = GetKeyPosition();

            room = GenerateRoom();                        
        }

        internal void NextStep(ConsoleKeyInfo input)
        {
            CheckIfPlayerWantsToQuit(input);
            SetNewPlayerPositionAndPrint(input);
            CheckIfYouWin();
            CheckIfKeyIsCollected();
            SetItemColorAndPrint(doorPosition, Utils.DoorIcon);
        }

        internal void CleanUp()
        {
            Console.CursorVisible = true;
        }
        
        private void SetCursorBelowPlayGround()
        {
            Console.SetCursorPosition(0, Utils.OriginalRowPosition + dimension.Y + 1);
        }

        internal void DrawInitialPlayGround()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(Utils.OriginalColumnPosition, Utils.OriginalRowPosition);            

            for (int row = 0; row < dimension.Y; row++)
            {
                for (int column = 0; column < dimension.X; column++)
                {
                    SetItemColor(column, row);
                    Console.Write(room[column, row]);
                    ResetItemColor();
                }
                Console.WriteLine();
            }
        }

        private void SetItemColor(int column, int row)
        {           
            ConsoleColor doorColor = isKeyCollected ? ConsoleColor.Green : ConsoleColor.Red;

            Console.ForegroundColor = room[column, row] switch
            {
                Utils.PlayerIcon => ConsoleColor.Yellow,
                Utils.DoorIcon => doorColor,
                Utils.KeyIcon => ConsoleColor.Blue,
                _ => defaultColor                
            };                        
        }

        private void ResetItemColor()
        {
            Console.ForegroundColor = defaultColor;
        }

        private void SetNewPlayerPositionAndPrint(ConsoleKeyInfo input)
        {
            Coordinate oldPlayerPosition = new Coordinate(playerPosition.X, playerPosition.Y);
            playerPosition = CalculateNewPlayerPositionOnValidRules(input.Key, playerPosition);

            SetItemPositionAndColorAndPrint(oldPlayerPosition, Utils.GroundIcon);          

            SetItemPositionAndColorAndPrint(playerPosition, Utils.PlayerIcon);

            Console.SetCursorPosition(Utils.OriginalColumnPosition, Utils.OriginalRowPosition + dimension.Y + 1);
        }

        private void SetItemPositionAndColorAndPrint(Coordinate itemPosition, char itemIcon)
        {
            Console.SetCursorPosition(Utils.OriginalColumnPosition + itemPosition.X, Utils.OriginalRowPosition + itemPosition.Y);
            room[itemPosition.X, itemPosition.Y] = itemIcon;
            Console.Write(itemIcon);
            
            SetItemColorAndPrint(itemPosition, itemIcon);
        }

        private void SetItemColorAndPrint(Coordinate itemPosition, char itemIcon)
        {
            SetItemColor(itemPosition.X, itemPosition.Y);
            Console.SetCursorPosition(Utils.OriginalColumnPosition + itemPosition.X, Utils.OriginalRowPosition + itemPosition.Y);
            Console.Write(itemIcon);
            ResetItemColor();
        }

        private void CheckIfKeyIsCollected()
        {
            isKeyCollected = 
                playerPosition.X == keyPosition.X 
                && playerPosition.Y == keyPosition.Y
                || isKeyCollected;
        }

        private void CheckIfYouWin()
        {
            if (isKeyCollected 
                && playerPosition.X == doorPosition.X
                && playerPosition.Y == doorPosition.Y)
            {
                SetCursorBelowPlayGround();
                throw new WinException(Utils.YouWin);
            }
        }
        private void CheckIfPlayerWantsToQuit(ConsoleKeyInfo input)
        {
            if (input.Key == ConsoleKey.Q)
            {
                SetCursorBelowPlayGround();
                throw new QuitException("Ok, das Spiel wird beendet.");
            }
        }

        private Coordinate CalculateNewPlayerPositionOnValidRules(ConsoleKey inputKey, Coordinate playerPosition)
        {
            bool IsPlayerOnOpenDoor(Coordinate positionToCheck)
            {
                if(isKeyCollected 
                    && positionToCheck.X == doorPosition.X 
                    && positionToCheck.Y == doorPosition.Y)
                {
                    return true;
                }

                return false;
            }

            switch (inputKey)
            {               
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    {
                        var newPosition = new Coordinate(playerPosition.X, playerPosition.Y - 1);
                        if (newPosition.Y == 0 && !IsPlayerOnOpenDoor(newPosition))
                        {
                            Utils.BeepOnWrongEntry();
                            return playerPosition;
                        }
                        return newPosition;
                    }
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    {
                        var newPosition = new Coordinate(playerPosition.X, playerPosition.Y + 1);
                        if (newPosition.Y == dimension.Y - 1 && !IsPlayerOnOpenDoor(newPosition))
                        {
                            Utils.BeepOnWrongEntry();
                            return playerPosition;
                        }
                        return newPosition;
                    }
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    {
                        var newPosition = new Coordinate(playerPosition.X + 1, playerPosition.Y);
                        if (newPosition.X == dimension.X - 1 && !IsPlayerOnOpenDoor(newPosition))
                        {
                            Utils.BeepOnWrongEntry();
                            return playerPosition;
                        }
                        return newPosition;
                    }
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    {
                        var newPosition = new Coordinate(playerPosition.X - 1, playerPosition.Y);
                        if (newPosition.X == 0 && !IsPlayerOnOpenDoor(newPosition))
                        {
                            Utils.BeepOnWrongEntry();
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
            int playerPositionX = randomGenerator.Next(1, dimension.X - 1);
            int playerPositionY = randomGenerator.Next(1, dimension.Y - 1);

            return new Coordinate(playerPositionX, playerPositionY);
        }

        private Coordinate GetDoorPosition()
        {
            //choose a side (0 = up, 1 = down, 2 = left, 3 = right)
            int side = randomGenerator.Next(0,4);
            
            if (side is 0 or 1)
            {
                int doorPositionX = randomGenerator.Next(1, dimension.X - 1);

                if (side == 0)
                {
                    return new Coordinate(doorPositionX, 0);
                }

                return new Coordinate(doorPositionX, dimension.Y - 1);
            }

            int doorPositionY = randomGenerator.Next(1, dimension.Y - 1);

            if (side == 2)
            {
                return new Coordinate(0, doorPositionY);
            }

            return new Coordinate(dimension.X - 1, doorPositionY);
        }

        private Coordinate GetKeyPosition()
        {
            int keyPositionX;
            int keyPositionY;

            //player and key should not be placed at the same position.
            do
            {
                keyPositionX = randomGenerator.Next(1, dimension.X - 1);
                keyPositionY = randomGenerator.Next(1, dimension.Y - 1);
            } while (keyPositionX == playerPosition.X && keyPositionY == playerPosition.Y);

            return new Coordinate(keyPositionX, keyPositionY);
        }

        private char[,] GenerateRoom()
        {
            char[,] roomDefinition = new char[dimension.X, dimension.Y];

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
            if (column == playerPosition.X && row == playerPosition.Y)
            {
                return Utils.PlayerIcon;
            }

            if (column == doorPosition.X && row == doorPosition.Y)
            {
                return Utils.DoorIcon;
            }

            if (column == 0 || column == dimension.X - 1)
            {
                return Utils.SideWallIcon;
            }
            if (row == 0 || row == dimension.Y - 1)
            {
                return Utils.TopWallIcon;
            }
            if (column == keyPosition.X && row == keyPosition.Y)
            {
                return Utils.KeyIcon;
            }

            return Utils.GroundIcon;
        }       
    }
}
