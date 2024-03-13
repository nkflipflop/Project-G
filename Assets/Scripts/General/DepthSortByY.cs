using UnityEngine;

public class DepthSortByY : MonoBehaviour
{
    private const int ISOMETRIC_RANGE_PER_Y_UNIT = 12;

    [SerializeField] private bool _walkingObject = false;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.sortingOrder = -(int)(transform.position.y * ISOMETRIC_RANGE_PER_Y_UNIT); // setting its order according to its y position
    }

    private void Update()
    {
        if (_walkingObject)
        {
            _renderer.sortingOrder = -(int)(transform.position.y * ISOMETRIC_RANGE_PER_Y_UNIT); // setting its order according to its y position
        }
    }
}