using System;
using UnityEngine;

public class Knight : ChessPiece
{
    public Knight(ChessColor color, Vector2Int position) : base(color, position)
    {
    }

    public override bool CanMove(Vector2Int from, Vector2Int to)
    {
        int dx = Math.Abs(to.x - from.x);
        int dy = Math.Abs(to.y - from.y);

        return (dx == 2 && dy == 1) || (dx == 1 && dy == 2);
    }
}