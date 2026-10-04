using System;
using UnityEngine;

public class MoveExecutor
{
    private PromotionHandler _promotionHandler;

    public MoveExecutor()
    {
        _promotionHandler = new PromotionHandler();
    }

    public MoveExecutorResult ExecuteMove(Move move, ChessBoard board)
    {
        Vector2Int capturedPosition;
        ChessPiece? capturedPiece;

        switch (move)
        {
            case EnPassantMove enPassantMove:
                capturedPosition = enPassantMove.CapturedPosition;
                break;
            case Move:
                capturedPosition = move.To;
                break;
            default:
                throw new InvalidOperationException();
        }

        capturedPiece = board.GetPiece(capturedPosition);

        board.RemovePiece(capturedPosition);

        board.RemovePiece(move.From);

        if (move is PromotionMove promotionMove && move.Piece is Pawn pawn)
        {
            ChessPiece newPiece = _promotionHandler.Promote(pawn, promotionMove.PromotionType, move.To);
            board.AddPiece(newPiece, move.To);
            return new MoveExecutorResult(capturedPiece, newPiece);
        }
        else
        {
            board.AddPiece(move.Piece, move.To);
            move.Piece.UpdatePosition(move.To);
        }

        return new MoveExecutorResult(capturedPiece, null);
    }
}