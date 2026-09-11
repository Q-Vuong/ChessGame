public class MoveSimulator {
    private ChessPiece? capturedPiece;

    public void Simulate(Move move, ChessBoard board)
    {
        capturedPiece = board.GetPiece(move.To);

        board.RemovePiece(move.From);

        if(capturedPiece != null)
        {
            board.RemovePiece(move.To);
        }

        board.AddPiece(move.Piece, move.To);
        move.Piece.UpdatePosition(move.To);
    }

    public void Undo(Move move, ChessBoard board)
    {
        board.RemovePiece(move.To);
        board.AddPiece(move.Piece, move.From);

        if(capturedPiece != null)
        {
            board.AddPiece(capturedPiece, move.To);
        }
        
        move.Piece.UpdatePosition(move.From);
    }
}