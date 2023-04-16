using System.Diagnostics;

namespace TheShittiestChess
{
    internal class Movement
    {
        public static List<Button> removeLater = new List<Button>();
        private static bool IsPostitionOccupied(Position position)
        {
            return MainPage.piecePostions.ContainsKey(MainPage.PositionToString(position));
        } // IsPostitionOccupied
        private static bool IsPieceTakeable(Position position, bool IsThePlayerWhite)
        {
            if (IsPostitionOccupied(position))
                if (MainPage.piecePostions[MainPage.PositionToString(position)].isWhite != IsThePlayerWhite)
                    return true;
            return false;
        } // IsPieceTakeable
        private static void TakePiece(Position enemyPosition, ChessPiece pieceTryingToTake)
        {
            if (IsPieceTakeable(enemyPosition, pieceTryingToTake.isWhite))
            {
                // "killing" the enemy piece
                MainPage.piecePostions[MainPage.PositionToString(enemyPosition)].isAlive = false;
                MainPage.Board.Remove(MainPage.imageButtons[MainPage.piecePostions[MainPage.PositionToString(enemyPosition)].index]);

                MainPage.piecePostions.Remove(MainPage.PositionToString(enemyPosition));


                MovePiece(pieceTryingToTake.position, enemyPosition);
            }
            else
            {
                Debug.WriteLine("The Chosen Piece is not Takeable");
            }
        } // TakePiece
        private static void MovePiece(Position oldPostition, Position newPostition)
        {
            if (!IsPostitionOccupied(newPostition))
            {
                var thePiece = MainPage.piecePostions[MainPage.PositionToString(oldPostition)];
                MainPage.chessPieceche[thePiece.index].position = newPostition;

                MainPage.piecePostions.Remove(MainPage.PositionToString(oldPostition));
                MainPage.piecePostions.Add(MainPage.PositionToString(newPostition), MainPage.chessPieceche[thePiece.index]);

                Grid.SetColumn(MainPage.imageButtons[thePiece.index], newPostition.x);
                Grid.SetRow(MainPage.imageButtons[thePiece.index], newPostition.y);
            }
            else
            {
                Debug.WriteLine("the chosen destination is occupied");
            }
        } // MovePiece
        private static void CheckSurroundings(ChessPiece thePiece, Position positionToCheck)
        {
            if (positionToCheck.x >= 0 && positionToCheck.x <= 7 && positionToCheck.y >= 0 && positionToCheck.y <= 7)
            {
                char ccolor = '?';
                Color color = new Color();
                if (IsPostitionOccupied(positionToCheck))
                {
                    if (IsPieceTakeable(positionToCheck, thePiece.isWhite)) { color = Colors.Red; ccolor = 'R'; }
                    else { color = Colors.Yellow; ccolor = 'Y'; }
                }
                else { color = Colors.Green; ccolor = 'G'; }

                string passOn = string.Format(MainPage.PositionToString(thePiece.position) + "|" + MainPage.PositionToString(positionToCheck));
                Button button = new Button
                {
                    //IsVisible = false,
                    Opacity = 0,
                    ZIndex = 30,
                    ClassId = passOn,

                };
                Button border = new Button
                {
                    BorderColor = color,
                    BorderWidth = 5,
                    ZIndex = 15,
                };
                MainPage.Board.Children.Add(border);
                Grid.SetColumn(border, positionToCheck.x);
                Grid.SetRow(border, positionToCheck.y);
                removeLater.Add(border);

                if (ccolor != 'Y')
                {
                    MainPage.Board.Children.Add(button);
                    Grid.SetColumn(button, positionToCheck.x);
                    Grid.SetRow(button, positionToCheck.y);
                    removeLater.Add(button);
                }

                switch (ccolor)
                {
                    case 'R':
                        button.Clicked += TakePiece_Clicked;
                        break;

                    case 'Y':
                        button.Clicked += SelectNewPiece_Clicked;
                        break;

                    case 'G':
                        button.Clicked += MovePiece_Clicked;
                        break;

                    default:
                        Debug.WriteLine("I dont know how you manged to press a button and yet not press anything");
                        break;
                }
            }
        } // CheckSurroundings
        private static void TakePiece_Clicked(object sender, EventArgs e)
        {
            // setup of information
            var button = sender as Button;
            string[] passedOn = button.ClassId.Split('|');
            ChessPiece thePiece = MainPage.piecePostions[passedOn[0]];
            Position newPosition = new Position(passedOn[1][2] - 48, passedOn[1][7] - 48);

            TakePiece(newPosition, thePiece);


            ClearRemoveLater();

            Debug.WriteLine("The piece Should have moved to a new position");
        } // TakePiece_Clicked
        private static void SelectNewPiece_Clicked(object sender, EventArgs e)
        {
            // this button/method should not be possible to activate
            // setup of information
            var button = sender as Button;
            string[] passedOn = button.ClassId.Split('|');
            ChessPiece thePiece = MainPage.piecePostions[passedOn[0]];
            Position newPosition = new Position(passedOn[1][2] - 48, passedOn[1][7] - 48);

            Debug.WriteLine("how did you activate this button?");
        }
        private static void MovePiece_Clicked(object sender, EventArgs e)
        {
            // setup of information
            var button = sender as Button;
            string[] passedOn = button.ClassId.Split('|');
            ChessPiece thePiece = MainPage.piecePostions[passedOn[0]];
            Position newPosition = new Position(passedOn[1][2] - 48, passedOn[1][7] - 48);

            MovePiece(thePiece.position, newPosition);

            ClearRemoveLater();

            Debug.WriteLine("The piece Should have moved to a new position");
        } // MovePiece_Clicked
        public static void ClearRemoveLater()
        {
            while (removeLater.Count > 0)
            {
                MainPage.Board.Remove(removeLater.First());
                removeLater.RemoveAt(0);
            }
        } // ClearRemoveLater
        public static void KingMover(ChessPiece thePiece)
        {
            //making a array of possible positions the king can move to
            Position[] kingPostions =
            {
                new Position(thePiece.position.x - 1, thePiece.position.y + 1),
                new Position(thePiece.position.x, thePiece.position.y + 1),
                new Position(thePiece.position.x + 1, thePiece.position.y + 1),
                new Position(thePiece.position.x + 1, thePiece.position.y),
                new Position(thePiece.position.x + 1, thePiece.position.y - 1),
                new Position(thePiece.position.x, thePiece.position.y - 1),
                new Position(thePiece.position.x - 1, thePiece.position.y - 1),
                new Position(thePiece.position.x - 1, thePiece.position.y),
            };

            for (int i = 0; i < kingPostions.Length; i++)
            {
                CheckSurroundings(thePiece, kingPostions[i]);
            }
        } // KingMover
        public static void QueenMover(ChessPiece thePiece)
        {
            List<Position> queenPositions = new List<Position>();
            bool[] bools = new bool[8];
            for (int i = 0; i < bools.Length; i++)
                bools[i] = true;

            for (int i = 1; i < 8; i++)
            {
                //diagnol
                if (bools[0])
                {
                    Position temp = new Position(thePiece.position.x + i, thePiece.position.y + i);
                    queenPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[0] = false;
                }
                if (bools[1])
                {
                    Position temp = new Position(thePiece.position.x - i, thePiece.position.y + i);
                    queenPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[1] = false;
                }
                if (bools[2])
                {
                    Position temp = new Position(thePiece.position.x - i, thePiece.position.y - i);
                    queenPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[2] = false;
                }
                if (bools[3])
                {
                    Position temp = new Position(thePiece.position.x + i, thePiece.position.y - i);
                    queenPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[3] = false;
                }

                //linear
                if (bools[4])
                {
                    Position temp = new Position(thePiece.position.x + i, thePiece.position.y);
                    queenPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[4] = false;
                }
                if (bools[5])
                {
                    Position temp = new Position(thePiece.position.x - i, thePiece.position.y);
                    queenPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[5] = false;
                }
                if (bools[6])
                {
                    Position temp = new Position(thePiece.position.x, thePiece.position.y + i);
                    queenPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[6] = false;
                }
                if (bools[7])
                {
                    Position temp = new Position(thePiece.position.x, thePiece.position.y - i);
                    queenPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[7] = false;
                }
            }
            for (int i = 0; i < queenPositions.Count; i++)
            {
                CheckSurroundings(thePiece, queenPositions[i]);
            }


        } // QueenMover
        public static void BishopMover(ChessPiece thePiece)
        {
            List<Position> bishopPositions = new List<Position>();
            bool[] bools = new bool[4];
            for (int i = 0; i < bools.Length; i++)
                bools[i] = true;

            for (int i = 1; i < 8; i++)
            {
                if (bools[0])
                {
                    Position temp = new Position(thePiece.position.x + i, thePiece.position.y + i);
                    bishopPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[0] = false;
                }
                if (bools[1])
                {
                    Position temp = new Position(thePiece.position.x - i, thePiece.position.y + i);
                    bishopPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[1] = false;
                }
                if (bools[2])
                {
                    Position temp = new Position(thePiece.position.x - i, thePiece.position.y - i);
                    bishopPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[2] = false;
                }
                if (bools[3])
                {
                    Position temp = new Position(thePiece.position.x + i, thePiece.position.y - i);
                    bishopPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[3] = false;
                }
            }
            for (int i = 0; i < bishopPositions.Count; i++)
            {
                CheckSurroundings(thePiece, bishopPositions[i]);
            }
        } // BishopMover
        public static void KnightMover(ChessPiece thePiece)
        {
            Position[] knightPositions =
            {
                new Position(thePiece.position.x + 2, thePiece.position.y + 1),
                new Position(thePiece.position.x + 2, thePiece.position.y - 1),
                new Position(thePiece.position.x - 2, thePiece.position.y + 1),
                new Position(thePiece.position.x - 2, thePiece.position.y - 1),
                new Position(thePiece.position.x + 1, thePiece.position.y + 2),
                new Position(thePiece.position.x - 1, thePiece.position.y + 2),
                new Position(thePiece.position.x + 1, thePiece.position.y - 2),
                new Position(thePiece.position.x - 1, thePiece.position.y - 2),
            };
            for (int i = 0; i < knightPositions.Length; i++)
            {
                CheckSurroundings(thePiece, knightPositions[i]);

            }
        } // KnightMover
        public static void RookMover(ChessPiece thePiece)
        {
            List<Position> rookPositions = new List<Position>();
            bool[] bools = new bool[4];
            for (int i = 0; i < bools.Length; i++)
                bools[i] = true;

            for (int i = 1; i < 8; i++)
            {
                if (bools[0])
                {
                    Position temp = new Position(thePiece.position.x + i, thePiece.position.y);
                    rookPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[0] = false;
                }
                if (bools[1])
                {
                    Position temp = new Position(thePiece.position.x - i, thePiece.position.y);
                    rookPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[1] = false;
                }
                if (bools[2])
                {
                    Position temp = new Position(thePiece.position.x, thePiece.position.y + i);
                    rookPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[2] = false;
                }
                if (bools[3])
                {
                    Position temp = new Position(thePiece.position.x, thePiece.position.y - i);
                    rookPositions.Add(temp);
                    if (IsPostitionOccupied(temp))
                        bools[3] = false;
                }
            }
            for (int i = 0; i < rookPositions.Count; i++)
            {
                CheckSurroundings(thePiece, rookPositions[i]);
            }
        } // RookMover
        public static void PawnMover(ChessPiece thePiece)
        {
            List<Position> pawnPostions = new List<Position>();
            int polarrization = 1;
            if (!thePiece.isWhite)
                polarrization = -1;

            // checking the varoius possible postions for pawns
            // checking if the spot straight ahead is takable or not
            Position tempPos1 = new Position(thePiece.position.x, thePiece.position.y + 1 * polarrization);
            if (!IsPieceTakeable(tempPos1, thePiece.isWhite))
                pawnPostions.Add(tempPos1);

            // checking if it can move two spots
            Position tempPos2 = new Position(thePiece.position.x, thePiece.position.y + 2 * polarrization);
            if (!IsPostitionOccupied(tempPos1) && !IsPostitionOccupied(tempPos2))
            {
                if (thePiece.isWhite)
                {
                    if (thePiece.position.y == 1)
                        pawnPostions.Add(tempPos2);
                }
                else
                {
                    if (thePiece.position.y == 6)
                        pawnPostions.Add(tempPos2);
                }

            }

            // attack position to the right
            Position tempPos3 = new Position(thePiece.position.x + 1 * polarrization, thePiece.position.y + 1 * polarrization);
            if (IsPieceTakeable(tempPos3, thePiece.isWhite))
                pawnPostions.Add(tempPos3);

            // attack position to the left
            Position tempPos4 = new Position(thePiece.position.x - 1 * polarrization, thePiece.position.y + 1 * polarrization);
            if (IsPieceTakeable(tempPos4, thePiece.isWhite))
                pawnPostions.Add(tempPos4);


            // checking the possible postions
            for (int i = 0; i < pawnPostions.Count; i++)
            {
                CheckSurroundings(thePiece, pawnPostions[i]);
            }
        } // PawnMover


    }
}
