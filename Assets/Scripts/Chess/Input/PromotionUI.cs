using System;
using UnityEngine;

public class PromotionUI : MonoBehaviour
{
    public event Action<ChessPieceType> PromotionSelected;

    public void SelectQueen()
    {
        PromotionSelected?.Invoke(ChessPieceType.Queen);
    }

    public void SelectRook()
    {
        PromotionSelected?.Invoke(ChessPieceType.Rook);
    }

    public void SelectBishop()
    {
        PromotionSelected?.Invoke(ChessPieceType.Bishop);
    }

    public void SelectKnight()
    {
        PromotionSelected?.Invoke(ChessPieceType.Knight);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
