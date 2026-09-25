using System;
using UnityEngine;

public class ChessGame : MonoBehaviour
{
    private ChessBoard _board;
    private BoardSetup _boardSetup;
    private MoveValidator _validator;
    private MoveExecutor _moveExecutor;
    private GameStateEvaluator _stateEvaluator;
    private ChessBoardView _boardView;
    private ChessPiecesView _piecesView;

    private ChessColor _currentTurn = ChessColor.White;
    private bool _gameOver;
    private Move _lastMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _board = new ChessBoard();
        _boardSetup = new BoardSetup();
        _validator = new MoveValidator();
        _moveExecutor = new MoveExecutor();
        _stateEvaluator = new GameStateEvaluator(_validator);
        _boardView = FindAnyObjectByType<ChessBoardView>();
        _piecesView = FindAnyObjectByType<ChessPiecesView>();

        _boardSetup.Setup(_board);
        _boardView.Render();
        _piecesView.Render(_board);

        // TestGame testGame = new TestGame();
        // testGame.RunAllTests(lastMove);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MakeMove(Move move)
    {
        if (_gameOver)
        {
            Debug.Log("Game Finished cannot move");
            return;
        }

        if (move.Piece.Color != _currentTurn)
            return;

        if(IsEnPassant(move, _lastMove))
        {
            move = new EnPassantMove(move.Piece, move.From, move.To, _lastMove.Piece.Position); 
        }

        if (!_validator.IsValidMove(move, _lastMove, _board))
            return;

        _moveExecutor.ExecuteMove(move, _board);

        _lastMove = move;

        _currentTurn = _currentTurn == ChessColor.White ? ChessColor.Black : ChessColor.White;

        GameState state = _stateEvaluator.Evaluate(_currentTurn, _board, _lastMove);

        switch (state)
        {
            case GameState.Normal:
                Debug.Log("NORMAL => Continue");
                break;
            case GameState.Check:
                Debug.Log("CHECK => Continue");
                break;
            case GameState.Checkmate:
                _gameOver = true;
                Debug.Log("CHECKMATE => Game Finish");
                break;
            case GameState.Stalemate:
                _gameOver = true;
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
