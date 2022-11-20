using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPos, TilemapVizualizer tilemapVizualizer)
    {
        HashSet<Vector2Int> basicWallPos = (HashSet<Vector2Int>)FindWallsInDirection(floorPos, Direction2D.cardinalDirectionsList);
        HashSet<Vector2Int> cornerWallsPositions = (HashSet<Vector2Int>)FindWallsInDirection(floorPos, Direction2D.diagonalDirectionsList);
        CreateBasicWall(tilemapVizualizer, basicWallPos, floorPos);
        CreateCornerWalls(tilemapVizualizer, cornerWallsPositions, floorPos);
    }

    private static void CreateCornerWalls(TilemapVizualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
        }
    }

    private static void CreateBasicWall(TilemapVizualizer tilemapVizualizer, HashSet<Vector2Int> basicWallPos, HashSet<Vector2Int> floorPos)
    {
        foreach (var pos in basicWallPos)
        {
            string neighboursBinaryType = "";
            foreach(var direction in Direction2D.cardinalDirectionsList)
            {
                var neighbourPos = pos + direction;

                if (floorPos.Contains(neighbourPos))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }

            tilemapVizualizer.PaintSingleBasicWall(pos, neighboursBinaryType);
        }
    }

    private static object FindWallsInDirection(HashSet<Vector2Int> floorPos, List<Vector2Int> directions)
    {
        HashSet<Vector2Int> wallPos = new HashSet<Vector2Int>();
        foreach(var pos in floorPos)
        {
            foreach(var direction in directions)
            {
                var neighbourPos = pos + direction;
                if (!floorPos.Contains(neighbourPos))
                {
                    wallPos.Add(neighbourPos);
                }
            }
        }
        return wallPos;
    }
}
