using UnityEngine;

public class MoveExecutor
{
    private PromotionHandler _promotionHandler;
    private Vector2Int _capturedPosition;

    public MoveExecutor()
    {
        _promotionHandler = new PromotionHandler();
    }

    public void ExecuteMove(Move move, ChessBoard board)
    {
        switch (move)
        {
            case EnPassantMove enPassantMove:
                _capturedPosition = enPassantMove.CapturedPosition;
                break;
            case Move:
                _capturedPosition = move.To;
                break;
        }

        board.RemovePiece(_capturedPosition);

        board.RemovePiece(move.From);

        if (move is PromotionMove promotionMove && move.Piece is Pawn pawn)
        {
            ChessPiece promotePiece = _promotionHandler.Promote(pawn, promotionMove.PromotionType, move.To);
            board.AddPiece(promotePiece, move.To);
        }
        else
        {
            board.AddPiece(move.Piece, move.To);
            move.Piece.UpdatePosition(move.To);
        }
    }
}