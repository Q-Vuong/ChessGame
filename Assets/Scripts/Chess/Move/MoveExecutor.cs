using UnityEngine;

public class MoveExecutor
{
    private PromotionHandler promotionHandler;
    private Vector2Int capturedPosition;

    public MoveExecutor()
    {
        promotionHandler = new PromotionHandler();
    }

    public void ExecuteMove(Move move, ChessBoard board)
    {
        switch (move)
        {
            case EnPassantMove enPassantMove:
                capturedPosition = enPassantMove.CapturedPosition;
            break;
            case Move:
                capturedPosition = move.To;
                break;
        }

        if (capturedPosition != null)
        {
            board.RemovePiece(capturedPosition);
        }

        board.RemovePiece(move.From);

        if (move is PromotionMove promotionMove && move.Piece is Pawn pawn)
        {
            ChessPiece promotePiece = promotionHandler.Promote(pawn, promotionMove.PromotionType, move.To);
            board.AddPiece(promotePiece, move.To);
        }
        else
        {
            board.AddPiece(move.Piece, move.To);
            move.Piece.UpdatePosition(move.To);
        }
    }
}