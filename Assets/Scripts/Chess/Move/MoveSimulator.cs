using UnityEngine;

public class MoveSimulator
{
    private ChessPiece? capturedPiece;
    private Vector2Int capturedPosition;

    public void Simulate(Move move, ChessBoard board)
    {
        switch (move)
        {
            case EnPassantMove enPassantMove:
                capturedPiece = board.GetPiece(enPassantMove.CapturedPosition);
                capturedPosition = enPassantMove.CapturedPosition;
                break;
            case Move:
                capturedPiece = board.GetPiece(move.To);
                capturedPosition = move.To;
                break;
        }

        board.RemovePiece(move.From);

        if (capturedPiece != null)
        {
            board.RemovePiece(capturedPosition);
        }

        board.AddPiece(move.Piece, move.To);
        move.Piece.UpdatePosition(move.To);
    }

    public void Undo(Move move, ChessBoard board)
    {
        board.RemovePiece(move.To);
        board.AddPiece(move.Piece, move.From);

        if (capturedPiece != null)
        {
            board.AddPiece(capturedPiece, capturedPosition);
        }

        move.Piece.UpdatePosition(move.From);
    }
}