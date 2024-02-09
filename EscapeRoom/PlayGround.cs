namespace EscapeRoom
{
    /// <summary>
    /// This is the main class of the game. It represents the playground where the room is painted and the character can move.
    /// </summary>
    internal sealed class PlayGround
    {
        private readonly Random randomGenerator;
        private readonly char[,] room;
        private readonly Vector2D keyPosition;
        private readonly Vector2D doorPosition;
        private readonly Vector2D dimension;
        
        private Vector2D playerPosition;
        private bool isKeyCollected;
        private ConsoleColor defaultColor = Console.ForegroundColor;

        /// <summary>
        /// Check for valid room dimensions.
        /// The room itself should know its limitations... 
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns>True if the dimensions are valid, false otherwise.</returns>
        internal static bool IsRoomDimensionValid(Vector2D dimension)
        {
            int maxDimensionX = Console.WindowWidth-2;
            int maxDimensionY = Console.WindowHeight-(Utils.OriginalRowPosition+2); //take care of the logo size.

            if (dimension.X <= 0 || dimension.Y <= 0)
            {
                Console.WriteLine(Utils.EnterRightPlayGroundDimension);
                return false;
            }
            else if (dimension.X > maxDimensionX || dimension.Y > maxDimensionY)
            {
                Console.Write(Utils.RoomDimensionNotValid, maxDimensionX, maxDimensionY);
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="dimension"></param>
        internal PlayGround(Vector2D dimension)
        {
            randomGenerator = new Random();

            this.dimension = dimension;
            playerPosition = GetRandomPlayerPosition();
            doorPosition = GetDoorPosition();
            keyPosition = GetKeyPosition();

            room = GenerateRoom();                        
        }

        /// <summary>
        /// Executes the player's next move.
        /// </summary>
        /// <param name="input">The input key.</param>
        internal void NextStep(ConsoleKeyInfo input)
        {
            CheckIfPlayerWantsToQuit(input);
            SetNewPlayerPositionAndPrint(input);
            CheckIfPlayerWins();
            CheckIfKeyIsCollected();
            SetItemColorAndPrint(doorPosition, Utils.DoorIcon);
        }

        /// <summary>
        /// Clean up of the play ground specific settings.
        /// </summary>
        internal void CleanUp()
        {
            Console.CursorVisible = true;
        }

        /// <summary>
        /// Draws the playing ground after generation. This is the only time that the room is completely drawn.
        /// After that, only the player's movement takes place.
        /// </summary>
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
        
        /// <summary>
        /// Set the cursor position below the play ground.
        /// </summary>
        private void SetCursorBelowPlayGround()
        {
            Console.SetCursorPosition(0, Utils.OriginalRowPosition + dimension.Y + 1);
        }

        /// <summary>
        /// Sets the specific item color of the item that is located under the given coordinate.
        /// </summary>
        /// <param name="row">The row</param>
        /// <param name="column">The column</param>
        private void SetItemColor(int column, int row)
        {           
            Console.ForegroundColor = room[column, row] switch
            {
                Utils.PlayerIcon => ConsoleColor.Yellow,
                Utils.DoorIcon => isKeyCollected ? ConsoleColor.Green : ConsoleColor.Red,
                Utils.KeyIcon => ConsoleColor.Blue,
                _ => defaultColor                
            };                        
        }

        /// <summary>
        /// Resets the foreground color.
        /// </summary>
        private void ResetItemColor()
        {
            Console.ForegroundColor = defaultColor;
        }

        /// <summary>
        /// Sets the new play position and print the player icon.
        /// </summary>
        /// <param name="input">The input key.</param>
        private void SetNewPlayerPositionAndPrint(ConsoleKeyInfo input)
        {
            Vector2D oldPlayerPosition = new (playerPosition.X, playerPosition.Y);
            playerPosition = CalculateNewPlayerPositionOnValidRules(input.Key);

            //clears the old player position on playground.
            SetItemPositionAndColorAndPrint(oldPlayerPosition, Utils.GroundIcon);          

            //spawns the player at his new position.
            SetItemPositionAndColorAndPrint(playerPosition, Utils.PlayerIcon);
            
            Console.SetCursorPosition(Utils.OriginalColumnPosition, Utils.OriginalRowPosition + dimension.Y + 1);
        }

        /// <summary>
        /// Sets the new position and the color of the itemIcon and prints it into the playground.
        /// </summary>
        /// <param name="itemPosition">The new position.</param>
        /// <param name="itemIcon">The icon of the item to print.</param>
        private void SetItemPositionAndColorAndPrint(Vector2D itemPosition, char itemIcon)
        {
            Console.SetCursorPosition(Utils.OriginalColumnPosition + itemPosition.X, Utils.OriginalRowPosition + itemPosition.Y);
            
            //maybe an own method...
            room[itemPosition.X, itemPosition.Y] = itemIcon;
            Console.Write(itemIcon);
            
            SetItemColorAndPrint(itemPosition, itemIcon);
        }

        /// <summary>
        /// Sets the new position and color of the itemIcon and prints it in the Playground.
        /// </summary>
        /// <param name="itemPosition"></param>
        /// <param name="itemIcon"></param>
        private void SetItemColorAndPrint(Vector2D itemPosition, char itemIcon)
        {
            SetItemColor(itemPosition.X, itemPosition.Y);
            Console.SetCursorPosition(Utils.OriginalColumnPosition + itemPosition.X, Utils.OriginalRowPosition + itemPosition.Y);
            Console.Write(itemIcon);
            ResetItemColor();
        }

        /// <summary>
        /// Has the player already found the key?
        /// </summary>
        private void CheckIfKeyIsCollected()
        {
            isKeyCollected = 
                playerPosition.X == keyPosition.X 
                && playerPosition.Y == keyPosition.Y
                || isKeyCollected;
        }

        /// <summary>
        /// Has the player won?
        /// </summary>
        /// <exception cref="WinException"></exception>
        private void CheckIfPlayerWins()
        {
            if (isKeyCollected 
                && playerPosition.X == doorPosition.X
                && playerPosition.Y == doorPosition.Y)
            {
                SetCursorBelowPlayGround();
                throw new WinException(Utils.YouWin);
            }
        }
        
        /// <summary>
        /// Does the player want to quit the game?
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="QuitException"></exception>
        private void CheckIfPlayerWantsToQuit(ConsoleKeyInfo input)
        {
            if (input.Key == ConsoleKey.Q)
            {
                SetCursorBelowPlayGround();
                throw new QuitException(Utils.QuitGame);
            }
        }

        /// <summary>
        /// Interprets the inputKey value and returns the new position on this basis.
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns>The new position vector.</returns>
        private Vector2D CalculateNewPlayerPositionOnValidRules(ConsoleKey inputKey)
        {
            switch (inputKey)
            {               
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    {                        
                        var newPosition = playerPosition with { Y = playerPosition.Y - 1 };
                        return GetNewPlayerPosition(newPosition.Y == 0 && !IsPlayerOnOpenDoor(newPosition), newPosition);                        
                    }
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    {
                        var newPosition = playerPosition with { Y = playerPosition.Y + 1 };
                        return GetNewPlayerPosition(newPosition.Y == dimension.Y - 1 && !IsPlayerOnOpenDoor(newPosition), newPosition);                        
                    }
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    {
                        var newPosition = playerPosition with { X = playerPosition.X + 1 };
                        return GetNewPlayerPosition(newPosition.X == dimension.X - 1 && !IsPlayerOnOpenDoor(newPosition), newPosition);                        
                    }
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    {
                        var newPosition = playerPosition with { X = playerPosition.X - 1 };
                        return GetNewPlayerPosition(newPosition.X == 0 && !IsPlayerOnOpenDoor(newPosition), newPosition);                        
                    }                
                default:
                    break;
            }

            return playerPosition;

            //local functions should be placed behind the return statement. (Rider told me...)
            Vector2D GetNewPlayerPosition(bool decision, Vector2D newPosition)
            {
                if (decision)
                {
                    Utils.BeepOnWrongEntry();
                    return playerPosition;
                }
                return newPosition;
            }

            bool IsPlayerOnOpenDoor(Vector2D positionToCheck)
            {
                if(isKeyCollected 
                   && positionToCheck.X == doorPosition.X 
                   && positionToCheck.Y == doorPosition.Y)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Randomly sets the initial player position.
        /// </summary>
        /// <returns>The position vector.</returns>
        private Vector2D GetRandomPlayerPosition()
        {
            int playerPositionX = randomGenerator.Next(1, dimension.X - 1);
            int playerPositionY = randomGenerator.Next(1, dimension.Y - 1);

            return new Vector2D(playerPositionX, playerPositionY);
        }

        /// <summary>
        /// Randomly sets the door position.
        /// </summary>
        /// <returns>The door position vector.</returns>
        private Vector2D GetDoorPosition()
        {
            //choose a side (0 = up, 1 = down, 2 = left, 3 = right)
            int side = randomGenerator.Next(0,4);
            
            if (side is 0 or 1)
            {
                int doorPositionX = randomGenerator.Next(1, dimension.X - 1);

                return side == 0 ? new Vector2D(doorPositionX, 0) : new Vector2D(doorPositionX, dimension.Y - 1);
            }

            int doorPositionY = randomGenerator.Next(1, dimension.Y - 1);

            return side == 2 ? new Vector2D(0, doorPositionY) : new Vector2D(dimension.X - 1, doorPositionY);
        }

        /// <summary>
        /// Randomly sets the key position. Prevents the key from appearing in the same position as the player.
        /// </summary>
        /// <returns>The key position vector.</returns>
        private Vector2D GetKeyPosition()
        {
            int keyPositionY;
            int keyPositionX;

            //player and key should not be placed at the same position.
            do
            {
                keyPositionX = randomGenerator.Next(1, dimension.X - 1);
                keyPositionY = randomGenerator.Next(1, dimension.Y - 1);
            } while (keyPositionX == playerPosition.X && keyPositionY == playerPosition.Y);

            return new Vector2D(keyPositionX, keyPositionY);
        }

        /// <summary>
        /// Generates the room.
        /// </summary>
        /// <returns>A char array.</returns>
        private char[,] GenerateRoom()
        {
            char[,] roomDefinition = new char[dimension.X, dimension.Y];

            for (int column = 0; column < dimension.X; column++)               
            {
                for (int row = 0; row < dimension.Y; row++)
                {
                    roomDefinition[column, row] = GetRoomPositionContent(column, row);
                }
            }            

            return roomDefinition;
        }

        /// <summary>
        /// Gets the symbol that can be found at the given position.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="row">The row.</param>
        /// <returns></returns>
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
