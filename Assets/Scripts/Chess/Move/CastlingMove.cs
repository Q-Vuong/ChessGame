using UnityEngine;

public class CastlingMove : Move
{
    public ChessPiece RookPiece { get; }
    public Vector2Int RookTo { get; }

    public CastlingMove(ChessPiece piece, Vector2Int from, Vector2Int to, ChessPiece rookPiece) : base(piece, from, to)
    {
        RookPiece = rookPiece;

        int direction = to.x > from.x ? 1 : -1;
        int newX = to.x - direction;
        RookTo = new Vector2Int(newX, from.y);
    }
}