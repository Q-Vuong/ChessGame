using System;
using UnityEngine;

public class TestGame
{
    private ChessBoard _board;
    private MoveValidator _validator;
    private GameStateEvaluator _evaluator;



    public TestGame()
    {
        _board = new ChessBoard();
        _validator = new MoveValidator();
        _evaluator = new GameStateEvaluator(_validator);
    }

    public void RunAllTests(Move lastMove)
    {
        // TestMove();
        // TestNormal(lastMove);
        // TestCheck(lastMove);
        // TestCheckmate(lastMove);
        // TestStalemate(lastMove);

        // TestNormalCapture();
        // TestEnPassant();
        //TestPromotionCapture();

        // TestSimulateNormalCapture();
        // TestSimulateEnPassant();
    }

    private void TestMove()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = _board.GetPiece(new Vector2Int(x, y));

                if (piece != null)
                {
                    Debug.Log($"{piece.GetType().Name} - {piece.Color} - {piece.Position}");
                }
            }
        }

        // ------------------------------

        Pawn pawn = new Pawn(ChessColor.White, new Vector2Int(3, 1));
        Knight knight = new Knight(ChessColor.White, new Vector2Int(1, 0));
        Pawn pawnBlack = new Pawn(ChessColor.Black, new Vector2Int(1, 1));
        Pawn pawnBlack1 = new Pawn(ChessColor.Black, new Vector2Int(1, 2));

        _board.AddPiece(pawnBlack, pawnBlack.Position);
        _board.AddPiece(pawnBlack1, pawnBlack1.Position);

        //Move move = new Move(pawn, new Vector2Int(3, 1), new Vector2Int(3, 3));
        Move move = new Move(knight, new Vector2Int(1, 0), new Vector2Int(2, 2));
    }

    private void TestNormal(Move lastMove)
    {
        _board = new ChessBoard();

        King whiteKing = new King(
            ChessColor.White,
            new Vector2Int(4, 0)
        );

        King blackKing = new King(
            ChessColor.Black,
            new Vector2Int(4, 7)
        );

        _board.AddPiece(whiteKing, whiteKing.Position);
        _board.AddPiece(blackKing, blackKing.Position);

        GameState state = _evaluator.Evaluate(
            ChessColor.White,
            _board,
            lastMove
        );

        Debug.Log($"TestNormal: {state}");
    }

    private void TestCheck(Move lastMove)
    {
        _board = new ChessBoard();

        King whiteKing = new King(
            ChessColor.White,
            new Vector2Int(4, 0)
        );

        King blackKing = new King(
            ChessColor.Black,
            new Vector2Int(0, 7)
        );

        Rook blackRook = new Rook(
            ChessColor.Black,
            new Vector2Int(4, 7)
        );

        _board.AddPiece(whiteKing, whiteKing.Position);
        _board.AddPiece(blackKing, blackKing.Position);
        _board.AddPiece(blackRook, blackRook.Position);

        GameState state = _evaluator.Evaluate(
            ChessColor.White,
            _board,
            lastMove
        );

        Debug.Log($"TestCheck: {state}");
    }

    private void TestCheckmate(Move lastMove)
    {
        _board = new ChessBoard();

        King whiteKing = new King(
            ChessColor.White,
            new Vector2Int(0, 0)
        );

        King blackKing = new King(
            ChessColor.Black,
            new Vector2Int(2, 2)
        );

        Queen blackQueen = new Queen(
            ChessColor.Black,
            new Vector2Int(1, 1)
        );

        _board.AddPiece(whiteKing, whiteKing.Position);
        _board.AddPiece(blackKing, blackKing.Position);
        _board.AddPiece(blackQueen, blackQueen.Position);

        GameState state = _evaluator.Evaluate(
            ChessColor.White,
            _board,
            lastMove
        );

        Debug.Log($"TestCheckmate: {state}");
    }

    private void TestStalemate(Move lastMove)
    {
        _board = new ChessBoard();

        King whiteKing = new King(
            ChessColor.White,
            new Vector2Int(0, 0)
        );

        King blackKing = new King(
            ChessColor.Black,
            new Vector2Int(2, 2)
        );

        Queen blackQueen = new Queen(
            ChessColor.Black,
            new Vector2Int(2, 1)
        );

        _board.AddPiece(whiteKing, whiteKing.Position);
        _board.AddPiece(blackKing, blackKing.Position);
        _board.AddPiece(blackQueen, blackQueen.Position);

        GameState state = _evaluator.Evaluate(
            ChessColor.White,
            _board,
            lastMove
        );

        Debug.Log($"TestStalemate: {state}");
    }

    private void TestNormalCapture()
    {
        _board = new ChessBoard();

        MoveExecutor executor = new MoveExecutor();

        Rook whiteRook = new Rook(
            ChessColor.White,
            new Vector2Int(4, 0)
        );

        Pawn blackPawn = new Pawn(
            ChessColor.Black,
            new Vector2Int(4, 4)
        );

        _board.AddPiece(whiteRook, whiteRook.Position);
        _board.AddPiece(blackPawn, blackPawn.Position);

        Move move = new Move(
            whiteRook,
            new Vector2Int(4, 0),
            new Vector2Int(4, 4)
        );

        executor.ExecuteMove(move, _board);

        ChessPiece? fromPiece = _board.GetPiece(new Vector2Int(4, 0));
        ChessPiece? toPiece = _board.GetPiece(new Vector2Int(4, 4));

        Debug.Log(
            $"TestNormalCapture: " +
            $"From Empty = {fromPiece == null}, " +
            $"To Is Rook = {toPiece is Rook}, " +
            $"To Color = {toPiece?.Color}"
        );
    }

    private void TestEnPassant()
    {
        _board = new ChessBoard();

        MoveExecutor executor = new MoveExecutor();

        Pawn whitePawn = new Pawn(
            ChessColor.White,
            new Vector2Int(1, 4)
        );

        Pawn blackPawn = new Pawn(
            ChessColor.Black,
            new Vector2Int(0, 4)
        );

        _board.AddPiece(whitePawn, whitePawn.Position);
        _board.AddPiece(blackPawn, blackPawn.Position);

        EnPassantMove move = new EnPassantMove(
            whitePawn,
            new Vector2Int(1, 4),
            new Vector2Int(0, 5),
            new Vector2Int(0, 4)
        );

        executor.ExecuteMove(move, _board);

        ChessPiece? fromPiece =
            _board.GetPiece(new Vector2Int(1, 4));

        ChessPiece? destinationPiece =
            _board.GetPiece(new Vector2Int(0, 5));

        ChessPiece? capturedPiece =
            _board.GetPiece(new Vector2Int(0, 4));

        Debug.Log(
            $"TestEnPassant: " +
            $"From Empty = {fromPiece == null}, " +
            $"To Is White Pawn = {destinationPiece is Pawn && destinationPiece.Color == ChessColor.White}, " +
            $"Captured Empty = {capturedPiece == null}"
        );
    }

    private void TestPromotionCapture()
    {
        _board = new ChessBoard();

        MoveExecutor executor = new MoveExecutor();

        Pawn whitePawn = new Pawn(
            ChessColor.White,
            new Vector2Int(6, 6)
        );

        Rook blackRook = new Rook(
            ChessColor.Black,
            new Vector2Int(7, 7)
        );

        _board.AddPiece(whitePawn, whitePawn.Position);
        _board.AddPiece(blackRook, blackRook.Position);

        PromotionMove move = new PromotionMove(
            whitePawn,
            new Vector2Int(6, 6),
            new Vector2Int(7, 7),
            ChessPieceType.Queen
        );

        executor.ExecuteMove(move, _board);

        ChessPiece? fromPiece =
            _board.GetPiece(new Vector2Int(6, 6));

        ChessPiece? destinationPiece =
            _board.GetPiece(new Vector2Int(7, 7));

        Debug.Log(
            $"TestPromotionCapture: " +
            $"From Empty = {fromPiece == null}, " +
            $"To Is Queen = {destinationPiece is Queen}, " +
            $"To Color = {destinationPiece?.Color}"
        );
    }

    private void TestSimulateNormalCapture()
    {
        _board = new ChessBoard();

        MoveSimulator simulator = new MoveSimulator();

        Rook whiteRook = new Rook(
            ChessColor.White,
            new Vector2Int(4, 0)
        );

        Pawn blackPawn = new Pawn(
            ChessColor.Black,
            new Vector2Int(4, 4)
        );

        _board.AddPiece(whiteRook, whiteRook.Position);
        _board.AddPiece(blackPawn, blackPawn.Position);

        Move move = new Move(
            whiteRook,
            new Vector2Int(4, 0),
            new Vector2Int(4, 4)
        );

        simulator.Simulate(move, _board);

        Debug.Log(
            $"TestSimulateNormalCapture - Simulate: " +
            $"From Empty = {_board.GetPiece(new Vector2Int(4, 0)) == null}, " +
            $"To Is Rook = {_board.GetPiece(new Vector2Int(4, 4)) is Rook}"
        );

        simulator.Undo(move, _board);

        Debug.Log(
            $"TestSimulateNormalCapture - Undo: " +
            $"From Is Rook = {_board.GetPiece(new Vector2Int(4, 0)) is Rook}, " +
            $"To Is Pawn = {_board.GetPiece(new Vector2Int(4, 4)) is Pawn}"
        );
    }

    private void TestSimulateEnPassant()
    {
        _board = new ChessBoard();

        MoveSimulator simulator = new MoveSimulator();

        Pawn whitePawn = new Pawn(
            ChessColor.White,
            new Vector2Int(1, 4)
        );

        Pawn blackPawn = new Pawn(
            ChessColor.Black,
            new Vector2Int(0, 4)
        );

        _board.AddPiece(whitePawn, whitePawn.Position);
        _board.AddPiece(blackPawn, blackPawn.Position);

        EnPassantMove move = new EnPassantMove(
            whitePawn,
            new Vector2Int(1, 4),
            new Vector2Int(0, 5),
            new Vector2Int(0, 4)
        );

        simulator.Simulate(move, _board);

        Debug.Log(
            $"TestSimulateEnPassant - Simulate: " +
            $"From Empty = {_board.GetPiece(new Vector2Int(1, 4)) == null}, " +
            $"To Is White Pawn = {_board.GetPiece(new Vector2Int(0, 5)) is Pawn}, " +
            $"Captured Empty = {_board.GetPiece(new Vector2Int(0, 4)) == null}"
        );

        simulator.Undo(move, _board);

        Debug.Log(
            $"TestSimulateEnPassant - Undo: " +
            $"From Is White Pawn = " +
            $"{_board.GetPiece(new Vector2Int(1, 4)) is Pawn && _board.GetPiece(new Vector2Int(1, 4)).Color == ChessColor.White}, " +
            $"Captured Is Black Pawn = " +
            $"{_board.GetPiece(new Vector2Int(0, 4)) is Pawn && _board.GetPiece(new Vector2Int(0, 4)).Color == ChessColor.Black}, " +
            $"To Empty = {_board.GetPiece(new Vector2Int(0, 5)) == null}"
        );
    }
}