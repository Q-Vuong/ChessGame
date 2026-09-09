using System;
using UnityEngine;

public class Pawn : ChessPiece
{
    public Pawn(ChessColor color, Vector2Int position) : base(color, position)
    {
    }

    public override bool CanMove(Vector2Int from, Vector2Int to)
    {
        int dx = to.x - from.x;
        int dy = to.y - from.y;
        int directionY = Color == ChessColor.White ? 1 : -1;

        return dx == 0 && dy == directionY;
    }

    public override bool CanAttack(Vector2Int from, Vector2Int to)
    {
        int dx = Math.Abs(to.x - from.x);
        int dy = to.y - from.y;
        int directionY = Color == ChessColor.White ? 1 : -1;

        return dx == 1 && dy == directionY;
    }
}