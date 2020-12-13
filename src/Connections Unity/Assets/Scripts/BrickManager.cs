using System.Collections.Generic;
using System.Linq;
using Objects;
using ScriptableObjects;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [Header("External components")]
    public Camera mainCamera;
    public GridManager gridManager;
    public BrickColors colors;
    
    [Header("Brick variables")]
    public BrickType type;
    public Direction initialFacingDirection;

    [Header("Connector")]
    public GameObject connectorPrefab;
    public Transform connectorParent;

    private Direction _facingDirection;
    private bool _isMoving;

    private Vector3 _lastMousePosition;
    private Vector2 _lastPosition;

    private bool _stop;

    public bool IsVertical => (type & BrickType.Vertical) != 0;
    public bool IsHorizontal => (type & BrickType.Horizontal) != 0;
    public bool IsRotation => (type & BrickType.Rotation) != 0;

    private const float ConnectorBlockedPosition = 0.5625f;

    public delegate void VoidDelegate();

    public event VoidDelegate MouseUpEvent; 

    private void Start()
    {
        colors.ChangeColor(this);
        _facingDirection = initialFacingDirection;
        connectorParent.rotation = Quaternion.Euler(0, 0, _facingDirection.ToAngleRotation());
    }

    private void OnMouseUp()
    {
        if (_stop) return;
        
        if (IsRotation && !_isMoving)
        {
            RotateBrick();
        }

        if (_isMoving)
        {
            _isMoving = false;
            var newPosition = SnapToGrid();
            gridManager.MoveBrickToPosition(transform, newPosition, _lastPosition);
        }
        
        MouseUpEvent?.Invoke();
    }

    private void OnMouseOver()
    {
        _lastMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        if (_stop) return;
        
        if (!IsHorizontal && !IsVertical)
            return;

        if (Vector3.Distance(Input.mousePosition, _lastMousePosition) < .1f)
            return;

        if (!_isMoving)
        {
            _lastPosition = transform.localPosition;
            _isMoving = true;
        }
        
        var worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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

    public void Stop()
    {
        _stop = true;
    }

    public void Resume()
    {
        _stop = false;
    }

    private Vector2 SnapToGrid()
    {
        var localPosition = transform.localPosition;
        var localX = Mathf.RoundToInt(localPosition.x);
        var localY = Mathf.RoundToInt(localPosition.y);

        var width = gridManager.grid.width;
        var height = gridManager.grid.height;
        
        if (localX < 0)
            localX = 0;
        if (localY < 0)
            localY = 0;
        if (localX >= width)
            localX = width - 1;
        if(localY >= height)
            localY = height - 1;
        
        return new Vector2(localX, localY);
    }

    private void RotateBrick()
    {
        _facingDirection = _facingDirection.Add(1);
        gridManager.RotateBrick(transform, connectorParent, _facingDirection);
    }

    public void CreateConnectors(List<Connector> connectors)
    {
        CreateConnectorsOnDirection(connectors, Direction.Up);
        CreateConnectorsOnDirection(connectors, Direction.Right);
        CreateConnectorsOnDirection(connectors, Direction.Left);
        CreateConnectorsOnDirection(connectors, Direction.Down);
    }

    private void CreateConnectorsOnDirection(List<Connector> connectors, Direction direction)
    {
        var numberOfConnectors = connectors
            .Where(c => c.direction == direction)
            .Sum(c => c.size);

        var positions = new List<float>();
        
        switch (numberOfConnectors)
        {
            case 1:
                positions.Add(0);
                break;
            case 2:
                positions.AddRange(new[] {-0.1f, 0.1f});
                break;
            case 3:
                positions.AddRange(new[] {-0.2f, 0f, 0.2f});
                break;
            case 4:
                positions.AddRange(new[] {-0.3f, -0.1f, 0.1f, 0.3f});
                break;
        }

        var connectorPoses = positions.Select(p =>
            new Pose(ConnectorPosition(direction, p), Quaternion.Euler(0, 0, direction.ToAngleRotation())));

        foreach (var pose in connectorPoses)
        {
            var connector = Instantiate(connectorPrefab, connectorParent);
            connector.transform.localPosition = pose.position;
            connector.transform.localRotation = pose.rotation;
        }
    }

    private Vector2 ConnectorPosition(Direction direction, float position)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector2(position, ConnectorBlockedPosition);
            case Direction.Left:
                return new Vector2(-ConnectorBlockedPosition, position);
            case Direction.Right:
                return new Vector2(ConnectorBlockedPosition, position);
            case Direction.Down:
                return new Vector2(position, -ConnectorBlockedPosition);
            default:
                return Vector2.zero;
        }
    }

    public List<Brick> GetNeighbours()
    {
        var brick = GetBrick();
        return gridManager.grid.GetBrickNeighbours(brick);
    }

    public Brick GetBrick()
    {
        return gridManager.grid.content.Find(b => b.position == (Vector2)transform.localPosition);
    }
}