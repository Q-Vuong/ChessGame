using UnityEngine;

public class ChessBoard
{
    public ChessPiece?[,] Board { get; private set; }

    public ChessBoard()
    {
        Board = new ChessPiece?[8, 8];
    }
    
    public bool AddPiece(ChessPiece piece, Vector2Int position)
    {
        if (Board[position.x, position.y] != null)
            return false;

        Board[position.x, position.y] = piece;
        return true;
    }

    public ChessPiece? GetPiece(Vector2Int position)
    {
        return Board[position.x, position.y];
    }

    public ChessPiece? RemovePiece(Vector2Int position)
    {
        ChessPiece? chessPiece = GetPiece(position);
        if (chessPiece == null)
            return null;

        Board[position.x, position.y] = null;
        return chessPiece;
    }
}