using System;
using UnityEngine;

public class PromotionHandler
{
    public ChessPiece Promote(Pawn pawn, ChessPieceType pieceType, Vector2Int position)
    {
        switch (pieceType)
        {
            case ChessPieceType.Queen:
                return new Queen(pawn.Color, position);
            case ChessPieceType.Bishop:
                return new Bishop(pawn.Color, position);
            case ChessPieceType.Knight:
                return new Knight(pawn.Color, position);
            case ChessPieceType.Rook:
                return new Rook(pawn.Color, position);
            default:
                throw new ArgumentException("Invalid promotion piece type.");
        }
    }
}