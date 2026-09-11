using UnityEngine;

public class PromotionMove : Move
{
    public ChessPieceType PromotionType { get; }
    public PromotionMove(ChessPiece piece, Vector2Int from, Vector2Int to, ChessPieceType promotionType) : base(piece, from, to)
    {
        PromotionType = promotionType;
    }
}