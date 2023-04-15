using System.Diagnostics;

namespace TheShittiestChess
{
    internal class Movement
    {
        public static List<Button> removeLater = new List<Button>();
        private static bool IsPostitionOccupied(Position position)
        {
            return MainPage.piecePostions.ContainsKey(MainPage.PositionToString(position));
        }
        private static bool IsPieceTakeable(Position position, bool IsThePlayerWhite)
        {
            if (MainPage.piecePostions[MainPage.PositionToString(position)].isWhite != IsThePlayerWhite)
                return true;
            else
                return false;
        }
        private static void TakePiece(Position enemyPosition, ChessPiece pieceTryingToTake)
        {
            if (IsPieceTakeable(enemyPosition, pieceTryingToTake.isWhite))
            {
                // "killing" the enemy piece
                MainPage.piecePostions[MainPage.PositionToString(enemyPosition)].isAlive = false;
                MainPage.piecePostions.Remove(MainPage.PositionToString(enemyPosition));

                MovePiece(pieceTryingToTake.position, enemyPosition);
            }
            else
            {
                Debug.WriteLine("The Chosen Piece is not Takeable");
            }
        }
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
        }
        private static void CheckSurroundings(ChessPiece thePiece, Position positionToCheck)
        {
            if (positionToCheck.x >= 0 && positionToCheck.x <= 7 && positionToCheck.y >= 0 && positionToCheck.y <= 7)
            {
                Color color = new Color();
                if (IsPostitionOccupied(positionToCheck))
                {
                    if (IsPieceTakeable(positionToCheck, thePiece.isWhite))
                        color = Colors.Red;
                    else
                        color = Colors.Yellow;
                }
                else
                    color = Colors.Green;

                Button button = new Button
                {
                    IsVisible = false,
                    ZIndex = 30,
                    ClassId = MainPage.PositionToString(thePiece.position),

                };
                Button border = new Button
                {
                    BorderColor = color,
                    BorderWidth = 5,
                    ZIndex = 15,
                };
                MainPage.board.Children.Add(button);
                MainPage.board.Children.Add(border);
                Grid.SetColumn(button, positionToCheck.x);
                Grid.SetColumn(border, positionToCheck.x);
                Grid.SetRow(button, positionToCheck.y);
                Grid.SetRow(border, positionToCheck.y);

                removeLater.Add(button);
                removeLater.Add(border);
                switch (color)
                {
                    case Colors.Red:
                        button.Clicked += TakePiece_Clicked;
                        break;

                    case Colors.Yellow:
                        button.Clicked += SelectNewPiece_Clicked;
                        break;

                    case Colors.Green:
                        button.Clicked += MovePiece_Clicked;
                        break;

                    default:
                        Debug.WriteLine("I dont know how you manged to press a button and yet not press anything");
                        break;
                }
            }

        }

        private static void TakePiece_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            ChessPiece thePiece = MainPage.piecePostions[button.ClassId];

        }
        private static void SelectNewPiece_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            ChessPiece thePiece = MainPage.piecePostions[button.ClassId];

        }
        private static void MovePiece_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            ChessPiece thePiece = MainPage.piecePostions[button.ClassId];

        }

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


        }



    }
}
