using UnityEngine;

public class TestGame
{
    private ChessBoard board;
    private MoveValidator validator;
    private GameStateEvaluator evaluator;

    public TestGame()
    {
        board = new ChessBoard();
        validator = new MoveValidator();
        evaluator = new GameStateEvaluator(validator);
    }

    public void RunAllTests(Move lastMove)
    {
        // TestMove();
        // TestNormal(lastMove);
        // TestCheck(lastMove);
        // TestCheckmate(lastMove);
        // TestStalemate(lastMove);

        TestNormalCapture();
        TestEnPassant();
        //TestPromotionCapture();

        TestSimulateNormalCapture();
        TestSimulateEnPassant();
    }

    private void TestMove()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = board.GetPiece(new Vector2Int(x, y));

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

        board.AddPiece(pawnBlack, pawnBlack.Position);
        board.AddPiece(pawnBlack1, pawnBlack1.Position);

        //Move move = new Move(pawn, new Vector2Int(3, 1), new Vector2Int(3, 3));
        Move move = new Move(knight, new Vector2Int(1, 0), new Vector2Int(2, 2));
    }

    private void TestNormal(Move lastMove)
    {
        board = new ChessBoard();

        King whiteKing = new King(
            ChessColor.White,
            new Vector2Int(4, 0)
        );

        King blackKing = new King(
            ChessColor.Black,
            new Vector2Int(4, 7)
        );

        board.AddPiece(whiteKing, whiteKing.Position);
        board.AddPiece(blackKing, blackKing.Position);

        GameState state = evaluator.Evaluate(
            ChessColor.White,
            board,
            lastMove
        );

        Debug.Log($"TestNormal: {state}");
    }

    private void TestCheck(Move lastMove)
    {
        board = new ChessBoard();

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

        board.AddPiece(whiteKing, whiteKing.Position);
        board.AddPiece(blackKing, blackKing.Position);
        board.AddPiece(blackRook, blackRook.Position);

        GameState state = evaluator.Evaluate(
            ChessColor.White,
            board,
            lastMove
        );

        Debug.Log($"TestCheck: {state}");
    }

    private void TestCheckmate(Move lastMove)
    {
        board = new ChessBoard();

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

        board.AddPiece(whiteKing, whiteKing.Position);
        board.AddPiece(blackKing, blackKing.Position);
        board.AddPiece(blackQueen, blackQueen.Position);

        GameState state = evaluator.Evaluate(
            ChessColor.White,
            board,
            lastMove
        );

        Debug.Log($"TestCheckmate: {state}");
    }

    private void TestStalemate(Move lastMove)
    {
        board = new ChessBoard();

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

        board.AddPiece(whiteKing, whiteKing.Position);
        board.AddPiece(blackKing, blackKing.Position);
        board.AddPiece(blackQueen, blackQueen.Position);

        GameState state = evaluator.Evaluate(
            ChessColor.White,
            board,
            lastMove
        );

        Debug.Log($"TestStalemate: {state}");
    }

    private void TestNormalCapture()
    {
        board = new ChessBoard();

        MoveExecutor executor = new MoveExecutor();

        Rook whiteRook = new Rook(
            ChessColor.White,
            new Vector2Int(4, 0)
        );

        Pawn blackPawn = new Pawn(
            ChessColor.Black,
            new Vector2Int(4, 4)
        );

        board.AddPiece(whiteRook, whiteRook.Position);
        board.AddPiece(blackPawn, blackPawn.Position);

        Move move = new Move(
            whiteRook,
            new Vector2Int(4, 0),
            new Vector2Int(4, 4)
        );

        executor.ExecuteMove(move, board);

        ChessPiece? fromPiece = board.GetPiece(new Vector2Int(4, 0));
        ChessPiece? toPiece = board.GetPiece(new Vector2Int(4, 4));

        Debug.Log(
            $"TestNormalCapture: " +
            $"From Empty = {fromPiece == null}, " +
            $"To Is Rook = {toPiece is Rook}, " +
            $"To Color = {toPiece?.Color}"
        );
    }

    private void TestEnPassant()
    {
        board = new ChessBoard();

        MoveExecutor executor = new MoveExecutor();

        Pawn whitePawn = new Pawn(
            ChessColor.White,
            new Vector2Int(1, 4)
        );

        Pawn blackPawn = new Pawn(
            ChessColor.Black,
            new Vector2Int(0, 4)
        );

        board.AddPiece(whitePawn, whitePawn.Position);
        board.AddPiece(blackPawn, blackPawn.Position);

        EnPassantMove move = new EnPassantMove(
            whitePawn,
            new Vector2Int(1, 4),
            new Vector2Int(0, 5),
            new Vector2Int(0, 4)
        );

        executor.ExecuteMove(move, board);

        ChessPiece? fromPiece =
            board.GetPiece(new Vector2Int(1, 4));

        ChessPiece? destinationPiece =
            board.GetPiece(new Vector2Int(0, 5));

        ChessPiece? capturedPiece =
            board.GetPiece(new Vector2Int(0, 4));

        Debug.Log(
            $"TestEnPassant: " +
            $"From Empty = {fromPiece == null}, " +
            $"To Is White Pawn = {destinationPiece is Pawn && destinationPiece.Color == ChessColor.White}, " +
            $"Captured Empty = {capturedPiece == null}"
        );
    }

    private void TestPromotionCapture()
    {
        board = new ChessBoard();

        MoveExecutor executor = new MoveExecutor();

        Pawn whitePawn = new Pawn(
            ChessColor.White,
            new Vector2Int(6, 6)
        );

        Rook blackRook = new Rook(
            ChessColor.Black,
            new Vector2Int(7, 7)
        );

        board.AddPiece(whitePawn, whitePawn.Position);
        board.AddPiece(blackRook, blackRook.Position);

        PromotionMove move = new PromotionMove(
            whitePawn,
            new Vector2Int(6, 6),
            new Vector2Int(7, 7),
            ChessPieceType.Queen
        );

        executor.ExecuteMove(move, board);

        ChessPiece? fromPiece =
            board.GetPiece(new Vector2Int(6, 6));

        ChessPiece? destinationPiece =
            board.GetPiece(new Vector2Int(7, 7));

        Debug.Log(
            $"TestPromotionCapture: " +
            $"From Empty = {fromPiece == null}, " +
            $"To Is Queen = {destinationPiece is Queen}, " +
            $"To Color = {destinationPiece?.Color}"
        );
    }

    private void TestSimulateNormalCapture()
    {
        board = new ChessBoard();

        MoveSimulator simulator = new MoveSimulator();

        Rook whiteRook = new Rook(
            ChessColor.White,
            new Vector2Int(4, 0)
        );

        Pawn blackPawn = new Pawn(
            ChessColor.Black,
            new Vector2Int(4, 4)
        );

        board.AddPiece(whiteRook, whiteRook.Position);
        board.AddPiece(blackPawn, blackPawn.Position);

        Move move = new Move(
            whiteRook,
            new Vector2Int(4, 0),
            new Vector2Int(4, 4)
        );

        simulator.Simulate(move, board);

        Debug.Log(
            $"TestSimulateNormalCapture - Simulate: " +
            $"From Empty = {board.GetPiece(new Vector2Int(4, 0)) == null}, " +
            $"To Is Rook = {board.GetPiece(new Vector2Int(4, 4)) is Rook}"
        );

        simulator.Undo(move, board);

        Debug.Log(
            $"TestSimulateNormalCapture - Undo: " +
            $"From Is Rook = {board.GetPiece(new Vector2Int(4, 0)) is Rook}, " +
            $"To Is Pawn = {board.GetPiece(new Vector2Int(4, 4)) is Pawn}"
        );
    }

    private void TestSimulateEnPassant()
    {
        board = new ChessBoard();

        MoveSimulator simulator = new MoveSimulator();

        Pawn whitePawn = new Pawn(
            ChessColor.White,
            new Vector2Int(1, 4)
        );

        Pawn blackPawn = new Pawn(
            ChessColor.Black,
            new Vector2Int(0, 4)
        );

        board.AddPiece(whitePawn, whitePawn.Position);
        board.AddPiece(blackPawn, blackPawn.Position);

        EnPassantMove move = new EnPassantMove(
            whitePawn,
            new Vector2Int(1, 4),
            new Vector2Int(0, 5),
            new Vector2Int(0, 4)
        );

        simulator.Simulate(move, board);

        Debug.Log(
            $"TestSimulateEnPassant - Simulate: " +
            $"From Empty = {board.GetPiece(new Vector2Int(1, 4)) == null}, " +
            $"To Is White Pawn = {board.GetPiece(new Vector2Int(0, 5)) is Pawn}, " +
            $"Captured Empty = {board.GetPiece(new Vector2Int(0, 4)) == null}"
        );

        simulator.Undo(move, board);

        Debug.Log(
            $"TestSimulateEnPassant - Undo: " +
            $"From Is White Pawn = " +
            $"{board.GetPiece(new Vector2Int(1, 4)) is Pawn && board.GetPiece(new Vector2Int(1, 4)).Color == ChessColor.White}, " +
            $"Captured Is Black Pawn = " +
            $"{board.GetPiece(new Vector2Int(0, 4)) is Pawn && board.GetPiece(new Vector2Int(0, 4)).Color == ChessColor.Black}, " +
            $"To Empty = {board.GetPiece(new Vector2Int(0, 5)) == null}"
        );
    }
}