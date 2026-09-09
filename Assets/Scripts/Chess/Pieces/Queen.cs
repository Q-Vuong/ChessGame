using System;
using UnityEngine;

public class Queen : ChessPiece
{
    public Queen(ChessColor color, Vector2Int position) : base(color, position)
    {
    }

    public override bool CanMove(Vector2Int from, Vector2Int to)
    {
        int dx = Math.Abs(to.x - from.x);
        int dy = Math.Abs(to.y - from.y);

        return (from != to) && ((from.x == to.x) || (from.y == to.y) || (dx == dy));
    }
}