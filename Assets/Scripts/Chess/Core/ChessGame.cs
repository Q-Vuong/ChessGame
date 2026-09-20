using System;
using UnityEngine;

public class ChessGame : MonoBehaviour
{
    private ChessBoard board;
    private BoardSetup boardSetup;
    private MoveValidator validator;
    private MoveExecutor moveExecutor;
    private GameStateEvaluator stateEvaluator;
    private ChessBoardView boardView;
    private ChessPiecesView piecesView;

    private ChessColor currentTurn = ChessColor.White;
    private bool gameOver;
    private Move lastMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = new ChessBoard();
        boardSetup = new BoardSetup();
        validator = new MoveValidator();
        moveExecutor = new MoveExecutor();
        stateEvaluator = new GameStateEvaluator(validator);
        boardView = FindAnyObjectByType<ChessBoardView>();
        piecesView = FindAnyObjectByType<ChessPiecesView>();

        boardSetup.Setup(board);
        boardView.Render();
        piecesView.Render(board);

        // TestGame testGame = new TestGame();
        // testGame.RunAllTests(lastMove);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MakeMove(Move move)
    {
        if (gameOver)
        {
            Debug.Log("Game Finished cannot move");
            return;
        }

        if (move.Piece.Color != currentTurn)
            return;

        if(IsEnPassant(move, lastMove))
        {
            move = new EnPassantMove(move.Piece, move.From, move.To, lastMove.Piece.Position); 
        }

        if (!validator.IsValidMove(move, lastMove, board))
            return;

        moveExecutor.ExecuteMove(move, board);

        lastMove = move;

        currentTurn = currentTurn == ChessColor.White ? ChessColor.Black : ChessColor.White;

        GameState state = stateEvaluator.Evaluate(currentTurn, board, lastMove);

        switch (state)
        {
            case GameState.Normal:
                Debug.Log("NORMAL => Continue");
                break;
            case GameState.Check:
                Debug.Log("CHECK => Continue");
                break;
            case GameState.Checkmate:
                gameOver = true;
                Debug.Log("CHECKMATE => Game Finish");
                break;
            case GameState.Stalemate:
                gameOver = true;
                Debug.Log("Stalemate => Game Finish");
                break;
        }
    }

    private bool IsEnPassant(Move move, Move? lastMove)
    {
        if (lastMove == null)
            return false;

        if (move.Piece is not Pawn || lastMove.Piece is not Pawn)
            return false;

        if (move.Piece.Color == lastMove.Piece.Color)
            return false;

        if (lastMove.From.y != 1 && lastMove.From.y != 6)
            return false;

        int dy = Math.Abs(lastMove.To.y - lastMove.From.y);
        if (dy != 2)
            return false;

        int dx = Math.Abs(move.Piece.Position.x - lastMove.Piece.Position.x);
        if (lastMove.Piece.Position.y != move.Piece.Position.y || dx != 1)
            return false;

        int middleY = (lastMove.From.y + lastMove.To.y) / 2;
        if (move.To.y != middleY)
            return false;

        return true;
    }
}
