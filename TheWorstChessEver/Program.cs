using System;

namespace TheWorstChessEver
{
    internal class Program
    {
        static ChessPiece[] chessPieceche = new ChessPiece[32];

        static void Main(string[] args)
        {
            InitPieces();
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine("\u265A");
            Console.WriteLine("\u265A");
            Console.WriteLine('\u2658');
            Console.WriteLine(chessPieceche[1].PieceType.ToString());
            Console.WriteLine(chessPieceche[1].PieceType.ToString());
            Console.WriteLine(chessPieceche[1].PieceType.ToString());
            Console.WriteLine(chessPieceche[1].PieceType.ToString());
            Console.WriteLine('♔');
            Console.WriteLine('♔');
            Console.WriteLine("\u265A");
            WriteBoard();
            Console.WriteLine("yes");
        } // Main
        static void WriteBoard()
        {
            for (int i = 0; i < chessPieceche.Length; i++)
            {
                Console.SetCursorPosition(chessPieceche[i].xPos, chessPieceche[i].yPos);
                Console.Write(chessPieceche[i].piece);
            }
        }
        static void InitPieces()
        {
            // pawns for both colors
            for (int i = 8; i < 8 + 8; i++)
            {
                chessPieceche[i] = new ChessPiece(ChessPiece.PieceTypes.pawn, true, '♙', 1, i - 1);
                chessPieceche[i + 16] = new ChessPiece(ChessPiece.PieceTypes.pawn, false, '♟', 6, i - 1);
            }

            // white pieces
            chessPieceche[0] = new ChessPiece(ChessPiece.PieceTypes.king, true, '♔', 3, 0);
            chessPieceche[1] = new ChessPiece(ChessPiece.PieceTypes.queen, true, '♕', 4, 0);
            chessPieceche[2] = new ChessPiece(ChessPiece.PieceTypes.bishop, true, '♗', 2, 0);
            chessPieceche[3] = new ChessPiece(ChessPiece.PieceTypes.bishop, true, '♗', 5, 0);
            chessPieceche[4] = new ChessPiece(ChessPiece.PieceTypes.knight, true, '♘', 1, 0);
            chessPieceche[5] = new ChessPiece(ChessPiece.PieceTypes.knight, true, '♘', 6, 0);
            chessPieceche[6] = new ChessPiece(ChessPiece.PieceTypes.rook, true, '♖', 0, 0);
            chessPieceche[7] = new ChessPiece(ChessPiece.PieceTypes.rook, true, '♖', 7, 0);

            // black pieces
            chessPieceche[16] = new ChessPiece(ChessPiece.PieceTypes.king, false, '♚', 3, 7);
            chessPieceche[17] = new ChessPiece(ChessPiece.PieceTypes.queen, false, '♛', 4, 7);
            chessPieceche[18] = new ChessPiece(ChessPiece.PieceTypes.bishop, false, '♝', 2, 7);
            chessPieceche[19] = new ChessPiece(ChessPiece.PieceTypes.bishop, false, '♝', 5, 7);
            chessPieceche[20] = new ChessPiece(ChessPiece.PieceTypes.knight, false, '♞', 1, 7);
            chessPieceche[21] = new ChessPiece(ChessPiece.PieceTypes.knight, false, '♞', 6, 7);
            chessPieceche[22] = new ChessPiece(ChessPiece.PieceTypes.rook, false, '♜', 0, 7);
            chessPieceche[23] = new ChessPiece(ChessPiece.PieceTypes.rook, false, '♜', 7, 7);
        }
    }

    internal class ChessPiece
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
        public PieceTypes PieceType { get; set; }
        public bool isWhite { get; set; }
        public int xPos { get; set; }
        public int yPos { get; set; }
        public char piece { get; set; }
        public ChessPiece(PieceTypes pieceType, bool isWhite, char piece, int xPOS, int yPOS)
        {
            PieceType = pieceType;
            this.isWhite = isWhite;
            xPos = xPOS;
            yPos = yPOS;
            this.piece = piece;
        }
    }
}