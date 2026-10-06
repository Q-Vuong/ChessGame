using System;
using UnityEngine;

public class MoveValidator
{
    private MoveSimulator _moveSimulator = new MoveSimulator();

    public bool IsValidMove(Move move, Move lastMove, ChessBoard board)
    {
        if (move.From != move.Piece.Position)
            return false;

        if (board.GetPiece(move.From) != move.Piece)
            return false;

        if (move is PromotionMove promotionMove)
        {
            if (promotionMove.Piece is not Pawn || (promotionMove.To.y != 7 && promotionMove.To.y != 0))
                return false;

            if (promotionMove.PromotionType == ChessPieceType.Pawn || promotionMove.PromotionType == ChessPieceType.King)
                return false;
        }

        ChessColor attackerColor = move.Piece.Color == ChessColor.White ? ChessColor.Black : ChessColor.White;

        if (move is EnPassantMove enPassantMove)
        {
            if (!IsValidEnPassant(enPassantMove, lastMove, board))
                return false;
        }
        else if (move is CastlingMove castlingMove)
        {
            if (!IsValidCastling(castlingMove, attackerColor, board))
                return false;
        }
        else
        {
            ChessPiece? destination = board.GetPiece(move.To);

            if (destination == null)
            {
                if (!move.Piece.CanMove(move.From, move.To))
                    return false;
            }
            else
            {
                if (destination.Color == move.Piece.Color)
                {
                    return false;
                }
                else
                {
                    if (!move.Piece.CanAttack(move.From, move.To))
                        return false;
                }
            }
        }

        if (move.Piece is Pawn)
        {
            int dY = Math.Abs(move.To.y - move.From.y);

            if (dY == 2 && !IsPathClear(move.From, move.To, board))
                return false;
        }

        if (move.Piece.NeedsPathCheck)
        {
            if (!IsPathClear(move.From, move.To, board))
                return false;
        }

        if (!IsKingSafe(move, attackerColor, board))
            return false;

        return true;
    }

    private bool IsPathClear(Vector2Int from, Vector2Int to, ChessBoard board)
    {
        int dx = to.x - from.x;
        int dy = to.y - from.y;

        int directionX = (int)Mathf.Sign(dx);
        int directionY = (int)Mathf.Sign(dy);

        if (dx == 0 && dy != 0)
        {
            for (int y = from.y + directionY; y != to.y; y += directionY)
            {
                if (board.GetPiece(new Vector2Int(from.x, y)) != null)
                    return false;
            }
        }
        else if (dx != 0 && dy == 0)
        {
            for (int x = from.x + directionX; x != to.x; x += directionX)
            {
                if (board.GetPiece(new Vector2Int(x, from.y)) != null)
                    return false;
            }
        }
        else if (Mathf.Abs(dx) == Mathf.Abs(dy))
        {
            for (Vector2Int x = from + new Vector2Int(directionX, directionY); x != to; x += new Vector2Int(directionX, directionY))
            {
                if (board.GetPiece(x) != null)
                    return false;
            }
        }
        return true;
    }

    public bool IsSquareAttacked(Vector2Int targetSquare, ChessColor attackerColor, ChessBoard board)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece? piece = board.Board[x, y];
                if (piece != null)
                {
                    if (piece.Color == attackerColor)
                    {
                        if (piece.CanAttack(piece.Position, targetSquare))
                        {
                            if (piece.NeedsPathCheck)
                            {
                                if (IsPathClear(piece.Position, targetSquare, board))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    private bool IsKingSafe(Move move, ChessColor attackerColor, ChessBoard board)
    {
        bool isSafe = true;

        _moveSimulator.Simulate(move, board);

        King king = move.Piece is King piece ? piece : board.FindKing(move.Piece.Color);

        if (IsSquareAttacked(king.Position, attackerColor, board))
            isSafe = false;

        _moveSimulator.Undo(move, board);

        return isSafe;
    }

    private bool IsValidEnPassant(EnPassantMove move, Move lastMove, ChessBoard board)
    {
        ChessPiece capturingPiece = move.Piece;
        ChessPiece capturedPiece = board.GetPiece(move.CapturedPosition);

        if (board.GetPiece(move.To) != null)
            return false;
        if (capturingPiece is not Pawn || capturedPiece is not Pawn)
            return false;
        if (capturingPiece.Color == capturedPiece.Color)
            return false;
        if (capturedPiece != lastMove.Piece)
            return false;

        int dy = Math.Abs(lastMove.To.y - lastMove.From.y);
        if (dy != 2)
            return false;

        int dx = Math.Abs(capturingPiece.Position.x - capturedPiece.Position.x);
        if (dx != 1)
            return false;

        int middleY = (lastMove.From.y + lastMove.To.y) / 2;
        if (move.To.y != middleY || move.To.x != capturedPiece.Position.x)
            return false;

        return true;
    }

    private bool IsValidCastling(CastlingMove move, ChessColor attackerColor, ChessBoard board)
    {
        if (move.Piece is not King || move.RookPiece is not Rook)
            return false;

        if (move.Piece.Color != move.RookPiece.Color)
            return false;

        if (move.Piece.HasMoved || move.RookPiece.HasMoved)
            return false;

        int dx = Math.Abs(move.To.x - move.From.x);
        int y = move.Piece.Color == ChessColor.White ? 0 : 7;
        if (dx != 2 || move.From.y != y)
            return false;

        int PosRook_X = move.To.x > move.From.x ? 7 : 0;
        if (move.From.y != move.RookPiece.Position.y || move.RookPiece.Position.x != PosRook_X)
            return false;

        if (!IsPathClear(move.From, move.RookPiece.Position, board))
            return false;

        int kingDirection = move.To.x > move.From.x ? 1 : -1;
        Vector2Int kingPosition = move.From;
        Vector2Int kingMoved = new Vector2Int(kingPosition.x + kingDirection, move.From.y);
        if (IsSquareAttacked(kingPosition, attackerColor, board) || IsSquareAttacked(kingMoved, attackerColor, board))
            return false;

        return true;
    }
}