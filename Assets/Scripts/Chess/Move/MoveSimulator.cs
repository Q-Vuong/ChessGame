using UnityEngine;

public class MoveSimulator
{
    private ChessPiece? _capturedPiece;
    private Vector2Int _capturedPosition;

    public void Simulate(Move move, ChessBoard board)
    {
        switch (move)
        {
            case EnPassantMove enPassantMove:
                _capturedPiece = board.GetPiece(enPassantMove.CapturedPosition);
                _capturedPosition = enPassantMove.CapturedPosition;
                break;
            case Move:
                _capturedPiece = board.GetPiece(move.To);
                _capturedPosition = move.To;
                break;
        }

        board.RemovePiece(move.From);

        if (_capturedPiece != null)
        {
            board.RemovePiece(_capturedPosition);
        }

        board.AddPiece(move.Piece, move.To);
        move.Piece.UpdatePosition(move.To);
    }

    public void Undo(Move move, ChessBoard board)
    {
        board.RemovePiece(move.To);
        board.AddPiece(move.Piece, move.From);

        if (_capturedPiece != null)
        {
            board.AddPiece(_capturedPiece, _capturedPosition);
        }

        move.Piece.UpdatePosition(move.From);
    }
}