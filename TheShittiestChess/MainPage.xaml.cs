using System.Diagnostics;

namespace TheShittiestChess
{
    public partial class MainPage : ContentPage
    {
        public static ChessPiece[] chessPieceche = new ChessPiece[32];
        public static Dictionary<string, ChessPiece> piecePostions = new Dictionary<string, ChessPiece>();
        public static ImageButton[] imageButtons = new ImageButton[32];
        public static int currentRound = 0;
        public static bool isDebugging = false;
        public static bool isFlipped = false;
        public static Grid Board { get; private set; }
        public static Grid HighLight { get; private set; }
        public static Label ColorBox { get; private set; }
        public MainPage()
        {
            InitializeComponent();
            Board = board;
            HighLight = HighLightBoard;
            ColorBox = colorBox;

            MakeBoard(Board);
            InitPieces();
            MakePieces(Board);


        }
        public static void MakePieces(Grid board)
        {
            for (int i = 0; i < chessPieceche.Length; i++)
            {
                if (chessPieceche[i].isAlive)
                {
                    imageButtons[i] = new ImageButton
                    {
                        ZIndex = 20,
                        Source = chessPieceche[i].imageSource,
                        CommandParameter = chessPieceche[i],
                        ClassId = i.ToString(),
                    };
                    board.Children.Add(imageButtons[i]);
                    Grid.SetColumn(imageButtons[i], chessPieceche[i].position.x);
                    Grid.SetRow(imageButtons[i], chessPieceche[i].position.y);

                    imageButtons[i].Clicked += Button_Clicked;
                }
            }

        }
        private static void Button_Clicked(object sender, EventArgs e)
        {
            //setup 
            var button = sender as ImageButton;
            int identifier = int.Parse(button.ClassId);

            Movement.ClearRemoveLater();



            if (chessPieceche[identifier].isWhite == TurnBox(false) || isDebugging) // the second part is for debug use only, it should be false in actual gameplay
            {
                int poloroid = 0;
                if (!TurnBox(false))
                    poloroid = 16;

                var (legalPositions, boo) = Movement.CanTheKingMove(chessPieceche[poloroid]);




                switch (chessPieceche[identifier].pieceType) // this switch checks which piece movement type that is necessary
                {
                    case ChessPiece.PieceTypes.king:
                        Movement.KingMover(chessPieceche[identifier], legalPositions);
                        break;
                    case ChessPiece.PieceTypes.queen:
                        Movement.QueenMover(chessPieceche[identifier], legalPositions);
                        break;
                    case ChessPiece.PieceTypes.rook:
                        Movement.RookMover(chessPieceche[identifier], legalPositions);
                        break;
                    case ChessPiece.PieceTypes.bishop:
                        Movement.BishopMover(chessPieceche[identifier], legalPositions);
                        break;
                    case ChessPiece.PieceTypes.knight:
                        Movement.KnightMover(chessPieceche[identifier], legalPositions);
                        break;
                    case ChessPiece.PieceTypes.pawn:
                        Movement.PawnMover(chessPieceche[identifier], legalPositions);
                        break;
                    default:
                        break;
                }
                legalPositions?.Clear();
            }
        }
        public static bool TurnBox(bool isChange)
        {
            bool isItWhite = false;
            if (currentRound % 2 == 0)
                isItWhite = true;
            if (isChange)
                if (isItWhite)
                {
                    ColorBox.BackgroundColor = Colors.White;
                    ColorBox.Text = "White's Turn";
                    ColorBox.TextColor = Colors.Black;
                }
                else
                {
                    ColorBox.BackgroundColor = Colors.Black;
                    ColorBox.Text = "Black's Turn";
                    ColorBox.TextColor = Colors.White;
                }

            return isItWhite;
        }
        public static void IsThereCheck(bool isWhiteAsking)
        {
            int poloroid = 0;
            if (isWhiteAsking)
                poloroid = 16;

            var (legalpos, boo) = Movement.CanTheKingMove(chessPieceche[poloroid]);

            if (legalpos != null)
            {
                var legalList = legalpos.ToList();
                for (int i = legalList.Count - 1; i >= 0; i--)
                {
                    var (boohoo, something) = Movement.CheckTheKingCheck(chessPieceche[poloroid], StringToPosition(legalList[i].Key));

                    
                    
                    if (boohoo)
                    {
                        legalpos.Remove(legalList[i].Key);
                    }
                    
                }

            }

            Debug.WriteLine(legalpos?.Count);

            if (legalpos?.Count == 0 && !boo)
            {
                if (isWhiteAsking)
                {
                    ColorBox.BackgroundColor = Colors.White;
                    ColorBox.Text = "White Won";
                    ColorBox.TextColor = Colors.Black;
                }
                else
                {
                    ColorBox.BackgroundColor = Colors.Black;
                    ColorBox.Text = "Black Won";
                    ColorBox.TextColor = Colors.White;
                }
            }
        }
        public static Position StringToPosition(string idk)
        {
            return new Position(idk[2] - 48, idk[7] - 48);
        }
        public static void MakeBoard(Grid board)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Color color;
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                            color = Colors.AliceBlue;
                        else
                            color = Colors.Navy;
                    }
                    else
                    {
                        if (j % 2 == 1)
                            color = Colors.AliceBlue;
                        else
                            color = Colors.Navy;
                    }

                    Label item = new Label
                    {
                        BackgroundColor = color,
                        ZIndex = 10
                    };
                    board.Children.Add(item);
                    Grid.SetColumn(item, i);
                    Grid.SetRow(item, j);
                }
            }
        }
        public static void InitPieces()
        {
            if (isFlipped)
            {
                // pawns for both colors
                for (int i = 8; i < 8 + 8; i++)
                {
                    chessPieceche[i] = new ChessPiece(ChessPiece.PieceTypes.pawn, true, "pawnw.png", true, i, new Position(i - 8, 1));
                    chessPieceche[i + 16] = new ChessPiece(ChessPiece.PieceTypes.pawn, false, "pawnb.png", true, i + 16, new Position(i - 8, 6));
                }

                // white pieces
                chessPieceche[0] = new ChessPiece(ChessPiece.PieceTypes.king, true, "kingw.png", true, 0, new Position(3, 0));
                chessPieceche[1] = new ChessPiece(ChessPiece.PieceTypes.queen, true, "queenw.png", true, 1, new Position(4, 0));
                chessPieceche[2] = new ChessPiece(ChessPiece.PieceTypes.bishop, true, "bishopw.png", true, 2, new Position(2, 0));
                chessPieceche[3] = new ChessPiece(ChessPiece.PieceTypes.bishop, true, "bishopw.png", true, 3, new Position(5, 0));
                chessPieceche[4] = new ChessPiece(ChessPiece.PieceTypes.knight, true, "knightw.png", true, 4, new Position(1, 0));
                chessPieceche[5] = new ChessPiece(ChessPiece.PieceTypes.knight, true, "knightw.png", true, 5, new Position(6, 0));
                chessPieceche[6] = new ChessPiece(ChessPiece.PieceTypes.rook, true, "rookw.png", true, 6, new Position(0, 0));
                chessPieceche[7] = new ChessPiece(ChessPiece.PieceTypes.rook, true, "rookw.png", true, 7, new Position(7, 0));

                // black pieces
                chessPieceche[16] = new ChessPiece(ChessPiece.PieceTypes.king, false, "kingb.png", true, 16, new Position(3, 7));
                chessPieceche[17] = new ChessPiece(ChessPiece.PieceTypes.queen, false, "queenb.png", true, 17, new Position(4, 7));
                chessPieceche[18] = new ChessPiece(ChessPiece.PieceTypes.bishop, false, "bishopb.png", true, 18, new Position(2, 7));
                chessPieceche[19] = new ChessPiece(ChessPiece.PieceTypes.bishop, false, "bishopb.png", true, 19, new Position(5, 7));
                chessPieceche[20] = new ChessPiece(ChessPiece.PieceTypes.knight, false, "knightb.png", true, 20, new Position(1, 7));
                chessPieceche[21] = new ChessPiece(ChessPiece.PieceTypes.knight, false, "knightb.png", true, 21, new Position(6, 7));
                chessPieceche[22] = new ChessPiece(ChessPiece.PieceTypes.rook, false, "rookb.png", true, 22, new Position(0, 7));
                chessPieceche[23] = new ChessPiece(ChessPiece.PieceTypes.rook, false, "rookb.png", true, 23, new Position(7, 7));

            }
            else
            {
                // pawns for both colors
                for (int i = 8; i < 8 + 8; i++)
                {
                    chessPieceche[i] = new ChessPiece(ChessPiece.PieceTypes.pawn, true, "pawnw.png", true, i, new Position(i - 8, 6));
                    chessPieceche[i + 16] = new ChessPiece(ChessPiece.PieceTypes.pawn, false, "pawnb.png", true, i + 16, new Position(i - 8, 1));
                }

                // white pieces
                chessPieceche[0] = new ChessPiece(ChessPiece.PieceTypes.king, true, "kingw.png", true, 0, new Position(4, 7));
                chessPieceche[1] = new ChessPiece(ChessPiece.PieceTypes.queen, true, "queenw.png", true, 1, new Position(3, 7));
                chessPieceche[2] = new ChessPiece(ChessPiece.PieceTypes.bishop, true, "bishopw.png", true, 2, new Position(2, 7));
                chessPieceche[3] = new ChessPiece(ChessPiece.PieceTypes.bishop, true, "bishopw.png", true, 3, new Position(5, 7));
                chessPieceche[4] = new ChessPiece(ChessPiece.PieceTypes.knight, true, "knightw.png", true, 4, new Position(1, 7));
                chessPieceche[5] = new ChessPiece(ChessPiece.PieceTypes.knight, true, "knightw.png", true, 5, new Position(6, 7));
                chessPieceche[6] = new ChessPiece(ChessPiece.PieceTypes.rook, true, "rookw.png", true, 6, new Position(0, 7));
                chessPieceche[7] = new ChessPiece(ChessPiece.PieceTypes.rook, true, "rookw.png", true, 7, new Position(7, 7));

                // black pieces
                chessPieceche[16] = new ChessPiece(ChessPiece.PieceTypes.king, false, "kingb.png", true, 16, new Position(4, 0));
                chessPieceche[17] = new ChessPiece(ChessPiece.PieceTypes.queen, false, "queenb.png", true, 17, new Position(3, 0));
                chessPieceche[18] = new ChessPiece(ChessPiece.PieceTypes.bishop, false, "bishopb.png", true, 18, new Position(2, 0));
                chessPieceche[19] = new ChessPiece(ChessPiece.PieceTypes.bishop, false, "bishopb.png", true, 19, new Position(5, 0));
                chessPieceche[20] = new ChessPiece(ChessPiece.PieceTypes.knight, false, "knightb.png", true, 20, new Position(1, 0));
                chessPieceche[21] = new ChessPiece(ChessPiece.PieceTypes.knight, false, "knightb.png", true, 21, new Position(6, 0));
                chessPieceche[22] = new ChessPiece(ChessPiece.PieceTypes.rook, false, "rookb.png", true, 22, new Position(0, 0));
                chessPieceche[23] = new ChessPiece(ChessPiece.PieceTypes.rook, false, "rookb.png", true, 23, new Position(7, 0));


            }
            for (int i = 0; i < chessPieceche.Length; i++)
            {
                piecePostions.Add(PositionToString(chessPieceche[i].position), chessPieceche[i]);
            }
        }
        public static string PositionToString(Position position)
        {
            return string.Format("x:{0}, y:{1}", position.x, position.y);
        }
    }
    public class ChessPiece
    {
        public enum PieceTypes
        {
            king,
            queen,
            rook,
            bishop,
            knight,
            pawn
        }
        public PieceTypes pieceType { get; set; }
        public bool isWhite { get; set; }
        public Position position { get; set; }
        public string imageSource { get; set; }
        public bool isAlive { get; set; }
        public int index { get; set; }
        public int lastMoveRound { get; set; }
        public ChessPiece(PieceTypes pieceType, bool isWhite, string imageSource, bool isAlive, int index, Position position)
        {
            this.pieceType = pieceType;
            this.isWhite = isWhite;
            this.imageSource = imageSource;
            this.position = position;
            this.isAlive = isAlive;
            this.index = index;
        }
    }
    public class Position
    {
        public int x;
        public int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return string.Format("x:{0}, y:{1}", x, y);
        }

    }

}