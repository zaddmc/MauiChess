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

                // removes the old allocated postion in the dictionary and allocates the new position
                MainPage.piecePostions.Remove(MainPage.PositionToString(oldPostition));
                MainPage.piecePostions.Add(MainPage.PositionToString(newPostition), MainPage.chessPieceche[thePiece.index]);

                // moves the given piece
                Grid.SetColumn(MainPage.imageButtons[thePiece.index], newPostition.x);
                Grid.SetRow(MainPage.imageButtons[thePiece.index], newPostition.y);

                // the round counter goes up
                MainPage.chessPieceche[thePiece.index].lastMoveRound = MainPage.currentRound;
                MainPage.currentRound++;

                // to change the box
                MainPage.TurnBox(true);

                // checking if there is a winner
                MainPage.IsThereCheck(thePiece.isWhite);
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

                Color backColor;
                if (positionToCheck.x % 2 == 0)
                {
                    if (positionToCheck.y % 2 == 0)
                        backColor = Colors.AliceBlue;
                    else
                        backColor = Colors.Navy;
                }
                else
                {
                    if (positionToCheck.y % 2 == 1)
                        backColor = Colors.AliceBlue;
                    else
                        backColor = Colors.Navy;
                }


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
                    BackgroundColor = backColor,
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

            if (passedOn.Length == 3) // en passant moment
            {
                Position enPassant = new Position(passedOn[2][2] - 48, passedOn[2][7] - 48);
                TakePiece(newPosition, thePiece, enPassant);
            }
            else // the standerd to take
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

            if (passedOn.Length == 4) // casteling moment
                MovePiece(thePiece.position, newPosition, new Position(passedOn[2][2] - 48, passedOn[2][7] - 48), new Position(passedOn[3][2] - 48, passedOn[3][7] - 48));
            else // the standard route
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
        public static List<Position> KingPositions(ChessPiece thePiece)
        {
            List<Position> kingPositions = new List<Position>
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
            return kingPositions;
        }
        public static List<Position> KingPositions(Position newPosition)
        {
            List<Position> kingPositions = new List<Position>
            {
            new Position(newPosition.x - 1, newPosition.y + 1),
            new Position(newPosition.x, newPosition.y + 1),
            new Position(newPosition.x + 1, newPosition.y + 1),
            new Position(newPosition.x + 1, newPosition.y),
            new Position(newPosition.x + 1, newPosition.y - 1),
            new Position(newPosition.x, newPosition.y - 1),
            new Position(newPosition.x - 1, newPosition.y - 1),
            new Position(newPosition.x - 1, newPosition.y),
            };
            return kingPositions;
        }
        public static void KingMover(ChessPiece thePiece, Dictionary<string, bool> kingIsChecked)
        {
            // castleing
            if (MainPage.piecePostions[thePiece.position.ToString()].lastMoveRound == 0)
            {
                Position castleRight = new(7, thePiece.position.y);
                bool castleRightBoo = true;
                for (int i = thePiece.position.x + 1; i < 7; i++)
                    if (IsPostitionOccupied(new Position(i, thePiece.position.y)))
                        castleRightBoo = false;

                if (MainPage.piecePostions.ContainsKey(castleRight.ToString()) && castleRightBoo &&
                    MainPage.piecePostions[castleRight.ToString()].pieceType == ChessPiece.PieceTypes.rook &&
                    MainPage.piecePostions[castleRight.ToString()].lastMoveRound == 0)
                    CheckSurroundings(thePiece, new Position(thePiece.position.x + 2, thePiece.position.y), castleRight, new Position(thePiece.position.x + 1, thePiece.position.y));


                Position castleLeft = new(0, thePiece.position.y);
                bool castleLeftBoo = true;
                for (int i = 1; i < thePiece.position.x; i++)
                    if (IsPostitionOccupied(new Position(i, thePiece.position.y)))
                        castleLeftBoo = false;

                if (MainPage.piecePostions.ContainsKey(castleLeft.ToString()) && castleLeftBoo &&
                    MainPage.piecePostions[castleLeft.ToString()].pieceType == ChessPiece.PieceTypes.rook &&
                    MainPage.piecePostions[castleLeft.ToString()].lastMoveRound == 0)
                    CheckSurroundings(thePiece, new Position(thePiece.position.x - 2, thePiece.position.y), castleLeft, new Position(thePiece.position.x - 1, thePiece.position.y));
            }

            List<Position> kingPositions = KingPositions(thePiece);
            for (int i = 0; i < kingPositions.Count; i++)
                if (kingIsChecked != null)
                    if (kingIsChecked.ContainsKey(kingPositions[i].ToString()))
                        CheckSurroundings(thePiece, kingPositions[i]);
                    else
                        continue;
                else
                    CheckSurroundings(thePiece, kingPositions[i]);

        } // KingMover
        public static (Dictionary<string, bool>, bool iDidSomething) CanTheKingMove(ChessPiece thePiece)
        {
            var (boo, legalIfChecked) = CheckTheKingStep(thePiece, thePiece.position);
            if (!boo)
            {
                return (legalIfChecked, true);
            }

            return (null, false);
        } // CanTheKingMove
        public static (bool boo, Dictionary<string, bool> legalIfChecked) CheckTheKingStep(ChessPiece thePiece, Position newPosition)
        {
            Dictionary<string, bool> legalIfChecked = new Dictionary<string, bool>();
            List<Position>[] positionStack = new List<Position>[11];
            for (int i = 0; i < positionStack.Length; i++)
                positionStack[i] = new List<Position>();
            bool returnValue = true;

            // queen, bishop and rook (the queen consist of both)
            var tempPos = QueenPositions(newPosition);
            for (int i = 0; i < tempPos.Length; i++)
                positionStack[i] = tempPos[i];

            // knight
            positionStack[8].AddRange(KnightPositions(newPosition));

            // pawn
            positionStack[9].AddRange(PawnPositions(thePiece, newPosition));

            // king
            //positionStack[10].AddRange(KingPositions(newPosition));

            // the unholy checks
            for (int j = 0; j < positionStack.Length; j++)
                switch (j)
                {
                    case int x when x is >= 0 and <= 3: // diagnol pieces
                        for (int k = 0; k < positionStack[j].Count; k++)
                            if (IsPostitionOccupied(positionStack[j][k]))
                                if (MainPage.piecePostions[positionStack[j][k].ToString()].pieceType == ChessPiece.PieceTypes.queen &&
                                    MainPage.piecePostions[positionStack[j][k].ToString()].isWhite != thePiece.isWhite ||
                                    MainPage.piecePostions[positionStack[j][k].ToString()].pieceType == ChessPiece.PieceTypes.bishop &&
                                    MainPage.piecePostions[positionStack[j][k].ToString()].isWhite != thePiece.isWhite)
                                {
                                    for (int l = 0; l < positionStack[j].Count; l++)
                                        legalIfChecked.Add(positionStack[j][l].ToString(), true);

                                    returnValue = false;
                                }
                        break;

                    case int x when x is >= 4 and <= 7: // linear pieces
                        for (int k = 0; k < positionStack[j].Count; k++)
                            if (IsPostitionOccupied(positionStack[j][k]))
                                if (MainPage.piecePostions[positionStack[j][k].ToString()].pieceType == ChessPiece.PieceTypes.queen &&
                                    MainPage.piecePostions[positionStack[j][k].ToString()].isWhite != thePiece.isWhite ||
                                    MainPage.piecePostions[positionStack[j][k].ToString()].pieceType == ChessPiece.PieceTypes.rook &&
                                    MainPage.piecePostions[positionStack[j][k].ToString()].isWhite != thePiece.isWhite)
                                {
                                    for (int l = 0; l < positionStack[j].Count; l++)
                                        legalIfChecked.Add(positionStack[j][l].ToString(), true);

                                    returnValue = false;
                                }
                        break;

                    case 8: // knights
                        for (int i = 0; i < positionStack[j].Count; i++)
                            if (IsPostitionOccupied(positionStack[j][i]))
                                if (MainPage.piecePostions[positionStack[j][i].ToString()].pieceType == ChessPiece.PieceTypes.knight &&
                                    MainPage.piecePostions[positionStack[j][i].ToString()].isWhite != thePiece.isWhite)
                                {
                                    legalIfChecked.Add(positionStack[j][i].ToString(), true);

                                    returnValue = false;
                                }
                        break;

                    case 9: // pawns
                        for (int i = 0; i < positionStack[j].Count; i++)
                            if (IsPostitionOccupied(positionStack[j][i]))
                                if (MainPage.piecePostions[positionStack[j][i].ToString()].pieceType == ChessPiece.PieceTypes.pawn &&
                                    MainPage.piecePostions[positionStack[j][i].ToString()].isWhite != thePiece.isWhite)
                                {
                                    legalIfChecked.Add(positionStack[j][i].ToString(), true);

                                    returnValue = false;
                                }
                        break;

                    case 10: // king
                        for (int i = 0; i < positionStack[j].Count; i++)
                            if (IsPostitionOccupied(positionStack[j][i]))
                                if (MainPage.piecePostions[positionStack[j][i].ToString()].pieceType == ChessPiece.PieceTypes.king &&
                                    MainPage.piecePostions[positionStack[j][i].ToString()].isWhite != thePiece.isWhite)
                                {
                                    legalIfChecked.Add(positionStack[j][i].ToString(), true);

                                    returnValue = false;
                                }
                        break;

                    default:
                        Debug.WriteLine("how and why did you fuck this up");
                        throw new Exception();
                }
            return (returnValue, legalIfChecked);
        }
        public static (bool boo, Dictionary<string, bool> legalIfChecked) CheckTheKingCheck(ChessPiece thePiece, Position newPosition)
        {
            Dictionary<string, bool> legalIfChecked = new Dictionary<string, bool>();
            List<Position>[] positionStack = new List<Position>[10];
            for (int i = 0; i < positionStack.Length; i++)
                positionStack[i] = new List<Position>();
            bool returnValue = true;

            // queen, bishop and rook (the queen consist of both)
            var tempPos = QueenPositions(newPosition);
            for (int i = 0; i < tempPos.Length; i++)
                positionStack[i] = tempPos[i];

            // knight
            positionStack[8].AddRange(KnightPositions(newPosition));

            // pawn
            positionStack[9].AddRange(PawnPositions(thePiece, newPosition));

            // king
            // positionStack[10].AddRange(KingPositions(newPosition));

            // the unholy checks
            for (int j = 0; j < positionStack.Length; j++)
                switch (j)
                {
                    case int x when x is >= 0 and <= 3: // diagnol pieces
                        for (int k = 0; k < positionStack[j].Count; k++)
                            if (IsPostitionOccupied(positionStack[j][k]))
                                if (MainPage.piecePostions[positionStack[j][k].ToString()].pieceType == ChessPiece.PieceTypes.queen &&
                                    MainPage.piecePostions[positionStack[j][k].ToString()].isWhite == thePiece.isWhite ||
                                    MainPage.piecePostions[positionStack[j][k].ToString()].pieceType == ChessPiece.PieceTypes.bishop &&
                                    MainPage.piecePostions[positionStack[j][k].ToString()].isWhite == thePiece.isWhite)
                                {
                                    for (int l = 0; l < positionStack[j].Count; l++)
                                        legalIfChecked.Add(positionStack[j][l].ToString(), true);

                                    returnValue = false;
                                }
                        break;

                    case int x when x is >= 4 and <= 7: // linear pieces
                        for (int k = 0; k < positionStack[j].Count; k++)
                            if (IsPostitionOccupied(positionStack[j][k]))
                                if (MainPage.piecePostions[positionStack[j][k].ToString()].pieceType == ChessPiece.PieceTypes.queen &&
                                    MainPage.piecePostions[positionStack[j][k].ToString()].isWhite == thePiece.isWhite ||
                                    MainPage.piecePostions[positionStack[j][k].ToString()].pieceType == ChessPiece.PieceTypes.rook &&
                                    MainPage.piecePostions[positionStack[j][k].ToString()].isWhite == thePiece.isWhite)
                                {
                                    for (int l = 0; l < positionStack[j].Count; l++)
                                        legalIfChecked.Add(positionStack[j][l].ToString(), true);

                                    returnValue = false;
                                }
                        break;

                    case 8: // knights
                        for (int i = 0; i < positionStack[j].Count; i++)
                            if (IsPostitionOccupied(positionStack[j][i]))
                                if (MainPage.piecePostions[positionStack[j][i].ToString()].pieceType == ChessPiece.PieceTypes.knight &&
                                    MainPage.piecePostions[positionStack[j][i].ToString()].isWhite == thePiece.isWhite)
                                {
                                    legalIfChecked.Add(positionStack[j][i].ToString(), true);

                                    returnValue = false;
                                }
                        break;

                    case 9: // pawns
                        for (int i = 0; i < positionStack[j].Count; i++)
                            if (IsPostitionOccupied(positionStack[j][i]))
                                if (MainPage.piecePostions[positionStack[j][i].ToString()].pieceType == ChessPiece.PieceTypes.pawn &&
                                    MainPage.piecePostions[positionStack[j][i].ToString()].isWhite == thePiece.isWhite)
                                {
                                    legalIfChecked.Add(positionStack[j][i].ToString(), true);

                                    returnValue = false;
                                }
                        break;

                    case 10: // king
                        for (int i = 0; i < positionStack[j].Count; i++)
                            if (IsPostitionOccupied(positionStack[j][i]))
                                if (MainPage.piecePostions[positionStack[j][i].ToString()].pieceType == ChessPiece.PieceTypes.king &&
                                    MainPage.piecePostions[positionStack[j][i].ToString()].isWhite == thePiece.isWhite)
                                {
                                    legalIfChecked.Add(positionStack[j][i].ToString(), true);

                                    returnValue = false;
                                }
                        break;

                    default:
                        Debug.WriteLine("how and why did you fuck this up");
                        throw new Exception();
                }
            return (returnValue, legalIfChecked);
        }
        private static void CheckSurroundings(ChessPiece thePiece, Position newKingPos, Position oldRookPos, Position newRookPos)
        {
            string passOn = string.Format(MainPage.PositionToString(thePiece.position) + "|" + MainPage.PositionToString(newKingPos) + "|" + MainPage.PositionToString(oldRookPos) + "|" + MainPage.PositionToString(newRookPos));
            Button button = new Button
            {
                Opacity = 0,
                ZIndex = 30,
                ClassId = passOn,

            };
            Button border = new Button
            {
                BorderColor = Colors.Green,
                BorderWidth = 5,
                ZIndex = 15,
            };
            MainPage.Board.Children.Add(border);
            Grid.SetColumn(border, newKingPos.x);
            Grid.SetRow(border, newKingPos.y);
            removeLater.Add(border);

            MainPage.Board.Children.Add(button);
            Grid.SetColumn(button, newKingPos.x);
            Grid.SetRow(button, newKingPos.y);
            removeLater.Add(button);

            button.Clicked += MovePiece_Clicked;

        } // CheckSurroundings for the kings castleing
        private static void MovePiece(Position oldKingPos, Position newKingPos, Position oldRookPos, Position newRookPos)
        {
            var theKing = MainPage.piecePostions[MainPage.PositionToString(oldKingPos)];
            MainPage.chessPieceche[theKing.index].position = newKingPos;

            var theRook = MainPage.piecePostions[MainPage.PositionToString(oldRookPos)];
            MainPage.chessPieceche[theRook.index].position = newRookPos;

            // removes the old allocated postion in the dictionary and allocates the new position for the king
            MainPage.piecePostions.Remove(MainPage.PositionToString(oldKingPos));
            MainPage.piecePostions.Add(MainPage.PositionToString(newKingPos), MainPage.chessPieceche[theKing.index]);

            // removes the old allocated postion in the dictionary and allocates the new position for the rook
            MainPage.piecePostions.Remove(MainPage.PositionToString(oldRookPos));
            MainPage.piecePostions.Add(MainPage.PositionToString(newRookPos), MainPage.chessPieceche[theRook.index]);

            // moves the king
            Grid.SetColumn(MainPage.imageButtons[theKing.index], newKingPos.x);
            Grid.SetRow(MainPage.imageButtons[theKing.index], newKingPos.y);

            // moves the rook
            Grid.SetColumn(MainPage.imageButtons[theRook.index], newRookPos.x);
            Grid.SetRow(MainPage.imageButtons[theRook.index], newRookPos.y);

            // the round counter goes up
            MainPage.chessPieceche[theKing.index].lastMoveRound = MainPage.currentRound;
            MainPage.chessPieceche[theRook.index].lastMoveRound = MainPage.currentRound;
            MainPage.currentRound++;

            // to change the box
            MainPage.TurnBox(true);
        } // MovePiece for the kings casteling
        private static List<Position>[] QueenPositions(ChessPiece thePiece)
        {
            List<Position>[] queenPositions = new List<Position>[8];
            var rook = RookPositions(thePiece);
            var bishop = BishopPositions(thePiece);
            for (int i = 0; i < 4; i++)
            {
                queenPositions[i] = rook[i];
                queenPositions[i + 4] = bishop[i];
            }
            return queenPositions;
        }
        private static List<Position>[] QueenPositions(Position newPosition)
        {
            List<Position>[] queenPositions = new List<Position>[8];
            var rook = RookPositions(newPosition);
            var bishop = BishopPositions(newPosition);
            for (int i = 0; i < 4; i++)
            {
                queenPositions[i] = rook[i];
                queenPositions[i + 4] = bishop[i];
            }
            return queenPositions;
        }
        public static void QueenMover(ChessPiece thePiece, Dictionary<string, bool> kingIsChecked)
        {
            List<Position>[] queenPositions = QueenPositions(thePiece);

            for (int i = 0; i < queenPositions.Length; i++)
                for (int j = 0; j < queenPositions[i].Count; j++)
                    if (kingIsChecked != null)
                        if (kingIsChecked.ContainsKey(queenPositions[j].ToString()))
                            CheckSurroundings(thePiece, queenPositions[i][j]);
                        else
                            continue;
                    else
                        CheckSurroundings(thePiece, queenPositions[i][j]);
        } // QueenMover
        private static List<Position>[] BishopPositions(ChessPiece thePiece)
        {
            List<Position>[] positionStack = new List<Position>[4];
            for (int i = 0; i < positionStack.Length; i++)
                positionStack[i] = new List<Position>();

            bool[] bools = new bool[4];
            for (int j = 0; j < bools.Length; j++)
                bools[j] = true;

            for (int i = 1; i < 8; i++)
            {
                if (bools[0])
                {
                    positionStack[0].Add(new Position(thePiece.position.x + i, thePiece.position.y + i));
                    if (IsPostitionOccupied(positionStack[0].Last()))
                        if (MainPage.piecePostions[positionStack[0].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[0] = false;
                }
                if (bools[1])
                {
                    positionStack[1].Add(new Position(thePiece.position.x - i, thePiece.position.y + i));
                    if (IsPostitionOccupied(positionStack[1].Last()))
                        if (MainPage.piecePostions[positionStack[1].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[1] = false;
                }
                if (bools[2])
                {
                    positionStack[2].Add(new Position(thePiece.position.x - i, thePiece.position.y - i));
                    if (IsPostitionOccupied(positionStack[2].Last()))
                        if (MainPage.piecePostions[positionStack[2].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[2] = false;
                }
                if (bools[3])
                {
                    positionStack[3].Add(new Position(thePiece.position.x + i, thePiece.position.y - i));
                    if (IsPostitionOccupied(positionStack[3].Last()))
                        if (MainPage.piecePostions[positionStack[3].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[3] = false;
                }
            }

            return positionStack;
        }
        private static List<Position>[] BishopPositions(Position newPosition)
        {
            List<Position>[] positionStack = new List<Position>[4];
            for (int i = 0; i < positionStack.Length; i++)
                positionStack[i] = new List<Position>();

            bool[] bools = new bool[4];
            for (int j = 0; j < bools.Length; j++)
                bools[j] = true;

            for (int i = 1; i < 8; i++)
            {
                if (bools[0])
                {
                    positionStack[0].Add(new Position(newPosition.x + i, newPosition.y + i));
                    if (IsPostitionOccupied(positionStack[0].Last()))
                        if (MainPage.piecePostions[positionStack[0].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[0] = false;
                }
                if (bools[1])
                {
                    positionStack[1].Add(new Position(newPosition.x - i, newPosition.y + i));
                    if (IsPostitionOccupied(positionStack[1].Last()))
                        if (MainPage.piecePostions[positionStack[1].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[1] = false;
                }
                if (bools[2])
                {
                    positionStack[2].Add(new Position(newPosition.x - i, newPosition.y - i));
                    if (IsPostitionOccupied(positionStack[2].Last()))
                        if (MainPage.piecePostions[positionStack[2].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[2] = false;
                }
                if (bools[3])
                {
                    positionStack[3].Add(new Position(newPosition.x + i, newPosition.y - i));
                    if (IsPostitionOccupied(positionStack[3].Last()))
                        if (MainPage.piecePostions[positionStack[3].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[3] = false;
                }
            }

            return positionStack;
        }

        public static void BishopMover(ChessPiece thePiece, Dictionary<string, bool> kingIsChecked)
        {
            List<Position>[] bishopPositions = BishopPositions(thePiece);

            for (int i = 0; i < bishopPositions.Length; i++)
                for (int j = 0; j < bishopPositions[i].Count; j++)
                    if (kingIsChecked != null)
                        if (kingIsChecked.ContainsKey(bishopPositions[i][j].ToString()))
                            CheckSurroundings(thePiece, bishopPositions[i][j]);
                        else
                            continue;
                    else
                        CheckSurroundings(thePiece, bishopPositions[i][j]);
        } // BishopMover
        private static List<Position> KnightPositions(ChessPiece thePiece)
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
            return knightPositions.ToList();
        }
        private static List<Position> KnightPositions(Position newPosition)
        {
            Position[] knightPositions =
            {
                new Position(newPosition.x + 2, newPosition.y + 1),
                new Position(newPosition.x + 2, newPosition.y - 1),
                new Position(newPosition.x - 2, newPosition.y + 1),
                new Position(newPosition.x - 2, newPosition.y - 1),
                new Position(newPosition.x + 1, newPosition.y + 2),
                new Position(newPosition.x - 1, newPosition.y + 2),
                new Position(newPosition.x + 1, newPosition.y - 2),
                new Position(newPosition.x - 1, newPosition.y - 2),
            };
            return knightPositions.ToList();
        }
        public static void KnightMover(ChessPiece thePiece, Dictionary<string, bool> kingIsChecked)
        {
            List<Position> knightPositions = KnightPositions(thePiece);
            for (int i = 0; i < knightPositions.Count; i++)
                if (kingIsChecked != null)
                    if (kingIsChecked.ContainsKey(knightPositions[i].ToString()))
                        CheckSurroundings(thePiece, knightPositions[i]);
                    else
                        continue;
                else
                    CheckSurroundings(thePiece, knightPositions[i]);
        } // KnightMover
        private static List<Position>[] RookPositions(ChessPiece thePiece)
        {
            List<Position>[] positionStack = new List<Position>[4];
            for (int i = 0; i < positionStack.Length; i++)
                positionStack[i] = new List<Position>();

            bool[] bools = new bool[4];
            for (int j = 0; j < bools.Length; j++)
                bools[j] = true;

            for (int i = 1; i < 8; i++)
            {
                if (bools[0])
                {
                    positionStack[0].Add(new Position(thePiece.position.x + i, thePiece.position.y));
                    if (IsPostitionOccupied(positionStack[0].Last()))
                        if (MainPage.piecePostions[positionStack[0].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[0] = false;
                }
                if (bools[1])
                {
                    positionStack[1].Add(new Position(thePiece.position.x - i, thePiece.position.y));
                    if (IsPostitionOccupied(positionStack[1].Last()))
                        if (MainPage.piecePostions[positionStack[1].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[1] = false;
                }
                if (bools[2])
                {
                    positionStack[2].Add(new Position(thePiece.position.x, thePiece.position.y + i));
                    if (IsPostitionOccupied(positionStack[2].Last()))
                        if (MainPage.piecePostions[positionStack[2].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[2] = false;
                }
                if (bools[3])
                {
                    positionStack[3].Add(new Position(thePiece.position.x, thePiece.position.y - i));
                    if (IsPostitionOccupied(positionStack[3].Last()))
                        if (MainPage.piecePostions[positionStack[3].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[3] = false;
                }
            }

            return positionStack;
        }
        private static List<Position>[] RookPositions(Position newPosition)
        {
            List<Position>[] positionStack = new List<Position>[4];
            for (int i = 0; i < positionStack.Length; i++)
                positionStack[i] = new List<Position>();

            bool[] bools = new bool[4];
            for (int j = 0; j < bools.Length; j++)
                bools[j] = true;

            for (int i = 1; i < 8; i++)
            {
                if (bools[0])
                {
                    positionStack[0].Add(new Position(newPosition.x + i, newPosition.y));
                    if (IsPostitionOccupied(positionStack[0].Last()))
                        if (MainPage.piecePostions[positionStack[0].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[0] = false;
                }
                if (bools[1])
                {
                    positionStack[1].Add(new Position(newPosition.x - i, newPosition.y));
                    if (IsPostitionOccupied(positionStack[1].Last()))
                        if (MainPage.piecePostions[positionStack[1].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[1] = false;
                }
                if (bools[2])
                {
                    positionStack[2].Add(new Position(newPosition.x, newPosition.y + i));
                    if (IsPostitionOccupied(positionStack[2].Last()))
                        if (MainPage.piecePostions[positionStack[2].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[2] = false;
                }
                if (bools[3])
                {
                    positionStack[3].Add(new Position(newPosition.x, newPosition.y - i));
                    if (IsPostitionOccupied(positionStack[3].Last()))
                        if (MainPage.piecePostions[positionStack[3].Last().ToString()].pieceType != ChessPiece.PieceTypes.king)
                            bools[3] = false;
                }
            }

            return positionStack;
        }
        public static void RookMover(ChessPiece thePiece, Dictionary<string, bool> kingIsChecked)
        {
            List<Position>[] rookPositions = RookPositions(thePiece);

            for (int i = 0; i < rookPositions.Length; i++)
                for (int j = 0; j < rookPositions[i].Count; j++)
                    if (kingIsChecked != null)
                        if (kingIsChecked.ContainsKey(rookPositions[i][j].ToString()))
                            CheckSurroundings(thePiece, rookPositions[i][j]);
                        else
                            continue;
                    else
                        CheckSurroundings(thePiece, rookPositions[i][j]);
        } // RookMover
        private static List<Position> PawnPositions(ChessPiece thePiece)
        {
            List<Position> pawnPostions = new List<Position>();
            int polarrization = 1;
            int twoJump = 1;
            if (MainPage.isFlipped)
            {
                if (!thePiece.isWhite)
                {
                    polarrization = -1;
                    twoJump = 6;
                }
            }
            else
            {
                if (thePiece.isWhite)
                {
                    polarrization = -1;
                    twoJump = 6;
                }
            }

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
                    if (thePiece.position.y == twoJump)
                        pawnPostions.Add(tempPos2);
                }
                else
                {
                    if (thePiece.position.y == twoJump)
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

            return pawnPostions;
        }
        private static List<Position> PawnPositions(ChessPiece thePiece, Position newPosition)
        {
            List<Position> pawnPostions = new List<Position>();
            int polarrization = 1;
            int twoJump = 1;
            if (MainPage.isFlipped)
            {
                if (!thePiece.isWhite)
                {
                    polarrization = -1;
                    twoJump = 6;
                }
            }
            else
            {
                if (thePiece.isWhite)
                {
                    polarrization = -1;
                    twoJump = 6;
                }
            }

            // checking the varoius possible postions for pawns
            // checking if the spot straight ahead is takable or not
            Position tempPos1 = new Position(newPosition.x, newPosition.y + 1 * polarrization);
            if (!IsPieceTakeable(tempPos1, thePiece.isWhite))
                pawnPostions.Add(tempPos1);

            // checking if it can move two spots
            Position tempPos2 = new Position(newPosition.x, newPosition.y + 2 * polarrization);
            if (!IsPostitionOccupied(tempPos1) && !IsPostitionOccupied(tempPos2))
            {
                if (thePiece.isWhite)
                {
                    if (newPosition.y == twoJump)
                        pawnPostions.Add(tempPos2);
                }
                else
                {
                    if (newPosition.y == twoJump)
                        pawnPostions.Add(tempPos2);
                }
            }

            // attack position to the right
            Position tempPos3 = new Position(newPosition.x + 1 * polarrization, newPosition.y + 1 * polarrization);
            if (IsPieceTakeable(tempPos3, thePiece.isWhite))
                pawnPostions.Add(tempPos3);

            // attack position to the left
            Position tempPos4 = new Position(newPosition.x - 1 * polarrization, newPosition.y + 1 * polarrization);
            if (IsPieceTakeable(tempPos4, thePiece.isWhite))
                pawnPostions.Add(tempPos4);

            return pawnPostions;
        }
        public static void PawnMover(ChessPiece thePiece, Dictionary<string, bool> kingIsChecked)
        {
            List<Position> pawnPostions = PawnPositions(thePiece);
            int polarrization = 1;
            int enpassantpos = 4;
            if (MainPage.isFlipped)
            {
                if (!thePiece.isWhite)
                {
                    polarrization = -1;
                    enpassantpos = 3;
                }
            }
            else
            {
                if (thePiece.isWhite)
                {
                    polarrization = -1;
                    enpassantpos = 3;
                }
            }

            // an attempt at making en passant
            if (thePiece.position.y == enpassantpos)
            {
                // en passant check on the right side
                Position tempEnpassant1 = new Position(thePiece.position.x + 1 * polarrization, thePiece.position.y + 1 * polarrization);
                Position tempEnpassant2 = new Position(thePiece.position.x + 1 * polarrization, thePiece.position.y);
                if (IsPieceTakeable(tempEnpassant2, thePiece.isWhite) && !IsPostitionOccupied(tempEnpassant1) && MainPage.piecePostions[MainPage.PositionToString(tempEnpassant2)].lastMoveRound == MainPage.currentRound - 1)
                    CheckSurroundings(thePiece, tempEnpassant2, tempEnpassant1);

                // en passant check on the left side
                Position tempEnpassant3 = new Position(thePiece.position.x - 1 * polarrization, thePiece.position.y + 1 * polarrization);
                Position tempEnpassant4 = new Position(thePiece.position.x - 1 * polarrization, thePiece.position.y);
                if (IsPieceTakeable(tempEnpassant4, thePiece.isWhite) && !IsPostitionOccupied(tempEnpassant3) && MainPage.piecePostions[MainPage.PositionToString(tempEnpassant4)].lastMoveRound == MainPage.currentRound - 1)
                    CheckSurroundings(thePiece, tempEnpassant4, tempEnpassant3);
            }



            // checking the possible postions
            for (int i = 0; i < pawnPostions.Count; i++)
                if (kingIsChecked != null)
                    if (kingIsChecked.ContainsKey(pawnPostions[i].ToString()))
                        CheckSurroundings(thePiece, pawnPostions[i]);
                    else
                        continue;
                else
                    CheckSurroundings(thePiece, pawnPostions[i]);
        } // PawnMover
        private static void CheckSurroundings(ChessPiece thePiece, Position positionToCheck, Position enPassant)
        {
            if (positionToCheck.x >= 0 && positionToCheck.x <= 7 && positionToCheck.y >= 0 && positionToCheck.y <= 7)
            {
                string passOn = string.Format(MainPage.PositionToString(thePiece.position) + "|" + MainPage.PositionToString(positionToCheck) + "|" + MainPage.PositionToString(enPassant));
                Button button = new Button
                {
                    Opacity = 0,
                    ZIndex = 30,
                    ClassId = passOn,

                };
                Button button2 = new Button
                {
                    Opacity = 0,
                    ZIndex = 30,
                    ClassId = passOn,

                };
                Button border = new Button
                {
                    BorderColor = Colors.Red,
                    BorderWidth = 5,
                    ZIndex = 15,
                };
                MainPage.Board.Children.Add(border);
                Grid.SetColumn(border, enPassant.x);
                Grid.SetRow(border, enPassant.y);
                removeLater.Add(border);

                MainPage.Board.Children.Add(button);
                Grid.SetColumn(button, enPassant.x);
                Grid.SetRow(button, enPassant.y);
                removeLater.Add(button);

                MainPage.Board.Children.Add(button2);
                Grid.SetColumn(button2, positionToCheck.x);
                Grid.SetRow(button2, positionToCheck.y);
                removeLater.Add(button2);

                button.Clicked += TakePiece_Clicked;
                button2.Clicked += TakePiece_Clicked;
            }
        } // CheckSurroundings
        private static void TakePiece(Position enemyPosition, ChessPiece pieceTryingToTake, Position enPassant)
        {
            // "killing" the enemy piece
            MainPage.piecePostions[MainPage.PositionToString(enemyPosition)].isAlive = false;
            MainPage.Board.Remove(MainPage.imageButtons[MainPage.piecePostions[MainPage.PositionToString(enemyPosition)].index]);

            MainPage.piecePostions.Remove(MainPage.PositionToString(enemyPosition));


            MovePiece(pieceTryingToTake.position, enPassant);
        } // TakePiece
    }
}
