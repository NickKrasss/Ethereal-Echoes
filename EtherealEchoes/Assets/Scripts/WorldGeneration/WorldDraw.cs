using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(WorldObject))]
public class WorldDraw : MonoBehaviour
{
    [Tooltip("Тайлмап для пола и стен")]
    [SerializeField]
    private Tilemap tilemap;

    [Tooltip("Тайлмап для границ (переход между стеной и полом). Sort order: Top left, Mode: individual")]
    [SerializeField]
    private Tilemap borderTilemap;

    [Tooltip("Тайлы для пола")]
    [SerializeField]
    private Tile[] floors;

    [Tooltip("Тайлы для стен")]
    [SerializeField]
    private Tile[] walls;

    [Tooltip("Тайлы для границ. 0-правая 1-левая 2-верхняя 3-нижняя")]
    [SerializeField]
    private Tile[] borders;

    [Tooltip("Префаб для бортиков.")]
    [SerializeField]
    private GameObject border3D;

    [Tooltip("Рисует весь мир сразу. Для тестов.")]
    [SerializeField]
    private bool drawEverything = false;

    private WorldFill worldScr;

    [Tooltip("Радиус отрисовки мира.")]
    [SerializeField]
    private int renderRadius = 10;

    [Tooltip("Обьект игрока.")]
    [SerializeField]
    private GameObject playerObj;

    private Vector2 playerPreviousPos = Vector2.zero;

    public World world;

    private void Update()
    {
        if (!drawEverything)
        {
            if (playerObj == null)
            {
                playerObj = GameObject.FindGameObjectWithTag("Player");
                return;
            }
            if ((int)playerPreviousPos.x != (int)playerObj.transform.position.x || (int)playerPreviousPos.y != (int)playerObj.transform.position.y)
            {
                DrawSector(new Sector(new float[] { (int)playerObj.transform.position.x, (int)playerObj.transform.position.y }, renderRadius*2+6, renderRadius*2+6), renderRadius);
            }
            playerPreviousPos = playerObj.transform.position;
        }
    }

    private void DrawCell(int x, int y)
    {
        if (!World.isCellInBorders(world.Map, x, y)) return;
        if (world.Map[x, y] == 0 || world.Map[x, y] == 2)
        {
            tilemap.SetTile(new Vector3Int(x, y), floors[Random.Range(0, floors.Length)]);
            RotateTile(tilemap, new Vector3Int(x, y), Random.Range(0, 4) * 90);
        }
        else
        {
            borderTilemap.SetTile(new Vector3Int(x, y, 0), walls[Random.Range(0, walls.Length)]);
            tilemap.SetTile(new Vector3Int(x, y), floors[Random.Range(0, floors.Length)]);
            if (x < world.Width - 1 && world.Map[x + 1, y] != 1)
            {
                borderTilemap.SetTile(new Vector3Int(x, y, 1), borders[0]);
                Instantiate(border3D, new Vector3(x + 1, y + 0.5f, -0.35f), Quaternion.Euler(180, 90, 90));
            }
            else borderTilemap.SetTile(new Vector3Int(x, y, 1), null);
            if (x > 0 && world.Map[x - 1, y] != 1)
            {
                borderTilemap.SetTile(new Vector3Int(x, y, 2), borders[1]);
                Instantiate(border3D, new Vector3(x, y + 0.5f, -0.35f), Quaternion.Euler(0, 90, -90));
            }
            else borderTilemap.SetTile(new Vector3Int(x, y, 2), null);
            if (y < world.Height - 1 && world.Map[x, y + 1] != 1)
            {
                borderTilemap.SetTile(new Vector3Int(x, y, 3), borders[2]);
                Instantiate(border3D, new Vector3(x + 0.5f, y + 1f, -0.35f), Quaternion.Euler(90, 0, 0));
            }
            else borderTilemap.SetTile(new Vector3Int(x, y, 3), null);
            if (y > 0 && world.Map[x, y - 1] != 1)
            {
                borderTilemap.SetTile(new Vector3Int(x, y, 4), borders[3]);
                Instantiate(border3D, new Vector3(x + 0.5f, y, -0.35f), Quaternion.Euler(-90, -90, -90));
            }
            else borderTilemap.SetTile(new Vector3Int(x, y, 4), null);
        }
    }

    void RotateTile(Tilemap tilemap, Vector3Int tilePosition, int degrees)
    {
        Matrix4x4 matrix = tilemap.GetTransformMatrix(tilePosition);
        matrix *= Matrix4x4.Rotate(Quaternion.Euler(0, 0, degrees));
        tilemap.SetTransformMatrix(tilePosition, matrix);
    }

    private void ClearCell(int x, int y)
    {
        if (!World.isCellInBorders(world.Map, x, y)) return;
        tilemap.SetTile(new Vector3Int(x, y), null);
        borderTilemap.SetTile(new Vector3Int(x, y, 0), null);
        borderTilemap.SetTile(new Vector3Int(x, y, 1), null);
        borderTilemap.SetTile(new Vector3Int(x, y, 2), null);
        borderTilemap.SetTile(new Vector3Int(x, y, 3), null);
    }

    public void ClearAllCells()
    {
        for (int x = 0; x < worldScr.width; x++)
        {
            for (int y = 0; y < worldScr.height; y++)
            {
                ClearCell(x, y);
            }
        }
    }

    public void DrawEverything()
    {
        if (drawEverything)
        {
            for (int x = 0; x < world.Width; x++)
            {
                for (int y = 0; y < world.Height; y++)
                {
                    ClearCell(x, y);
                    DrawCell(x, y);
                }
            }
        }
    }

    public void DrawSector(Sector s)
    {
        for (int x = s.x1; x < s.x2; x++)
        {
            for (int y = s.y1; y < s.y2; y++)
            {
                ClearCell(x, y);
                DrawCell(x, y);
            }
        }
    }

    public void DrawSector(Sector s, int radius)
    {
        for (int x = s.x1; x < s.x2; x++)
        {
            for (int y = s.y1; y < s.y2; y++)
            {
                ClearCell(x, y);
                if (x <= s.center[0] + radius && x >= s.center[0] - radius && y <= s.center[1] + radius && y >= s.center[1] - radius)
                    DrawCell(x, y);
            }
        }
    }
}
