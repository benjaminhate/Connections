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

        public void GenerateLevel(int seed)
        {
            var random = new Random(seed);
            content = new List<Brick>();

            for (var i = 0; i < 8; i++)
            {
                var brickType = (BrickType) random.Next(0, 8);
                
                content.Add(new Brick
                {
                    position = RandomizeBrickPosition(Vector2.zero, random, true, true),
                    type = brickType,
                    facingDirection = RandomizeRotation(random)
                });
            }
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

        public List<Brick> GetBrickNeighbours(Brick brick)
        {
            var neighbours = new List<Brick>();
            
            neighbours.AddRange(new []
            {
                GetBrickNeighbour(brick, Direction.Down),
                GetBrickNeighbour(brick, Direction.Left),
                GetBrickNeighbour(brick, Direction.Right),
                GetBrickNeighbour(brick, Direction.Up)
            });

            return neighbours.Where(b => b != null).ToList();
        }

        private Brick GetBrickNeighbour(Brick brick, Direction neighbourDirection)
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

            return content.Find(b => b.position == position);
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

        private Direction RandomizeRotation(Random random)
        {
            var randomDirection = random.Next(0, 4);
            return (Direction) randomDirection;
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
                facingDirection = RandomizeRotation(random);
            }

            brick.position = position;
            brick.facingDirection = facingDirection;
        }
    }
}
