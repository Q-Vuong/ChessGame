using UnityEngine;

public class MoveSimulator
{
    private ChessPiece? _capturedPiece;
    private Vector2Int _rookOriginalPosition;

    public void Simulate(Move move, ChessBoard board)
    {
        _capturedPiece = default;

        switch (move)
        {
            case EnPassantMove enPassantMove:
                _capturedPiece = board.GetPiece(enPassantMove.CapturedPosition);
                break;

            case CastlingMove castlingMove:
                _rookOriginalPosition = castlingMove.RookPiece.Position;
                MoveRook(castlingMove, board);
                break;

            default:
                _capturedPiece = board.GetPiece(move.To);
                break;
        }

        MovePiece(move, board);
    }

    public void Undo(Move move, ChessBoard board)
    {
        board.RemovePiece(move.To);
        board.AddPiece(move.Piece, move.From);
        move.Piece.UpdatePosition(move.From);

        if (_capturedPiece != null)
        {
            board.AddPiece(_capturedPiece, _capturedPiece.Position);
        }

        if (move is CastlingMove castlingMove)
        {
            UndoRookMove(castlingMove, board);
        }
    }

    private void MovePiece(Move move, ChessBoard board)
    {
        if (_capturedPiece != null)
        {
            board.RemovePiece(_capturedPiece.Position);
        }

        board.RemovePiece(move.From);
        board.AddPiece(move.Piece, move.To);
        move.Piece.UpdatePosition(move.To);
    }

    private void MoveRook(CastlingMove move, ChessBoard board)
    {
        ChessPiece rook = move.RookPiece;

        board.RemovePiece(rook.Position);
        board.AddPiece(rook, move.RookTo);
        rook.UpdatePosition(move.RookTo);
    }

    private void UndoRookMove(CastlingMove move, ChessBoard board)
    {
        ChessPiece rook = move.RookPiece;

        board.RemovePiece(rook.Position);
        board.AddPiece(rook, _rookOriginalPosition);
        rook.UpdatePosition(_rookOriginalPosition);
    }
}