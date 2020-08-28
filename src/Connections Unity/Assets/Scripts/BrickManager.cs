using System;
using Objects;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickManager : MonoBehaviour
{
    public Camera camera;
    public GridManager grid;
    public BrickType type;
    public Direction initialFacingDirection;
    private Direction _facingDirection;
    public BrickColors colors;

    private bool _isMoving;

    private Vector3 _lastMousePosition;

    private bool IsVertical => (type & BrickType.Vertical) != 0;
    private bool IsHorizontal => (type & BrickType.Horizontal) != 0;
    private bool IsRotation => (type & BrickType.Rotation) != 0;

    private void Start()
    {
        colors.ChangeColor(this);
        _facingDirection = initialFacingDirection;
        transform.rotation = Quaternion.Euler(0, 0, 180f - 90f * (int)_facingDirection);
    }

    private void OnMouseUp()
    {
        Debug.Log($"You clicked me {name}");

        if (IsRotation && !_isMoving)
        {
            RotateBrick();
        }

        _isMoving = false;

        SnapToGrid();
    }

    private void OnMouseOver()
    {
        _lastMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (!IsHorizontal && !IsVertical)
            return;

        if (Vector3.Distance(Input.mousePosition, _lastMousePosition) < .1f)
            return;
        
        _isMoving = true;
        
        var worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        if (IsHorizontal && !IsVertical)
        {
            worldPosition.y = transform.position.y;
        }

        if (IsVertical && !IsHorizontal)
        {
            worldPosition.x = transform.position.x;
        }
        transform.position = worldPosition;
    }

    private void SnapToGrid()
    {
        var localX = Mathf.RoundToInt(transform.localPosition.x);
        var localY = Mathf.RoundToInt(transform.localPosition.y);
        
        if (localX < 0)
            localX = 0;
        if (localY < 0)
            localY = 0;
        if (localX > grid.grid.width)
            localX = grid.grid.width;
        if(localY > grid.grid.height)
            localY = grid.grid.height;
        
        transform.localPosition = new Vector3(localX, localY, 0);

    }

    private void RotateBrick()
    {
        _facingDirection = (Direction)(((int)_facingDirection + 1) % 4);
        transform.rotation = Quaternion.Euler(0, 0, 180f - 90f * (int)_facingDirection);
    }

    public void RandomizeVertical(int width)
    {
        if (!IsVertical)
            return;

        var randomPosition = Random.Range(0, width);
        transform.localPosition = new Vector3(transform.localPosition.x, randomPosition, 0);
    }

    public void RandomizeHorizontal(int height)
    {
        if (!IsHorizontal)
            return;

        var randomPosition = Random.Range(0, height);
        transform.localPosition = new Vector3(randomPosition, transform.localPosition.y, 0);
    }

    public void RandomizeRotation()
    {
        if (!IsRotation)
            return;

        var randomDirection = Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0:
                _facingDirection = Direction.Up;
                break;
            case 1:
                _facingDirection = Direction.Right;
                break;
            case 2:
                _facingDirection = Direction.Down;
                break;
            case 3:
                _facingDirection = Direction.Left;
                break;
        }

        transform.rotation = Quaternion.Euler(0, 0, 180f - 90f * (int)_facingDirection);
    }
}