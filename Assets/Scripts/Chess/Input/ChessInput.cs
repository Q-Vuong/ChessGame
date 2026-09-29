using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChessInput : MonoBehaviour
{
    private InputSystem_Actions _input;
    public event Action<Vector2Int> SquareClicked;

    private void Awake()
    {
        _input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _input.Chess.Enable();
        _input.Chess.Select.performed += OnSelect;
    }

    private void OnDisable()
    {
        _input.Chess.Select.performed -= OnSelect;
        _input.Chess.Disable();
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        //Debug.Log($"INPUT: Selected {screenPosition}");

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //Debug.Log(hit.collider.gameObject.name);
            Vector3 worldPosition = hit.collider.gameObject.transform.position;

            Vector2Int boardPosition = new Vector2Int((int)worldPosition.x, (int)worldPosition.z);
            SquareClicked?.Invoke(boardPosition);
        }
    }
}
