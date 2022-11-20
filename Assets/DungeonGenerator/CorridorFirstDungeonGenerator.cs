using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.UI;

public class CorridorFirstDungeonGenerator : SimpleDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    public static List<GameObject> enemies = new List<GameObject>();
    private float roomPercent = 0.8f;
    public GameObject[] enemie;
    public NavMeshSurface2d surface;
    GameObject player;
    GameObject pet;
    public Text remainEnemies;
    public GameObject[] itens;
    public static List<GameObject> itensList = new List<GameObject>();
    HashSet<Vector2Int> roomPositionsss = new HashSet<Vector2Int>();

    private void Update()
    {
        //SpawnManager.countEnemies = enemies.Count()
        Debug.Log(enemies.Count() + " -- " + SpawnManager.countEnemies);

        remainEnemies.text = SpawnManager.countEnemies + " Enemies Left";
        Debug.Log(8 + Portal.dungeonLevel * 1.5);
    }

    public override void RunProceduralGeneration()
    {

        tilemapVisualizer.Clear();
        for (int i = 0; i < enemies.Count(); i++) {
            SpawnManager.enemies.Remove(enemies[i]);
            Destroy(enemies[i]);
            enemies.RemoveAt(i);
        }

        for (int i = 0; i < itensList.Count(); i++)
        {
            //SpawnItensManager.countItems--;
            SpawnItensManager.itemsInScene.Remove(itensList[i]);
            Destroy(itensList[i]);
            itensList.RemoveAt(i);
        }


        surface.RemoveData();
        player = GameObject.Find("Player");
        pet = GameObject.Find("Pet");
        CorridorFirstGenerator();
        player.transform.position = new Vector3(-50, 150, -3.831578f);
        pet.transform.position = new Vector3(-50, 150, -3.831578f);
        //surface.BuildNavMesh();
        //NavMeshBuilder.ClearAllNavMeshes();
        surface.BuildNavMeshAsync();
        

        foreach (var pos in roomPositionsss)
        {
            Debug.Log(tilemapVisualizer.IsFloor(pos));
            if (tilemapVisualizer.IsFloor(pos) && Vector2.Distance(pos, startPos) > 20)
            {
                if (Random.Range(0, 100) < 50)
                {
                    if (Random.Range(0, 100) < 8 + Portal.dungeonLevel * 1.5)
                    {
                        GameObject enem = Instantiate(enemie[Random.Range(0, enemie.Length)], new Vector3(pos.x, pos.y, 0), Quaternion.identity);
                        enemies.Add(enem);
                        SpawnManager.enemies.Add(enem);
                        SpawnManager.countEnemies++;
                    }
                }
                else
                {
                    if (Random.Range(0, 100) < 5)
                    {
                        GameObject item = Instantiate(itens[Random.Range(0, itens.Length)], new Vector3(pos.x, pos.y, 0), Quaternion.identity);
                        itensList.Add(item);
                    }
                }
            }
        }

        remainEnemies.gameObject.transform.parent.gameObject.SetActive(true);
        remainEnemies.text = SpawnManager.countEnemies + " Enemies Left";
    }

    public void CorridorFirstGenerator()
    {
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potencialRoomPos = new HashSet<Vector2Int>();
        
        CreateCorridors(floorPos, potencialRoomPos);

        HashSet<Vector2Int> roomPos = CreateRooms(potencialRoomPos);
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPos);

        CreateRoomsAtDeadEnd(deadEnds, roomPos);

        floorPos.UnionWith(roomPos);

        tilemapVisualizer.PaintFloorTiles(floorPos);
        WallGenerator.CreateWalls(floorPos, tilemapVisualizer);
    }
    //12
    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach(var pos in deadEnds)
        {
            if (!roomFloors.Contains(pos))
            {
                var room = RunRandomWalk(randomWalkParameters, pos);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPos)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach(var pos in floorPos)
        {
            int neighboursCount = 0;
            foreach(var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPos.Contains(pos + direction))
                    neighboursCount++;
            }
            if(neighboursCount == 1)
            {
                deadEnds.Add(pos);
            }
        }
        return deadEnds; 
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potencialRoomPos)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potencialRoomPos.Count * roomPercent);

        List<Vector2Int> roomToCreate = potencialRoomPos.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        foreach (var roomPos in roomToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPos);
            roomPositions.UnionWith(roomFloor);
        }
        roomPositionsss = roomPositions;
        
        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPos, HashSet<Vector2Int> potencialRoomPos)
    {
        var currentPos = startPos;
        potencialRoomPos.Add(currentPos);
        for(int i = 0; i < corridorCount; i++)
        {
            var corridor = GenerationsAlgoritms.RandomWalkCorridor(currentPos, corridorLength);
            currentPos = corridor[corridor.Count - 1];
            potencialRoomPos.Add(currentPos);
            floorPos.UnionWith(corridor);
        }
    }
}
