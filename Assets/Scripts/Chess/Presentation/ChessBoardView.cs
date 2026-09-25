using UnityEngine;

public class ChessBoardView : MonoBehaviour
{
    [SerializeField]
    private GameObject _whiteSquarePrefab;
    [SerializeField]
    private GameObject _blackSquarePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Render()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Vector3 position = new Vector3(x, 0, y);

                if ((x + y) % 2 == 0)
                {
                    Instantiate(_whiteSquarePrefab, position, _whiteSquarePrefab.transform.rotation, transform);
                }
                else
                {
                    Instantiate(_blackSquarePrefab, position, _blackSquarePrefab.transform.rotation, transform);
                }
            }
        }
    }
}
