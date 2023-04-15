using System;

namespace yestester
{
    internal class Program
    {
        static int widthX = 30;
        static int heightY = 20;
        static int Xplayer = widthX / 2;
        static int Yplayer = heightY / 2;
        static TileType.Types[][] positionType;
        static int tileRareity = 20;
        static float luck = 0.8f;
        static int tilesWalked = 0;
        static int chestsLooted = 0;
        static int banditsDefeated = 0;
        static int bossesSlayed = 0;
        static float bread = 20;
        static Random random = new Random();


        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            positionType = new TileType.Types[widthX + 1][];
            for (int i = 0; i < positionType.Length; i++)
                positionType[i] = new TileType.Types[heightY + 1];

            MakeBoard();




            // game loop
            bool isRunning = true;
            while (isRunning)
            {
                MovementAction();

            }




        } // Main
        static void MovementAction()
        {
            bool didItMove = false;
            ConsoleKeyInfo keyAction = Console.ReadKey(true);

            Console.SetCursorPosition(Xplayer, Yplayer);
            Console.Write('+');

            switch (keyAction.Key)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    if (positionType[Xplayer][Yplayer - 1] != TileType.Types.wall)
                    {
                        Yplayer--;
                        didItMove = true;
                    }
                    break;

                case ConsoleKey.A or ConsoleKey.LeftArrow:
                    if (positionType[Xplayer - 1][Yplayer] != TileType.Types.wall)
                    {
                        Xplayer--;
                        didItMove = true;
                    }
                    break;

                case ConsoleKey.S or ConsoleKey.DownArrow:
                    if (positionType[Xplayer][Yplayer + 1] != TileType.Types.wall)
                    {
                        Yplayer++;
                        didItMove = true;
                    }
                    break;

                case ConsoleKey.D or ConsoleKey.RightArrow:
                    if (positionType[Xplayer + 1][Yplayer] != TileType.Types.wall)
                    {
                        Xplayer++;
                        didItMove = true;
                    }
                    break;

                case ConsoleKey.X:
                    Console.SetCursorPosition(0, heightY + 1);
                    Console.Clear();
                    Console.CursorVisible = true;
                    string[] tmepReadOut = Console.ReadLine().Split(' ');
                    Console.CursorVisible = false;
                    int[] tempReadOut = new int[tmepReadOut.Length];
                    for (int i = 0; i < tmepReadOut.Length; i++)
                    {
                        tempReadOut[i] = int.Parse(tmepReadOut[i]);

                    }
                    Console.WriteLine(positionType[tempReadOut[0]][tempReadOut[1]]);
                    Console.SetCursorPosition(tempReadOut[0], tempReadOut[1]);
                    Console.Write('x');


                    break;
                default:
                    break;
            }
            Console.SetCursorPosition(Xplayer, Yplayer);
            Console.Write('@');
            if (didItMove)
            {
                TileChecker();
                positionType[Xplayer][Yplayer] = TileType.Types.explored;
                tilesWalked++;

            }

            // info and stats
            Console.SetCursorPosition(widthX + 1, 0);
            Console.WriteLine("PlayerPos x:{0}  y:{1} ", Xplayer, Yplayer);
            Console.SetCursorPosition(widthX + 1, 1);
            Console.WriteLine("Bread:{0:f1}", bread);
            Console.SetCursorPosition(widthX + 1, 2);
            Console.WriteLine("Chests Looted:{0}", chestsLooted);
            Console.SetCursorPosition(widthX + 1, 3);
            Console.WriteLine("Bandits Defeated:{0}", banditsDefeated);
            Console.SetCursorPosition(widthX + 1, 4);
            Console.WriteLine("TilesWalked:{0}", tilesWalked);
            Console.SetCursorPosition(widthX + 1, 5);
            Console.WriteLine("MapsExlpored:{0}", bossesSlayed);


        } // MovementAction
        static void TileChecker()
        {
            switch (positionType[Xplayer][Yplayer])
            {
                default:
                    Console.WriteLine("missing value and it is dead");
                    Console.ReadLine();
                    break;

                case TileType.Types.wall:
                    Console.WriteLine("how the fuck did you get here, fix it you dumb fuck");
                    Console.ReadLine();
                    break;

                case TileType.Types.unexplored:
                    bread--;
                    break;

                case TileType.Types.explored:
                    bread -= 0.1f;
                    break;

                case TileType.Types.chest:
                    bread--;
                    bread += random.Next(5, 10) * luck;
                    chestsLooted++;
                    break;

                case TileType.Types.bandits:
                    bread -= 2;
                    banditsDefeated++;
                    break;

                case TileType.Types.boss:
                    bread -= 4;
                    bossesSlayed++;
                    MakeBoard();
                    Console.SetCursorPosition(Xplayer, Yplayer);
                    Console.Write('@');
                    break;


            }


        }
        static void MakeBoard()
        {
            for (int i = 1; i < heightY; i++)
            {
                Console.SetCursorPosition(1, i);
                for (int j = 1; j < widthX; j++)
                {
                    if (1 == random.Next(0, tileRareity))
                    {
                        if (1 == random.Next(0, tileRareity))
                        {
                            Console.Write('¤');
                            positionType[j][i] = TileType.Types.bandits;

                        }
                        else
                        {
                            Console.Write('¤');
                            positionType[j][i] = TileType.Types.chest;

                        }
                    }
                    else
                    {
                        Console.Write('.');
                        positionType[j][i] = TileType.Types.unexplored;

                    }
                }
            }

            int tempX = random.Next(1, widthX - 1);
            int tempY = random.Next(1, heightY - 1);

            Console.SetCursorPosition(tempX, tempY);
            Console.Write('B');
            positionType[tempX][tempY] = TileType.Types.boss;


            Console.SetCursorPosition(0, 0);
            Console.Write('╔');
            Console.SetCursorPosition(widthX, 0);
            Console.Write('╗');
            for (int i = 1; i < widthX; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write('═');
                Console.SetCursorPosition(i, heightY);
                Console.Write('═');

            }


            for (int i = 1; i < heightY; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write('║');
                Console.SetCursorPosition(widthX, i);
                Console.Write('║');
            }
            Console.SetCursorPosition(0, heightY);
            Console.Write('╚');
            Console.SetCursorPosition(widthX, heightY);
            Console.Write('╝');

        } // MakeBoard
        public static (string Name, char Logo, int Int, ConsoleColor color) GetTileTypeInfo(TileType.Types theTile)
        {
            switch (theTile)
            {
                case TileType.Types.wall:
                    return ("wall", 'w', 10, ConsoleColor.White);

                case TileType.Types.unexplored:
                    return ("une", '.', 20, ConsoleColor.DarkGreen);

                case TileType.Types.explored:
                    return ("exp", '+', 30, ConsoleColor.Green);

                case TileType.Types.chest:
                    return ("chest", 'c', 40, ConsoleColor.Yellow);

                case TileType.Types.bandits:
                    return ("band", 'b', 50, ConsoleColor.Magenta);

                case TileType.Types.boss:
                    return ("boss", 'b', 100, ConsoleColor.Red);

                default:
                    return ("null", '?', 0, ConsoleColor.DarkRed);
            }
        } // GetTileTypeInfo
        public static void PlaceHere(int xPos, int yPos, TileType.Types theType)
        {
            var tile = GetTileTypeInfo(theType);
            Console.SetCursorPosition(xPos, yPos);
            Console.ForegroundColor = tile.color;
            Console.Write(tile.Logo);


        }
    } // Program
    internal class Position
    {
        public int row;
        public int col;

        public Position(int r, int c)
        {
            r = row;
            c = col;
        }
    } // Position
    internal class TileType
    {
        public enum Types
        {
            wall,
            unexplored,
            explored,
            chest,
            bandits,
            boss
        }
        public Types theTileType;
        public TileType(Types tileType)
        {
            tileType = theTileType;
        }
    }

}