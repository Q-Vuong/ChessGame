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
    private ChessInput _chessInput;
    private PromotionUI _promotionUI;

    [SerializeField]
    private ChessColor _currentTurn = ChessColor.White;
    private bool _gameOver;
    private Move _lastMove;
    private ChessPiece _selectedPiece;
    private Vector2Int _selectedTarget;
    private Move _pendingPromotionMove;

    private void Awake()
    {
        _chessInput = FindAnyObjectByType<ChessInput>();
        _promotionUI = FindAnyObjectByType<PromotionUI>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _promotionUI.Hide();

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

        //TestGame testGame = new TestGame();
        // testGame.RunAllTests(lastMove);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        _chessInput.SquareClicked += OnSquareClicked;
        _promotionUI.PromotionSelected += OnPromotionSelected;
    }


    void OnDisable()
    {
        _chessInput.SquareClicked -= OnSquareClicked;
        _promotionUI.PromotionSelected -= OnPromotionSelected;
    }

    private void OnSquareClicked(Vector2Int position)
    {
        if (_pendingPromotionMove != null)
            return;

        ChessPiece? piece = _board.GetPiece(position);

        if (_selectedPiece == null)
        {
            if (_gameOver)
                return;

            if (piece == null)
                return;

            if (piece.Color != _currentTurn)
                return;

            _selectedPiece = piece;
            Debug.Log($"Selected: {_selectedPiece.GetType().Name} - {_selectedPiece.Color}");
        }
        else
        {
            if (piece != null && piece.Color == _currentTurn)
            {
                _selectedPiece = piece;
                Debug.Log($"Selected: {_selectedPiece.GetType().Name} - {_selectedPiece.Color}");
                return;
            }

            _selectedTarget = position;

            Move move = CreateMove(_selectedPiece, _selectedTarget);

            MoveResult result = TryMakeMove(move);

            if (result == MoveResult.Success)
            {
                MoveExecutorResult moveResult = CompleteMove(move);
                HandleMoveCompleted(move, moveResult.CapturedPiece, moveResult.NewPiece);

                Debug.Log("Success move");
            }
            else if (result == MoveResult.Failed)
            {
                Debug.Log("Fail move");
            }
        }
    }

    private void OnPromotionSelected(ChessPieceType type)
    {
        Debug.Log($"Player selected: {type}");
        PromotionMove promotionMove = new PromotionMove(_pendingPromotionMove.Piece, _pendingPromotionMove.From, _pendingPromotionMove.To, type);
        MoveExecutorResult moveResult = CompleteMove(promotionMove);
        HandleMoveCompleted(promotionMove, moveResult.CapturedPiece, moveResult.NewPiece);
        _promotionUI.Hide();
    }

    private Move CreateMove(ChessPiece selectedPiece, Vector2Int target)
    {
        if (IsEnPassant(selectedPiece, target, _lastMove))
        {
            return new EnPassantMove(selectedPiece, selectedPiece.Position, target, _lastMove.To);
        }

        return new Move(selectedPiece, selectedPiece.Position, target);
    }

    private MoveResult TryMakeMove(Move move)
    {
        if (_gameOver)
        {
            Debug.Log("Game Finished cannot move");
            return MoveResult.Failed;
        }

        if (move.Piece.Color != _currentTurn)
            return MoveResult.Failed;

        if (!_validator.IsValidMove(move, _lastMove, _board))
        {
            Debug.Log($"Validator failed. Move type: {move.GetType().Name}");
            return MoveResult.Failed;
        }

        if (IsPromotion(move))
        {
            _pendingPromotionMove = move;
            _promotionUI.Show();
            return MoveResult.WaitingForPromotion;
        }
        else
        {
            return MoveResult.Success;
        }
    }

    private MoveExecutorResult CompleteMove(Move move)
    {
        MoveExecutorResult result = _moveExecutor.ExecuteMove(move, _board);

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

        return result;
    }

    private void HandleMoveCompleted(Move move, ChessPiece? capturedPiece, ChessPiece? newPiece)
    {
        switch (move)
        {
            case PromotionMove promotionMove:
                if (newPiece != null)
                {
                    _piecesView.ReplacePieceView(promotionMove.Piece, newPiece, promotionMove.To);

                }
                break;
            case Move:
                _piecesView.UpdatePiecePosition(move.Piece, move.To);
                break;
        }

        if (capturedPiece != null)
        {
            _piecesView.RemovePiece(capturedPiece);
        }

        _selectedPiece = default;
        _selectedTarget = default;
        _pendingPromotionMove = default;
    }

    private bool IsEnPassant(ChessPiece selectedPiece, Vector2 target, Move? lastMove)
    {
        Debug.Log($"Selected: {selectedPiece.Position}");
        Debug.Log($"Target: {target}");
        Debug.Log($"LastMove: {lastMove?.From} -> {lastMove?.To}");
        
        if (lastMove == null)
            return false;

        if (selectedPiece is not Pawn || lastMove.Piece is not Pawn)
            return false;

        if (selectedPiece.Color == lastMove.Piece.Color)
            return false;

        if (lastMove.From.y != 1 && lastMove.From.y != 6)
            return false;

        int dy = Math.Abs(lastMove.To.y - lastMove.From.y);
        if (dy != 2)
            return false;

        int dx = Math.Abs(selectedPiece.Position.x - lastMove.From.x);
        if (lastMove.From.y != selectedPiece.Position.y || dx != 1)
            return false;

        int middleY = (lastMove.From.y + lastMove.To.y) / 2;
        if (target.y != middleY)
            return false;

        return true;
    }

    private bool IsPromotion(Move move)
    {
        if (move.Piece is not Pawn)
            return false;

        if (move.To.y != 0 && move.To.y != 7)
            return false;

        return true;
    }

}
