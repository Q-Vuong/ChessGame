public class MoveExecutor
{
    public void ExecuteMove(Move move, ChessBoard board)
    {
        if(board.GetPiece(move.To) != null)
        {
            board.RemovePiece(move.To);
        }

        board.RemovePiece(move.From);
        board.AddPiece(move.Piece, move.To);
        move.Piece.UpdatePosition(move.To);
    }
}