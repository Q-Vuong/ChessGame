using UnityEngine;

public class Rook : ChessPiece
{
    public Rook(ChessColor color, Vector2Int position) : base(color, position)
    {
    }

    public override bool CanMove(Vector2Int from, Vector2Int to)
    {
        return ((from.x == to.x) || (from.y == to.y)) && (from != to);
    }
}