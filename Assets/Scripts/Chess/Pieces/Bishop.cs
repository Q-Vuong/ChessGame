using UnityEngine;

public class Bishop : ChessPiece
{
    public Bishop(ChessColor color, Vector2Int position) : base(color, position)
    {
    }

    public override bool CanMove(Vector2Int from, Vector2Int to)
    {
        int dx = to.x - from.x;
        int dy = to.y - from.y;

        return (Mathf.Abs(dx) == Mathf.Abs(dy)) && (from != to);
    }
}