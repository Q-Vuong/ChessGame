using System;
using UnityEngine;

public class King : ChessPiece
{
    public King(ChessColor color, Vector2Int position) : base(color, position)
    {
    }

    public override bool CanMove(Vector2Int from, Vector2Int to)
    {
        int dx = Math.Abs(to.x - from.x);
        int dy = Math.Abs(to.y - from.y);

        return dx <= 1 && dy <= 1 && from != to;
    }
}