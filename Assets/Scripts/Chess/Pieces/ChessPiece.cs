using UnityEngine;

public class ChessPiece
{
    public ChessPiece(ChessColor color, Vector2Int position)
    {
        Color = color;
        Position = position;
    }

    public ChessColor Color { get; private set; }
    public Vector2Int Position { get; private set; }
    public virtual bool NeedsPathCheck { get; } = false;
    public bool HasMoved { get; private set; }

    public void MarkAsMove()
    {
        HasMoved = true;
    }

    public void UpdatePosition(Vector2Int newPosition)
    {
        Position = newPosition;
    }

    //Luat di chuyen
    public virtual bool CanMove(Vector2Int from, Vector2Int to)
    {
        return false;
    }

    public virtual bool CanAttack(Vector2Int from, Vector2Int to)
    {
        return CanMove(from, to);
    }
}