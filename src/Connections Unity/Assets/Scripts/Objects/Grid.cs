using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Objects
{
    [Serializable]
    public class Grid
    {
        public int height = 4;
        public int width = 4;
        public List<Brick> content;

        public void GenerateLevel(int seed, Difficulty difficulty)
        {
            var random = new Random(seed);
            var gridSize = difficulty.GridSize();
            height = (int)gridSize.x;
            width = (int)gridSize.y;
            content = new List<Brick>();

            var initialBrick = new Brick
            {
                position = RandomizeBrickPosition(Vector2.zero, random, true, true),
                type = BrickTypeHelper.RandomType(random, difficulty)
            };
            content.Add(initialBrick);

            var positions = GetBrickEmptyNeighbours(initialBrick);
            
            var brickNumber = difficulty.BrickNumber();
            var stop = false;
            while (positions.Count > 0 && !stop)
            {
                var randomIndex = random.Next(0, positions.Count);
                var nextPosition = positions[randomIndex];
                positions.Remove(nextPosition);

                var nextBrick = new Brick
                {
                    position = nextPosition,
                    type = BrickTypeHelper.RandomType(random, difficulty)
                };
                content.Add(nextBrick);

                var neighbours = GetBrickNeighbours(nextBrick);
                foreach (var neighbour in neighbours)
                {
                    var neighbourDirection = GetNeighbourDirection(nextBrick, neighbour);
                    var connectors = random.Next(1, 5);
                    
                    nextBrick.connectors.Add(new Connector
                    {
                        direction = neighbourDirection,
                        size = connectors
                    });
                    neighbour.connectors.Add(new Connector
                    {
                        direction = neighbourDirection.OppositeDirection(),
                        size = connectors
                    });
                }
                
                positions.AddRange(GetBrickEmptyNeighbours(nextBrick).Where(p => !positions.Contains(p)));

                stop = content.Count >= brickNumber;
            }
        }

        private Direction GetNeighbourDirection(Brick nextBrick, Brick neighbour)
        {
            if (nextBrick.position.x > neighbour.position.x)
                return Direction.Left;
            if (nextBrick.position.x < neighbour.position.x)
                return Direction.Right;
            if (nextBrick.position.y > neighbour.position.y)
                return Direction.Down;
            if (nextBrick.position.y < neighbour.position.y)
                return Direction.Up;

            return Direction.Down;
        }

        public void RandomizeLevel(int seed)
        {
            var random = new Random(seed);
            foreach (var brick in content)
            {
                RandomizeBrick(brick, random);
            }
        }

        public bool IsBrickInPosition(Vector2 position)
        {
            return content.Exists(b => b.position == position);
        }

        public bool IsPositionOutOfBounds(Vector2 position)
        {
            return position.x < 0 || position.x >= width || position.y < 0 || position.y >= height;
        }

        private List<Vector2> GetBrickEmptyNeighbours(Brick brick)
        {
            var positions = GetBrickNeighbourPositions(brick)
                .Where(p => !content.Exists(b => b.position == p))
                .Where(p => !IsPositionOutOfBounds(p))
                .ToList();

            return positions;
        }

        public List<Brick> GetBrickNeighbours(Brick brick)
        {
            return GetBrickNeighbourPositions(brick)
                .Select(p => content.Find(b => b.position == p))
                .Where(b => b != null)
                .ToList();
        }

        private List<Vector2> GetBrickNeighbourPositions(Brick brick)
        {
            return new List<Vector2>
            {
                GetBrickNeighbourPosition(brick, Direction.Down),
                GetBrickNeighbourPosition(brick, Direction.Left),
                GetBrickNeighbourPosition(brick, Direction.Right),
                GetBrickNeighbourPosition(brick, Direction.Up)
            };
        }

        private Vector2 GetBrickNeighbourPosition(Brick brick, Direction neighbourDirection)
        {
            var position = brick.position;

            switch (neighbourDirection)
            {
                case Direction.Down:
                    position.y -= 1;
                    break;
                case Direction.Right:
                    position.x += 1;
                    break;
                case Direction.Up:
                    position.y += 1;
                    break;
                case Direction.Left:
                    position.x -= 1;
                    break;
            }

            return position;
        }

        private Vector2 RandomizeBrickPosition(Vector2 basePosition, Random random, bool isVertical, bool isHorizontal)
        {
            var position = basePosition;
            do
            {
                if (isHorizontal)
                {
                    var randomX = random.Next(0, width);
                    position.x = randomX;
                }

                if (isVertical)
                {
                    var randomY = random.Next(0, height);
                    position.y = randomY;
                }
            } while (IsBrickInPosition(position) && (isHorizontal || isVertical));

            return position;
        }

        private void RandomizeBrick(Brick brick, Random random)
        {
            var position = brick.position;
            var facingDirection = brick.facingDirection;

            if (brick.IsHorizontal || brick.IsVertical)
            {
                position = RandomizeBrickPosition(brick.position, random, brick.IsVertical, brick.IsHorizontal);
            }
            
            if (brick.IsRotation)
            {
                facingDirection = DirectionHelper.RandomDirection(random);
            }

            brick.position = position;
            brick.facingDirection = facingDirection;
        }
    }
}
