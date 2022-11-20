using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapVizualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpLeft, wallDiagonalCornerUpRight;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPos)
    {
        PaintTiles(floorPos, floorTilemap, floorTile);
        
    }

    public bool IsFloor(Vector2Int pos)
    {
        /*return floorTilemap.GetTile(floorTilemap.WorldToCell((Vector3Int)pos)).name.Equals("sprites_23") ||
            wallTilemap.GetTile(wallTilemap.WorldToCell((Vector3Int)pos)).name.Equals("sprites_23");*/
        return floorTilemap.HasTile(floorTilemap.WorldToCell((Vector3Int)pos)) &&
            floorTilemap.HasTile(floorTilemap.WorldToCell((Vector3Int)pos + new Vector3Int(1,0,0))) &&
            floorTilemap.HasTile(floorTilemap.WorldToCell((Vector3Int)pos + new Vector3Int(1, 1, 0))) &&
            floorTilemap.HasTile(floorTilemap.WorldToCell((Vector3Int)pos + new Vector3Int(1, -1, 0))) &&
            floorTilemap.HasTile(floorTilemap.WorldToCell((Vector3Int)pos + new Vector3Int(-1, 0, 0))) &&
            floorTilemap.HasTile(floorTilemap.WorldToCell((Vector3Int)pos + new Vector3Int(-1, 1, 0))) &&
            floorTilemap.HasTile(floorTilemap.WorldToCell((Vector3Int)pos + new Vector3Int(-1, -1, 0))) &&
            floorTilemap.HasTile(floorTilemap.WorldToCell((Vector3Int)pos + new Vector3Int(0, 1, 0))) &&
            floorTilemap.HasTile(floorTilemap.WorldToCell((Vector3Int)pos + new Vector3Int(0, -1, 0)));
    }
    internal void PaintSingleBasicWall(Vector2Int pos, string neighboursBinaryType)
    {
        int typeAsInt = Convert.ToInt32(neighboursBinaryType, 2);
        TileBase tile = null;
        if (WallByteTypes.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallByteTypes.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallByteTypes.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallByteTypes.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallByteTypes.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        if (tile!=null)
            PaintSingleTile(wallTilemap, tile, pos);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var pos in positions)
        {
            PaintSingleTile(tilemap, tile, pos);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos)
    {
        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int pos, string neighboursBinaryType)
    {
        int typeAsInt = Convert.ToInt32(neighboursBinaryType, 2);
        TileBase tile = null;
        if (WallByteTypes.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallByteTypes.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallByteTypes.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallByteTypes.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallByteTypes.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallByteTypes.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallByteTypes.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallByteTypes.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }

        if (tile != null)
            PaintSingleTile(wallTilemap, tile, pos);
    }
}
