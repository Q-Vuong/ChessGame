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
        ChessPiece? capturedPiece = null;

        switch (move)
        {
            case EnPassantMove enPassantMove:
                capturedPiece = board.GetPiece(enPassantMove.CapturedPosition);
                break;

            case CastlingMove castlingMove:
                MoveRook(castlingMove, board);
                break;

            default:
                capturedPiece = board.GetPiece(move.To);
                break;
        }

        if (move is PromotionMove promotionMove)
        {
            ChessPiece newPiece = MovePromotion(promotionMove, capturedPiece, board);
            return new MoveExecutorResult(capturedPiece, newPiece);
        }
        else
        {
            MovePiece(move, capturedPiece, board);
        }

        return new MoveExecutorResult(capturedPiece, null);
    }

    private void MovePiece(Move move, ChessPiece? capturedPiece, ChessBoard board)
    {
        if (capturedPiece != null)
        {
            board.RemovePiece(capturedPiece.Position);
        }

        board.RemovePiece(move.From);
        board.AddPiece(move.Piece, move.To);

        move.Piece.UpdatePosition(move.To);
        move.Piece.MarkAsMove();
    }

    private ChessPiece MovePromotion(PromotionMove move, ChessPiece? capturedPiece, ChessBoard board)
    {
        if (capturedPiece != null)
        {
            board.RemovePiece(capturedPiece.Position);
        }

        board.RemovePiece(move.From);

        ChessPiece newPiece = _promotionHandler.Promote((Pawn)move.Piece, move.PromotionType, move.To);

        board.AddPiece(newPiece, move.To);

        return newPiece;
    }

    private void MoveRook(CastlingMove move, ChessBoard board)
    {
        ChessPiece rook = move.RookPiece;

        board.RemovePiece(rook.Position);
        board.AddPiece(rook, move.RookTo);
        rook.UpdatePosition(move.RookTo);
        rook.MarkAsMove();
    }
}