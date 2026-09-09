using UnityEngine;

public class BoardSetup
{
    // private readonly ChessPieceType[] backRank =
    // {
    // ChessPieceType.Rook,
    // ChessPieceType.Knight,
    // ChessPieceType.Bishop,
    // ChessPieceType.Queen,
    // ChessPieceType.King,
    // ChessPieceType.Bishop,
    // ChessPieceType.Knight,
    // ChessPieceType.Rook
    // };

    private void SetupPlayerPieces(ChessBoard board, ChessColor color, int backRankY, int pawnY)
    {
        for (int x = 0; x < 8; x++)
        {
            switch (x)
            {
                case 0:
                case 7:
                    Rook rookPiece = new Rook(color, new Vector2Int(x, backRankY));
                    board.AddPiece(rookPiece, rookPiece.Position);
                    break;
                case 1:
                case 6:
                    Knight knightPiece = new Knight(color, new Vector2Int(x, backRankY));
                    board.AddPiece(knightPiece, knightPiece.Position);
                    break;
                case 2:
                case 5:
                    Bishop bishopPiece = new Bishop(color, new Vector2Int(x, backRankY));
                    board.AddPiece(bishopPiece, bishopPiece.Position);
                    break;
                case 3:
                    Queen queenPiece = new Queen(color, new Vector2Int(x, backRankY));
                    board.AddPiece(queenPiece, queenPiece.Position);
                    break;
                case 4:
                    King kingPiece = new King(color, new Vector2Int(x, backRankY));
                    board.AddPiece(kingPiece, kingPiece.Position);
                    break;
            }

            Pawn pawnPiece = new Pawn(color, new Vector2Int(x, pawnY));

            board.AddPiece(pawnPiece, pawnPiece.Position);
        }

    }

    public void Setup(ChessBoard board)
    {
        SetupPlayerPieces(board, ChessColor.White, 0, 1);
        SetupPlayerPieces(board, ChessColor.Black, 7, 6);
    }
}