using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    protected SimpleDungeonGeneratorSO randomWalkParameters;

    public override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPos = RunRandomWalk(randomWalkParameters, startPos);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPos);
        WallGenerator.CreateWalls(floorPos, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleDungeonGeneratorSO parameters, Vector2Int pos)
    {
        var currentPos = pos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for(int i = 0; i < parameters.iterations; i++)
        {
            var path = GenerationsAlgoritms.SimpleRandomWalk(currentPos, parameters.walkLength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
                currentPos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }
        return floorPositions;
    }
}
