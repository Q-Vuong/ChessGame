public class MoveExecutor
{
    private PromotionHandler promotionHandler;

    public MoveExecutor()
    {
        promotionHandler = new PromotionHandler();
    }

    public void ExecuteMove(Move move, ChessBoard board)
    {
        if (board.GetPiece(move.To) != null)
        {
            board.RemovePiece(move.To);
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