using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(WorldFill))]
public class WorldDraw : MonoBehaviour
{
    [Tooltip("������� ��� ���� � ����")]
    [SerializeField]
    private Tilemap tilemap;

    [Tooltip("������� ��� ������ (������� ����� ������ � �����). Sort order: Top left, Mode: individual")]
    [SerializeField]
    private Tilemap borderTilemap;

    [Tooltip("����� ��� ����")]
    [SerializeField]
    private Tile[] floors;

    [Tooltip("����� ��� ����")]
    [SerializeField]
    private Tile[] walls;

    [Tooltip("����� ��� ������. 0-������ 1-����� 2-������� 3-������")]
    [SerializeField]
    private Tile[] borders;

    [Tooltip("������ ���� ��� �����. ��� ������.")]
    [SerializeField]
    private bool drawEverything = false;

    private WorldFill worldScr;

    [Tooltip("������ ��������� ����.")]
    [SerializeField]
    private int renderRadius = 10;

    [Tooltip("������ ������.")]
    [SerializeField]
    private GameObject playerObj;

    private Vector2 playerPreviousPos = Vector2.zero;

    private void Start()
    {
        worldScr = GetComponent<WorldFill>();
    }

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
                DrawSector(new Sector(new int[] { (int)playerObj.transform.position.x, (int)playerObj.transform.position.y }, renderRadius*2+6, renderRadius*2+6), renderRadius);
            }
            playerPreviousPos = playerObj.transform.position;
        }
    }

    private void DrawCell(int x, int y)
    {
        if (!worldScr.isCellInBorders(x, y)) return;
        if (worldScr.world[x, y] != 0)
        {
            Random.InitState(x+y);
            tilemap.SetTile(new Vector3Int(x, y), floors[Random.Range(0, floors.Length)]);
        }
        else
        {
            borderTilemap.SetTile(new Vector3Int(x, y, -1), walls[Random.Range(0, walls.Length)]);
            if (x < worldScr.width - 1 && worldScr.world[x + 1, y] != 0) borderTilemap.SetTile(new Vector3Int(x, y, 0), borders[0]);
            else borderTilemap.SetTile(new Vector3Int(x, y, 0), null);
            if (x > 0 && worldScr.world[x - 1, y] != 0) borderTilemap.SetTile(new Vector3Int(x, y, 1), borders[1]);
            else borderTilemap.SetTile(new Vector3Int(x, y, 1), null);
            if (y < worldScr.height - 1 && worldScr.world[x, y + 1] != 0) borderTilemap.SetTile(new Vector3Int(x, y, 2), borders[2]);
            else borderTilemap.SetTile(new Vector3Int(x, y, 2), null);
            if (y > 0 && worldScr.world[x, y - 1] != 0) borderTilemap.SetTile(new Vector3Int(x, y, 3), borders[3]);
            else borderTilemap.SetTile(new Vector3Int(x, y, 3), null);
        }
    }
    private void ClearCell(int x, int y)
    {
        if (!worldScr.isCellInBorders(x, y)) return;
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
            if (worldScr == null) worldScr = GetComponent<WorldFill>();
            for (int x = 0; x < worldScr.width; x++)
            {
                for (int y = 0; y < worldScr.height; y++)
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