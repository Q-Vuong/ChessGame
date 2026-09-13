using UnityEngine;

public class EnPassantMove : Move
{
    public Vector2Int CapturedPosition { get; }

    public EnPassantMove(ChessPiece piece, Vector2Int from, Vector2Int to, Vector2Int capturedPosition) : base(piece, from, to)
    {
        CapturedPosition = capturedPosition;
    }
}