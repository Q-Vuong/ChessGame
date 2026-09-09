using UnityEngine;

public class Move
{
    public ChessPiece Piece { get; }
    public Vector2Int From { get; }
    public Vector2Int To { get; }

    public Move(ChessPiece piece, Vector2Int from, Vector2Int to)
    {
        Piece = piece;
        From = from;
        To = to;
    }
}